using UnityEngine;

[CreateAssetMenu(fileName = "FreezyEnemies_Effect", menuName = "Data/Item Effect/Freeze Enemies")]
public class FreezeEnemies_Effect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _transform) {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * .1f)
            return;

        // Check Cooldown
        if (!Inventory.instance.CanUseArmor())
            return;


        int radius = 2;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, radius);

        foreach (var hit in colliders) {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
