using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    private GameObject pauseMenu;
    private GameObject gameOverMenu;
    private GameObject gameWinMenu;
    private RectTransform pauseMenuRectTransform;
    private RectTransform gameOverMenuRectTransform;
    GameManagerScript gameManagerScript;
    PlayerScript playerScript;


    void Start()
    {
        gameManagerScript = GetComponent<GameManagerScript>();
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();

        pauseMenu = GameObject.FindWithTag("Canvas").transform.Find("PauseMenu").gameObject;
        gameOverMenu = GameObject.FindWithTag("Canvas").transform.Find("GameOverMenu").gameObject;
        gameWinMenu = GameObject.FindWithTag("Canvas").transform.Find("GameWinMenu").gameObject;

        pauseMenuRectTransform = pauseMenu.GetComponent<RectTransform>();
        gameOverMenuRectTransform = gameOverMenu.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (gameManagerScript.gameOver)
        {
            gameOverMenu.SetActive(true);
            gameManagerScript.gameOver = false;
        }

        if (playerScript.win)
        {
            gameWinMenu.SetActive(true);
        }
    }

    public void OnClickPause()
    {
        pauseMenu.SetActive(true);
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene("CampaignMenu");
        ResumeGame();
        if (gameManagerScript.currentHealth <= 0)
        {
            LeanTween.scale(gameOverMenuRectTransform, gameOverMenuRectTransform.localScale/5f, 0.15f).setOnComplete(Restart);
        }
        else
        {
            LeanTween.scale(pauseMenuRectTransform, pauseMenuRectTransform.localScale/5f, 0.15f).setOnComplete(Restart);
        }
    }
    void Restart()
    {
        gameManagerScript.FadeIn();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        gameManagerScript.isPaused = true;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        gameManagerScript.isPaused = false;
    }
}
