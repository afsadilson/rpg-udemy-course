using UnityEngine;

public class PlayerStats: CharacterStats {

    private Player player;
    private PlayerItemDrop myDropSystem;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
        myDropSystem = GetComponent<PlayerItemDrop>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipmentByType(EquipmentType.Armor);

        if (currentArmor != null)
            currentArmor.Effect(player.transform);
    }

    protected override void Die()
    {
        base.Die();
        player.Dead();

        myDropSystem.GenerateDrop();
    }
}