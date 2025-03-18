using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(FindFirstObjectByType<GameManager>() != null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
    }

    public void PauseButton()
    {
        gameManager.Pause();
    }
    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ToGame()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(1);
    }
    public void RestartGame()
    {
        gameManager.StartGame();
    }
    public void Quitgame()
    {
        Application.Quit();
    }
}
