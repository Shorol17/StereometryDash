using UnityEngine;
using UnityEngine.AI;

public class CubeController2 : MonoBehaviour
{
    public float movespeed;
    public float jumpforce;
    public GameObject Camera;
    public bool NoClip = false;
    public GameObject MainCube;
    public HandItemRenderer handItemRenderer;
    public GameObject handpoint;
    public Inventory inventory;
    public ItemManager itemManager;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void SwitchNoClip(GameObject obj, bool NoClip)
    {
        if (NoClip){
            Rigidbody[] Objects1 = obj.GetComponentsInChildren<Rigidbody>(false);
            Collider[] Objects2 = obj.GetComponentsInChildren<Collider>(false);
            foreach (Rigidbody r in Objects1)
            {
                r.useGravity = false;
            }
            foreach (Collider r in Objects2)
            {
                r.enabled = false;
            }
        }
        else
        {
            Rigidbody[] Objects1 = obj.GetComponentsInChildren<Rigidbody>(true);
            Collider[] Objects2 = obj.GetComponentsInChildren<Collider>(true);
            foreach (Rigidbody r in Objects1)
            {
                r.useGravity = true;
            }
            foreach (Collider r in Objects2)
            {
                r.enabled = true;
            }
        }

    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = (Camera.transform.right * x + Camera.transform.forward * z).normalized;

        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f);

        if (!Input.GetKey(KeyCode.E) && hit.collider && !NoClip)
            rb.linearVelocity += move*movespeed*Time.deltaTime;

        if (NoClip)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            transform.Translate(move*movespeed*Time.deltaTime, Space.World);

            if (Input.GetKey(KeyCode.Space))
                {
                    transform.position += new Vector3(0, movespeed*Time.deltaTime, 0);
                }
            if (Input.GetKey(KeyCode.LeftControl))
                {
                    transform.position -= new Vector3(0, movespeed*Time.deltaTime, 0);
                }
            }
        else 
        {
            if (Input.GetKeyDown(KeyCode.Space) && hit.collider)
            {
                Vector3 normal = new(hit.normal.x, hit.normal.y, hit.normal.z);
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, rb.linearVelocity.z) + normal*jumpforce + move;
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            NoClip = !NoClip;
            SwitchNoClip(MainCube, NoClip);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            movespeed *= 2;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            movespeed /= 2;

        if (Input.GetKeyDown(KeyCode.G))
            handItemRenderer.DropItem();

        if (Input.GetMouseButtonDown(1))
            itemManager.UseLMBdown();
        if (Input.GetMouseButtonUp(1))
            itemManager.UseLMBup();
        if (Input.GetMouseButtonDown(2))
            itemManager.UseMMB();

        if (Input.GetMouseButtonDown(0))
        {
            if (inventory.MouseSlot != 0)
                return;
            Ray ray = new Ray(MainCube.transform.position, Camera.transform.forward);
            RaycastHit hitpick;
            if(Physics.Raycast(ray, out hitpick, 5))
            {
                if (hitpick.collider.CompareTag("Pickable"))
                {
                    WorldItem worldItem = hitpick.collider.GetComponent<WorldItem>();
                    if(worldItem == null)
                        return;
                    int id = worldItem.itemId;
                    inventory.MouseSlot = id;
                    
                    Destroy(hitpick.collider.gameObject);
                    handItemRenderer.Refresh();
                }
            }
        }
    }
}