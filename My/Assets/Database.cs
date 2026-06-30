using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public ItemData[] items;
    public ItemData GetItem(int id)
    {
        foreach (var item in items)
        {
            if (item.id == id)
                return item;
        }
        return  null;
    }
}