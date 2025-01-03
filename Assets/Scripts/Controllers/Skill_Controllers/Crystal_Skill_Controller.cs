using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Player player;
    private CircleCollider2D cd;
    private Animator anim;
    private Transform closestEnemy;
    private float crystalExistsTimer;
    private bool canGrow;
    private float growSpeed;
    private bool canExplode;
    private bool canMoveToEnemy;
    private float moveSpeed;
    [SerializeField] private LayerMask whatIsEnemy;
    

    private void Awake() {
        cd = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
        crystalExistsTimer -= Time.deltaTime;

        if (crystalExistsTimer <= 0) {
            FinishCrystal();
        }

        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);

        if (canMoveToEnemy && closestEnemy != null) {
            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestEnemy.position) < 1) {
                FinishCrystal();
                canMoveToEnemy = false;
            }
        }
    }

    public void SetupCrystal(float _crystalExistsDuration, bool _canExplode, bool _canMoveToEnemy, float _moveSpeed, float _growSpeed, Transform _closestEnemy, Player _player) {
        player = _player;
        crystalExistsTimer = _crystalExistsDuration;
        canExplode = _canExplode;
        canMoveToEnemy = _canMoveToEnemy;
        moveSpeed = _moveSpeed;
        growSpeed = _growSpeed;
        closestEnemy = _closestEnemy;
    }

    public void ChooseRandomEnemy() {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
        if (colliders.Length > 0)
            closestEnemy = colliders[Random.Range(0, colliders.Length)].transform;
    }

    public void SelfDestroy() => Destroy(gameObject);
    
    private void AnimationExplodeEvent() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders) {
            if (hit.GetComponent<Enemy>() != null) {
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());

                // Add equipped Amulet damage/effect
                ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipmentByType(EquipmentType.Amulet);

                if (equipedAmulet != null)
                    equipedAmulet.Effect(hit.transform);
            }
        }
    }

    public void FinishCrystal() {
        if (canExplode) {
            canGrow = true;
            anim.SetTrigger("Explode");
        } else {
            SelfDestroy();
        }
    }
}
