using JetBrains.Annotations;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public GameObject MainCube;
    public float distance = 8f;
    public float heigh = 3f;
    public float rotatespeed = 300f;
    public bool FPW;
    public bool InventoryOpened;
    public GameObject[] FPWs;

    private float anglex;
    private float angley;

    void SwitchRenderer(GameObject[] FPWs, bool FPW)
    {
        foreach (GameObject obj in FPWs)
        {
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            renderer.enabled = !FPW;
        }
    }

    void Update()
    {
        if (target == null) return;
        if (FPW)
        {
            if (!InventoryOpened){
            anglex += Input.GetAxis("Mouse X") * rotatespeed * Time.deltaTime;
            angley += Input.GetAxis("Mouse Y") * rotatespeed * Time.deltaTime;
            }
            Quaternion Camera = Quaternion.Euler(-angley, anglex, 0f);
            Quaternion Cube = Quaternion.Euler(0f, anglex, 0f);
            transform.rotation = Camera;
            target.rotation = Cube;

            transform.position = target.position;
            
            SwitchRenderer(FPWs, FPW);
        }
        else
        {
            if (Input.GetKey(KeyCode.E))
            {
                MainCube.transform.Rotate(Camera.main.transform.up, Input.GetAxis("Mouse X")*10, Space.World);
                MainCube.transform.Rotate(Camera.main.transform.right, Input.GetAxis("Mouse Y")*-10, Space.World);
            }
            else
            {
            if (!InventoryOpened){
            anglex += Input.GetAxis("Mouse X") * rotatespeed * Time.deltaTime;
            angley += Input.GetAxis("Mouse Y") * rotatespeed * Time.deltaTime;
            }
                Vector3 offset = Quaternion.Euler(angley, anglex, 0f) * new Vector3(0f, heigh, distance);
                transform.position = target.position + offset;
                transform.LookAt(target.position);
            }
            SwitchRenderer(FPWs, FPW);
        }

        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        distance -= 8 * scroll;
        heigh -= 3 * scroll;

        if (distance < 0 && heigh < 0)
        {
            distance = 0;
            heigh = 0;
            FPW = true;
        }

        if (distance > 0 || heigh > 0 && Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            FPW = false;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryOpened = !InventoryOpened;
        }
    }
}