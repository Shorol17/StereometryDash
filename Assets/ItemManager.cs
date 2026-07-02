using System.Data;
using System.Data.Common;
using System.Linq;
using Unity.Multiplayer.Center.Common;
using UnityEditor.Callbacks;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Inventory inventory;
    public ItemDatabase database;
    public GameObject Source;
    public GameObject Camera;
    private GameObject PhysicsGunSelected;

    public void UseSingular()
    {
        int id = inventory.MouseSlot;
        if(id == 0) return;
        
        switch (id)
        {
            case 1:
                UseSpawnItemToolSing();
                break;
            case 2:
                UseDeleteToolSing();
                break;
            case 3:
                UsePhysicsGunSing();
                break;
        }
    }

    public void UseContinues()
    {
        int id = inventory.MouseSlot;
        if(id == 0) return;
        
        switch (id)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                UsePhysicsGunCont();
                break;
        }
    }

    void UseSpawnItemToolSing()
    {
        GameObject spawned = Instantiate(database.GetItem(2).ObjectWorld, Source.transform.position + Camera.transform.forward, Camera.transform.rotation);
        Rigidbody rb = spawned.GetComponent<Rigidbody>();
        rb.AddForce(Camera.transform.forward * 8f, ForceMode.Impulse);
    }

    void UseDeleteToolSing()
    {
        Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100))
        {
            Destroy(hit.collider.gameObject);
        }
    }

    void UsePhysicsGunSing()
    {
        Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100))
        {
            GameObject selected = hit.collider.gameObject;
            PhysicsGunSelected = selected;
            selected.transform.SetParent(Camera.transform);

            Rigidbody rb = selected.GetComponent<Rigidbody>();
            Collider col = selected.GetComponent<Collider>();

            if (rb != null)
                rb.useGravity = false;
            col.enabled = false;
        }
    }

    void UsePhysicsGunCont()
    {
        if (Input.GetMouseButtonUp(1))
        {
            GameObject selected = PhysicsGunSelected;
            PhysicsGunSelected = null;

            Rigidbody rb = selected.GetComponent<Rigidbody>();
            Collider col = selected.GetComponent<Collider>();

            selected.transform.SetParent(null);
            if (rb != null)
                rb.useGravity = true;
            col.enabled = true;
        }
    }

}
