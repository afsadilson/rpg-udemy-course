using UnityEngine;

enum StatType {
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    criticalChance,
    criticalPower,
    health,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightningDamage
}

[CreateAssetMenu(fileName = "Buff_Effect", menuName = "Data/Item Effect/Buff")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;

    public override void ExecuteEffect(Transform _enemyPosition) {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.IncreaseStatBy(buffAmount, buffDuration, StatToModify());
    }

    private Stat StatToModify() {
        switch (buffType) {
            case StatType.strength: return stats.strength;
            case StatType.agility: return stats.agility;
            case StatType.intelligence: return stats.intelligence;
            case StatType.vitality: return stats.vitality;
            case StatType.damage: return stats.damage;
            case StatType.criticalChance: return stats.criticalChance;
            case StatType.criticalPower: return stats.criticalPower;
            case StatType.health: return stats.maxHealth;
            case StatType.armor: return stats.armor;
            case StatType.evasion: return stats.evasion;
            case StatType.magicResistance: return stats.magicResistance;
            case StatType.fireDamage: return stats.fireDamage;
            case StatType.iceDamage: return stats.iceDamage;
            case StatType.lightningDamage: return stats.lightningDamage;
            default: return null;
        }
    }
}
