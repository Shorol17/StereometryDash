using UnityEditorInternal;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

public class ObjectSpawnerMenu : MonoBehaviour
{
    public ObjectDatabase database;
    public TMP_Dropdown dropdown;
    public GameObject menuRoot;
    public int SelectedIndex;
    public bool MenuOpened;

    void Start()
    {
        menuRoot.SetActive(false);
        dropdown.ClearOptions();

        List<string> options = new();
        for (int obj=0; obj < database.objects.Length; obj++)
            options.Add(database.GetObject(obj));
        dropdown.AddOptions(options);
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
