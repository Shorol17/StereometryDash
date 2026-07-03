using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
    {
        public Inventory inventory;
        public ItemDatabase database;
        public Image[] slotImages;
        public Sprite EmptySlotSprite;

        void Update()
        {
            for (int i = 0; i<27; i++)
            {
                ItemData item = database.GetItem(inventory.slots[i]);
                slotImages[i].enabled = true;
                slotImages[i].sprite = item.Icon;
            }
        }

        public void OnSlotCliched(int index)
        {
            inventory.InteractSlot(index);
        }
    }