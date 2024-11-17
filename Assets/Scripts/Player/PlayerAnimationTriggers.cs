using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger() {
        player.AnimationTrigger();
    }

    private void AttackTrigger() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders) {
            if (hit.GetComponent<Enemy>() != null) {
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                player.stats.DoDamage(_target);

                Inventory.instance.GetEquipmentByType(EquipmentType.Weapon)?.Effect(_target.transform);
            }
        }
    }

    private void ThrowSword() {
        SkillManager.instance.sword.CreateSword();
    }
}
