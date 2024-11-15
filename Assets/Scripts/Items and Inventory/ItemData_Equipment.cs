using UnityEngine;

public enum EquipmentType {
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Major Stats")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;

    [Header("Offensive Stats")]
    public int damage;
    public int criticalChance;
    public int criticalPower;

    [Header("Defensive Stats")]
    public int health;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic Stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;
    

    public void AddModifiers() {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddMofifier(strength);
        playerStats.agility.AddMofifier(agility);
        playerStats.intelligence.AddMofifier(intelligence);
        playerStats.vitality.AddMofifier(vitality);
        playerStats.damage.AddMofifier(damage);
        playerStats.criticalChance.AddMofifier(criticalChance);
        playerStats.criticalPower.AddMofifier(criticalPower);
        playerStats.maxHealth.AddMofifier(health);
        playerStats.armor.AddMofifier(armor);
        playerStats.evasion.AddMofifier(evasion);
        playerStats.magicResistance.AddMofifier(magicResistance);
        playerStats.fireDamage.AddMofifier(fireDamage);
        playerStats.iceDamage.AddMofifier(iceDamage);
        playerStats.lightningDamage.AddMofifier(lightningDamage);
    }
    
    public void RemoveModifiers() {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);
        playerStats.damage.RemoveModifier(damage);
        playerStats.criticalChance.RemoveModifier(criticalChance);
        playerStats.criticalPower.RemoveModifier(criticalPower);
        playerStats.maxHealth.RemoveModifier(health);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);
        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
    }
}
