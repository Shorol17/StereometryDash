using UnityEngine;

public class InventoryMenuController : MonoBehaviour
{
    public GameObject menuRoot;
    public bool InventoryOpened;

    private CursorLockMode cursorLockBeforePause;
    private bool cursorVisibleBeforePause;

    private void Awake()
    {
        HideMenu();
        InventoryOpened = false;
    }

    private void Update()
    {
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
        ShowMenu();

        cursorLockBeforePause = Cursor.lockState;
        cursorVisibleBeforePause = Cursor.visible;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseMenu()
    {
        InventoryOpened = false;
        Time.timeScale = 1f;
        HideMenu();

        Cursor.lockState = cursorLockBeforePause;
        Cursor.visible = cursorVisibleBeforePause;
    }

    private void ShowMenu()
    {
        if (menuRoot != null)
            menuRoot.SetActive(true);
    }

    private void HideMenu()
    {
        if (menuRoot != null)
            menuRoot.SetActive(false);
    }
}
