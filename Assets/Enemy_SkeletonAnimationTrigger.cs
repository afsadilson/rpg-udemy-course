using UnityEngine;

public class Enemy_SkeletonAnimationFinishTrigger: MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    private void AnimationTrigger() {
        enemy.AnimationFinishTrigger();
    }
}
