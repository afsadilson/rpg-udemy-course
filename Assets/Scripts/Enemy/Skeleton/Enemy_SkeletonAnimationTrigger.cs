using UnityEngine;

public class Enemy_SkeletonAnimationFinishTrigger: MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    private void AnimationTrigger() {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger() {
        Collider2D collider = Physics2D.OverlapCircle(enemy.attackCheck.position, enemy.attackCheckRadius);
        if (collider.GetComponent<Player>() != null)
            collider.GetComponent<Player>().Damage();
    }

    private void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindow();

    private void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();
}
