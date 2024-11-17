using UnityEngine;

[CreateAssetMenu(fileName = "Heal_Effect", menuName = "Data/Item Effect/Heal")]
public class Heal_Effect : ItemEffect
{
    //[SerializeField] private GameObject iceAndFirePrefab;
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;

    public override void ExecuteEffect(Transform _enemyPosition) {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);

        playerStats.IncreaseHealthBy(healAmount);
    }
}
