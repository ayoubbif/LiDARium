using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private bool _isGamePaused;
    public bool IsGamePaused => _isGamePaused;
    public void TogglePause()
    {
        _isGamePaused = !_isGamePaused;
        Time.timeScale = _isGamePaused ? 0f : 1f;
    }
}
