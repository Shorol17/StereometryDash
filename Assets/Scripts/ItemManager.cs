using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Inventory inventory;
    public ItemDatabase itemDatabase;
    public ObjectDatabase objectDatabase;
    public GameObject Source;
    public GameObject BlastPoint;
    public GameObject Camera;
    public ItemSpawnerMenu itemSpawnerMenu;
    public ObjectSpawnerMenu objectSpawnerMenu;
    public ParticleSystem shootEffect;
    public GameObject PhysicsGunBeam;
    private GameObject PhysicsGunSelected;
    private bool UsingPhysicsGun = false;

    public void UseLMBdown()
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
                UsePhysicsGunDown();
                break;
            case 4:
                UseObjectSpawner();
                break;
        }
    }

    public void UseLMBup()
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
                UsePhysicsGunUp();
                break;
            case 4:
                break;
        }
    }

    public void UseMMB()
    {
        int id = inventory.MouseSlot;
        if(id == 0) return;
        
        switch (id)
        {
            case 1:
                itemSpawnerMenu.SwitchMenu();
                break;
            case 2:
                break;
            case 3:
                UsePhysicsGunMMB();
                break;
            case 4:
                objectSpawnerMenu.SwitchMenu();
                break;
        }
    }

    void Update()
    {
        var beam = PhysicsGunBeam.GetComponent<LineRenderer>();
        beam.positionCount = 2;
        if (UsingPhysicsGun)

        {
            Ray ray = new Ray(Source.transform.position, Camera.transform.forward);
            RaycastHit hit;

            beam.SetPosition(0, BlastPoint.transform.position);
            if (Physics.Raycast(ray, out hit, 100))
                beam.SetPosition(1, hit.point);
            else
                beam.SetPosition(1, BlastPoint.transform.position + Camera.transform.forward*100);
        }
        else
        {
            beam.SetPosition(0, new Vector3(0,0,0));
            beam.SetPosition(1, new Vector3(0,0,0));
        }
    }

    void UseSpawnItemToolSing()
    {
        int id = itemSpawnerMenu.SelectedItemId;
        GameObject spawned = Instantiate(itemDatabase.GetItem(id).ObjectWorld, Source.transform.position + Camera.transform.forward, Camera.transform.rotation);
        Rigidbody rb = spawned.GetComponent<Rigidbody>();
        rb.AddForce(Camera.transform.forward * 8f, ForceMode.Impulse);
    }

    void UseDeleteToolSing()
    {
        shootEffect.Play();
        shootEffect.transform.rotation = Camera.transform.rotation;

        Ray ray = new Ray(Source.transform.position, Camera.transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100))
        {
            Destroy(hit.collider.gameObject);
        }
    }

    void UsePhysicsGunDown()
    {
        UsingPhysicsGun = true;

        Ray ray = new Ray(Source.transform.position, Camera.transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100))
        {
            GameObject selected = hit.collider.gameObject;
            PhysicsGunSelected = selected;
            selected.transform.SetParent(Camera.transform);

            Rigidbody rb = selected.GetComponent<Rigidbody>();
            Collider col = selected.GetComponent<Collider>();

            if (selected.GetComponent<Rigidbody>() != null)
            {
                rb.useGravity = false;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            col.enabled = false;
        }
    }

    void UsePhysicsGunUp()
    {
        if (UsingPhysicsGun == false)
            return;

        UsingPhysicsGun = false;

        GameObject selected = PhysicsGunSelected;
        PhysicsGunSelected = null;

        Rigidbody rb = selected.GetComponent<Rigidbody>();
        Collider col = selected.GetComponent<Collider>();
        Rigidbody rbs = Source.GetComponent<Rigidbody>();

        selected.transform.SetParent(null);
        if (rb != null){
            rb.useGravity = true;
            rb.linearVelocity = rbs.linearVelocity;
        }
        col.enabled = true;
    }

    void UsePhysicsGunMMB()
    {
        UsingPhysicsGun = false;

        GameObject selected = PhysicsGunSelected;
        PhysicsGunSelected = null;

        Rigidbody rb = selected.GetComponent<Rigidbody>();
        Collider col = selected.GetComponent<Collider>();

        selected.transform.SetParent(null);

        if (rb != null)
            rb.useGravity = true;
            if (rb.constraints == RigidbodyConstraints.None)
                rb.constraints = RigidbodyConstraints.FreezeAll;
            else
                rb.constraints = RigidbodyConstraints.None;
        col.enabled = true;
    }

    void UseObjectSpawner()
    {
        GameObject prefab = objectDatabase.objects[objectSpawnerMenu.dropdown.value];

        Ray ray = new Ray(Source.transform.position, Camera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 20f))
        {
            GameObject spawned = Instantiate(prefab, hit.point, Quaternion.identity);
            if (spawned.GetComponent<Rigidbody>() == null)
                spawned.AddComponent<Rigidbody>();
            if (spawned.GetComponent<Collider>() == null)
                spawned.AddComponent<Collider>();

            Collider col = spawned.GetComponent<Collider>();
            spawned.transform.position += hit.normal*col.bounds.extents.y;
        }
    }
}
