using UnityEditorInternal;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

public class ItemSpawnerMenu : MonoBehaviour
{
    public ItemDatabase database;
    public TMP_Dropdown dropdown;
    public GameObject menuRoot;
    public int SelectedItemId{get; private set;}
    public bool MenuOpened;

    void Start()
    {
        menuRoot.SetActive(false);
        dropdown.ClearOptions();

        List<string> options = new();
        foreach (ItemData item in database.items)
            options.Add(item.ItemName);
        dropdown.AddOptions(options);

        dropdown.onValueChanged.AddListener(OnChanged);
            OnChanged(0);
    }

    void OnChanged(int index)
    {
        SelectedItemId = database.items[index].id;
    }

    public void SwitchMenu()
    {
        menuRoot.SetActive(!MenuOpened);
        MenuOpened = !MenuOpened;
        if (MenuOpened)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
