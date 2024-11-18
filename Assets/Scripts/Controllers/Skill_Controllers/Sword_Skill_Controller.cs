using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;
    private bool canRotate = true;
    private bool isReturning;
    private float freezeTimeDuration;
    private float returnSpeed;

    [Header("Pierce info")]
    private int amountOfPierce;

    [Header("Bounce info")]
    private float bounceSpeed;
    private bool isBouncing;
    private int amountOfBounce;
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;
    private float hitTimer;
    private float hitCooldown;


    private void Awake() {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void Update() {

        if (canRotate)
            transform.right = rb.linearVelocity;

        if (isReturning) {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }

        BounceLogic();
        SpinLogic();
    }

    private void DestroyMe() {
        Destroy(gameObject);
    }

    private void StopWhenSpinning() {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void SpinLogic() {
        if (isSpinning) {
            if (Vector2.Distance(transform.position, player.transform.position) > maxTravelDistance && !wasStopped) {
                StopWhenSpinning();
            }

            if (wasStopped) {
                spinTimer -= Time.deltaTime;
                hitTimer -= Time.deltaTime;

                if (spinTimer <= 0) {
                    isSpinning = false;
                    isReturning = true;
                }

                if (hitTimer <= 0) {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null) {
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                        }
                    }
                }
            }
                

        }
    }

    private void BounceLogic() {
        if (isBouncing && enemyTarget.Count > 0) {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].transform.position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].transform.position) < .1f){
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());
                targetIndex++;
                amountOfBounce--;

                if (amountOfBounce <= 0) {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed) {
        rb.linearVelocity = _dir;
        rb.gravityScale = _gravityScale;
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;

        if (amountOfPierce <= 0)
            anim.SetBool("Rotation", true);

        Invoke("DestroyMe", 7);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounce, float _bounceSpeed) {
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounce;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int _amountOfPierce) {
        amountOfPierce = _amountOfPierce;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown) {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    public void ReturnSword() {
        isReturning = true;
        transform.parent = null;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (isReturning)
            return;
        
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null) {
            SwordSkillDamage(enemy);
        }
        

        SetupTargetsForBounce(collision);
        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy) {
        player.stats.DoDamage(enemy.GetComponent<CharacterStats>());
        enemy.FreezeTimeFor(freezeTimeDuration);

        // Add equipped Amulet damage/effect
        ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipmentByType(EquipmentType.Amulet);

        if (equipedAmulet != null)
            equipedAmulet.Effect(enemy.transform);
    }

    private void SetupTargetsForBounce(Collider2D collision) {
        if (collision.GetComponent<Enemy>() != null) {
            if (isBouncing && enemyTarget.Count <= 0) {

                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null) {
                        enemyTarget.Add(hit.transform);
                    }
                }

            }
        }
    }

    private void StuckInto(Collider2D collision) {

        if (amountOfPierce > 0 && collision.GetComponent<Enemy>() != null) {
            amountOfPierce--;
            
            return;
        }

        if (isSpinning) {
            StopWhenSpinning();
            return;
        };

        canRotate = false;
        cd.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        
        if (isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }

    protected virtual void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, 10);
    }
}
