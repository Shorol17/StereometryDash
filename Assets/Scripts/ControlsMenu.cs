using UnityEngine;

public class HelpMenu : MonoBehaviour
{
    public bool MenuOpened;
    public GameObject menuRoot;
    public PauseMenuController PauseMenu;

    private void Awake()
    {
        MenuOpened = false;
        Close();
    }

    public void Back()
    {
        Close();
        PauseMenu.ShowMenu();
    }

    public void Open()
    {
        MenuOpened = true;
        menuRoot.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Close()
    {
        MenuOpened = false;
        menuRoot.SetActive(false);
    }
}
