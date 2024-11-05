using UnityEngine;

public class ShockStrike_Controller : MonoBehaviour
{
    [SerializeField] private float speed;

    private CharacterStats targetStats;
    private Animator anim;
    private bool triggered;
    private int damage;

    void Start() {
        anim = GetComponentInChildren<Animator>();
    }

    public void Setup(int _damage, CharacterStats _targetStats) {
        damage = _damage;
        targetStats = _targetStats;
    }

    void Update() {
        if (triggered)
            return;

        if (!targetStats)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;
        
        if (Vector2.Distance(transform.position, targetStats.transform.position) < .25f) {
            anim.transform.localRotation = Quaternion.identity;
            anim.transform.localPosition = new Vector3(0, .5f);
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);
            
            Invoke("DamageSelfDestroy", .2f);
            triggered = true;
            anim.SetTrigger("Hit");
        }
    }

    private void DamageSelfDestroy() {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject, .4f);
    }
}
