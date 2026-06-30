using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    public int id;
    public string ItemName;
    public Sprite Icon;
    public GameObject ObjectHand;
    public GameObject ObjectWorld;
    public void Use()
    {
        
    }
}

