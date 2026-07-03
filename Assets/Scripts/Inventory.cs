using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int[] slots;
    public int MouseSlot;
    public HandItemRenderer handItemRenderer;
    public void InteractSlot(int index)
    {
        int temp = MouseSlot;
        MouseSlot = slots[index-1];
        slots[index-1] = temp;
        handItemRenderer.Refresh();
    }
}