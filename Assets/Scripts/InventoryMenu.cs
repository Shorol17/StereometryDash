using UnityEngine;

public class InventoryMenuController : MonoBehaviour
{
    public GameObject menuRoot;
    public bool InventoryOpened;

    private CursorLockMode cursorLockBeforePause;
    private bool cursorVisibleBeforePause;

    private void Awake()
    {
        menuRoot.SetActive(false);
        InventoryOpened = false;
    }

    private void Update()
    {
        Debug.Log("sadasdsa");
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (InventoryOpened)
                CloseMenu();
            else
                OpenMenu();
        }
    }
    public void OpenMenu()
    {
        InventoryOpened = true;
        menuRoot.SetActive(true);

        cursorLockBeforePause = Cursor.lockState;
        cursorVisibleBeforePause = Cursor.visible;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseMenu()
    {
        InventoryOpened = false;
        Time.timeScale = 1f;
        menuRoot.SetActive(false);

        Cursor.lockState = cursorLockBeforePause;
        Cursor.visible = cursorVisibleBeforePause;
    }
}
