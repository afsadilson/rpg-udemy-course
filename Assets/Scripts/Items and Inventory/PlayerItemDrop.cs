
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
  [Header("Player's Drop")]
  [SerializeField] private float chanceToLooseItems;
  [SerializeField] private float chanceToLooseMaterials;

    public override void GenerateDrop() {
      Inventory inventory = Inventory.instance;

      List<InventoryItem> currentStash = inventory.GetStashList();
      List<InventoryItem> currentEquipment = inventory.GetEquipmentList();
      List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
      List<InventoryItem> materialsToLoose = new List<InventoryItem>();

      foreach (InventoryItem item in currentEquipment) {
        if (Random.Range(0, 100) <= chanceToLooseItems) {
          DropItem(item.data);
          itemsToUnequip.Add(item);
        }
      }
      
      foreach (var item in itemsToUnequip) {
        inventory.UnequipItem(item.data as ItemData_Equipment); 
      }

      foreach (InventoryItem item in currentStash) {
        if (Random.Range(0, 100) <= chanceToLooseMaterials) {
          DropItem(item.data);
          materialsToLoose.Add(item);
        }
      }

      foreach (var item in materialsToLoose) {
        inventory.RemoveItem(item.data); 
      }
    }
}