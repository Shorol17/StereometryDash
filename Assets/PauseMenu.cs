using System.Runtime.CompilerServices;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class PauseMenuController : MonoBehaviour
{
    public static bool IsPaused;
    public GameObject menuRoot;

    private CursorLockMode cursorLockBeforePause;
    private bool cursorVisibleBeforePause;

    private void Awake()
    {
        HideMenu();
        IsPaused = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
                Resume();
            else
                Pause();
        }
    }
    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        ShowMenu();

        cursorLockBeforePause = Cursor.lockState;
        cursorVisibleBeforePause = Cursor.visible;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        HideMenu();

        Cursor.lockState = cursorLockBeforePause;
        Cursor.visible = cursorVisibleBeforePause;
    }

    public void Restart()
    {
        Resume();
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
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