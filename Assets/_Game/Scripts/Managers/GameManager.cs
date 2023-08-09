using _Game.Scripts.Utils;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject pauseMenuUI;
    private bool _isPaused;

    private void Start()
    {
        CursorUtils.LockCursor();
    }

    public void TogglePauseMenu()
    {
        _isPaused = !_isPaused;

        pauseMenuUI.SetActive(_isPaused);

        if (_isPaused)
        {
            CursorUtils.UnlockCursor();
        }
        else
        {
            CursorUtils.LockCursor();
        }

        Time.timeScale = _isPaused ? 0f : 1f;
    }

    public void QuitToMainMenu()
    {
        // Go back to the Main Menu scene, assuming it's named "Menu"
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");

        // Resume the time scale in case we quit while paused
        Time.timeScale = 1f;
    }
}