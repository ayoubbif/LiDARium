using _Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject optionsMenuUI;

    private void Start()
    {
        // Check if GameObjects are assigned.
        if (mainMenuUI == null || optionsMenuUI == null)
        {
            Debug.LogError("One or more of the required GameObjects is not assigned in the editor.");
            enabled = false;
            CursorUtils.LockCursor();
        }
    }

    public void StartGame()
    {
        // Check if the "Game" scene exists.
        if (Application.CanStreamedLevelBeLoaded("Game"))
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.LogError("The specified scene does not exist.");
        }
    }

    public void ShowOptionsMenu()
    {
        mainMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void ShowMainMenu()
    {
        optionsMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);

        CursorUtils.UnlockCursor();
    }
    
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit(); 
#endif
    }
}