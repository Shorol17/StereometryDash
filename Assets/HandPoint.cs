using UnityEngine;

public class HandItemRenderer : MonoBehaviour
{
    public Inventory inventory;
    public ItemDatabase database;
    public GameObject Camera;
    private GameObject CurrentObject;

    public void Refresh()
    {
        if (CurrentObject != null)
        {
            Destroy(CurrentObject);
        }
        int id = inventory.MouseSlot;
        ItemData item = database.GetItem(id);
        CurrentObject = Instantiate(item.ObjectHand, transform);
        CurrentObject.transform.localPosition = Vector3.zero;
        CurrentObject.transform.localRotation = Quaternion.identity;
    }

    public void DropItem()
    {
        int id = inventory.MouseSlot;
        if(id == 0)
            return;
        ItemData item = database.GetItem(id);
        GameObject dropped = Instantiate(item.ObjectWorld, transform.position + transform.forward, Quaternion.identity);

        Rigidbody rb = dropped.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Camera.transform.forward * 8f, ForceMode.Impulse);
        }
        inventory.MouseSlot = 0;
        Refresh();
    }
}
