using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject optionsMenuUI;

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    
        // // Hide and lock the cursor
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        
        startMenu.SetActive(false);
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

        // Show and unlock the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    
    public void HideMainMenu()
    {
        optionsMenuUI.SetActive(false);
        mainMenuUI.SetActive(false);
        
        // Hide and lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        // Quit the application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Quit in the Unity Editor
#else
        Application.Quit(); // Quit in a standalone build
#endif
    }
}
