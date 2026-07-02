using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int[] slots = {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
    public int MouseSlot = 0;
    public HandItemRenderer handItemRenderer;

    public void InteractSlot(int index)
    {
        int temp = MouseSlot;
        MouseSlot = slots[index-1];
        slots[index-1] = temp;
        handItemRenderer.Refresh();
    }
}