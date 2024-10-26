using System.Collections.Generic;
using UnityEngine;


public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;
    private bool canGrow = true;
    private float maxSize;
    private float growSpeed;
    private float blackholeTimer;
    
    private bool canShrink;
    private float shrinkSpeed;

    private bool canCreateHotKeys = true;
    private bool cloneAttackReleased;
    private bool playerCanDisapear = true;
    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKeys = new List<GameObject>();

    public bool playerCanExitState { get; private set; }

    private void Update() {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0) {
            blackholeTimer = Mathf.Infinity;

            if (targets.Count > 0) {
                ReleaseCloneAttack();
            } else {
                FinishBlackholeAbility();
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink) {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink) {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x <= 0)
                Destroy(gameObject);
        }
    }

    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeDuration) {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeDuration;

        if (SkillManager.instance.clone.crystalInsteadOfClone)
            playerCanDisapear = false;
    }

    private void ReleaseCloneAttack() {
        if (targets.Count <= 0) return;

        DestroyHotKeys();
        cloneAttackReleased = true;
        canCreateHotKeys = false;
        
        if (playerCanDisapear) {
            playerCanDisapear = false;
            PlayerManager.instance.player.MakeTransparent(true);
        }
    }

    private void CloneAttackLogic() {
        if (cloneAttackTimer <= 0 && cloneAttackReleased && amountOfAttacks > 0) {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);
            float xOffset = Random.Range(0, 100) > 50 ? 2 : -2;

            if (SkillManager.instance.clone.crystalInsteadOfClone) {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            } else {
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            }

            amountOfAttacks--;

            if (amountOfAttacks <= 0) {
                Invoke("FinishBlackholeAbility", 1f);
            }
        }
    }

    private void FinishBlackholeAbility() {
        DestroyHotKeys();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
    }

    private void DestroyHotKeys() {
        if (createdHotKeys.Count <= 0) return;

        for (int i = 0; i < createdHotKeys.Count; i++)
        {
            Destroy(createdHotKeys[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Enemy>() != null) {
            collision.GetComponent<Enemy>().FreezeTime(true);
            
            CreateHotKey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<Enemy>() != null) {
            collision.GetComponent<Enemy>().FreezeTime(false);
            
            DestroyHotKeys();
        }
    }

    private void CreateHotKey(Collider2D collision) {
        if (keyCodeList.Count <= 0) {
            Debug.LogWarning("keyCodeList is empty on Blackhole_Skill_Controller");
            return;
        }

        if (!canCreateHotKeys) return;

        KeyCode chosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(chosenKey);
        
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKeys.Add(newHotKey);

        Blackhole_HotKey_Skill_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_HotKey_Skill_Controller>();
        newHotKeyScript.SetupHotKey(chosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);

}
