using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;
    
    public void UpdateSlot(InventoryItem _newItem) {
        item = _newItem;
        
        if (item != null) {
            itemImage.color = Color.white;
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1) {
                itemText.text = item.stackSize.ToString();
            } else {
                itemText.text = "";
            }
        }
    }
}
