using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public PauseMenuController pauseMenuController;
    public InventoryMenuController inventoryMenuController;
    public ObjectSpawnerMenu objectSpawnerMenu;
    public ItemSpawnerMenu itemSpawnerMenu;

    public bool IsMenuOpened()
    {
        return pauseMenuController.IsPaused || inventoryMenuController.InventoryOpened || objectSpawnerMenu.MenuOpened || itemSpawnerMenu.MenuOpened;
    }
}
