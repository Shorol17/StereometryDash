using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework.Internal.Execution;
using Unity.VisualScripting;
using UnityEngine;

public class CubeController2 : MonoBehaviour
{
    public float movespeed = 7f;
    public float jumpforce = 6f;
    public Transform cameraTransform;
    public CameraController cameraController;
    public bool NoClip = false;
    public GameObject MainCube;
    public float Raylenght;
    public HandItemRenderer handItemRenderer;
    public Inventory inventory;
    public ItemManager itemManager;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
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

        Vector3 canForward = Vector3.forward;
        Vector3 canRight = Vector3.right;
        if (cameraTransform != null)
        {
            canForward = cameraTransform.forward;
            canRight = cameraTransform.right;
        }
        canForward.y = 0f;
        canRight.y = 0f;
        canForward.Normalize();
        canRight.Normalize();

        Vector3 move = (canRight * x + canForward * z).normalized;
        if (!Input.GetKey(KeyCode.E))
            transform.Translate(translation: move * movespeed * Time.deltaTime, relativeTo: Space.World);

        if (NoClip)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.linearVelocity = Vector3.zero;
            if (Input.GetKey(KeyCode.Space))
                {
                    transform.position += new Vector3(0, movespeed*Time.deltaTime, 0);
                }
            if (Input.GetKey(KeyCode.LeftControl))
                {
                    transform.position += new Vector3(0, -movespeed*Time.deltaTime, 0);
                }
            }
        else 
        {
            if (cameraController.FPW) Raylenght = 1f;
            else Raylenght = 0.7f;
            rb.constraints = RigidbodyConstraints.None;
            if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Raylenght))
            {
                Vector3 normal = new(hit.normal.x, hit.normal.y, hit.normal.z);
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, rb.linearVelocity.z) + normal * jumpforce;
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            NoClip = !NoClip;
            SwitchNoClip(MainCube, NoClip);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            movespeed = 14;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movespeed = 7;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            handItemRenderer.DropItem();
        }

        if (Input.GetMouseButtonDown(1))
        {
            itemManager.UseSingular();
        }

        if (Input.GetMouseButtonUp(1))
        {
            itemManager.UseContinues();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (inventory.MouseSlot != 0)
                return;
            Ray ray = new Ray(MainCube.transform.position, cameraTransform.transform.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 5))
            {
                if (hit.collider.CompareTag("Pickable"))
                {
                    WorldItem worldItem = hit.collider.GetComponent<WorldItem>();
                    if(worldItem == null)
                        return;
                    int id = worldItem.itemId;
                    inventory.MouseSlot = id;
                    
                    Destroy(hit.collider.gameObject);
                    handItemRenderer.Refresh();
                }
            }
        }
    }
}