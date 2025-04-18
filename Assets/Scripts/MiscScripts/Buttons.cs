using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField]private Canvas menu;
    [SerializeField]private Canvas controls;
    [SerializeField]private Canvas score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (FindFirstObjectByType<GameManager>() != null)
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
        gameManager.EndGame();
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
        gameManager.EndGame();
        gameManager.StartGame();
    }
    public void Quitgame()
    {
        Application.Quit();
    }

    public void ContolScreen()
    {
        controls.enabled = true;
        menu.enabled = false;
    }

    public void MenuScreen()
    {
        GetComponentInParent<Canvas>().enabled = false;
        menu.enabled = true;
    }

    public void ScoreBoard()
    {
        score.enabled = true;
        menu.enabled = false;
    }
}
