using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public bool IsPaused;
    public GameObject menuRoot;
    public HelpMenu helpMenu;

    private CursorLockMode cursorLockBeforePause;
    private bool cursorVisibleBeforePause;

    private void Awake()
    {
        HideMenu();
        IsPaused = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                Resume();
                if (helpMenu.MenuOpened)
                    helpMenu.Close();
            }
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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Restart()
    {
        Resume();
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void Help()
    {
        HideMenu();
        helpMenu.Open();
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void ShowMenu()
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