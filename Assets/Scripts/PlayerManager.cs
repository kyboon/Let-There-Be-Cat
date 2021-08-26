using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;

    public Text scoreText;

    public bool canPause = true;
    public GameObject mainMenu;
    public Text menuTitle;
    public Text mainMenuScore;
    public GameObject resumeButton; 

    private int score = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        score = 0;
        if (scoreText != null)
            scoreText.text = "0";
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canPause && Input.GetButtonDown("Pause"))
        {
            PauseGame();
        }
    }
    public void addScore(int scoreToAdd)
    {
        score += scoreToAdd;
        if (scoreText != null)
            scoreText.text = score.ToString();
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        Debug.Log("GameOver");
        showMenu(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        showMenu(false);
    }
    public void Resume()
    {
        Time.timeScale = 1;
        hideMenu();
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void showMenu(bool isGameOver)
    {
        scoreText.enabled = false;
        if (mainMenu != null)
        {
            mainMenu.SetActive(true);

            if (isGameOver)
            {
                menuTitle.text = "You've been caught!\nYour Score:";
                resumeButton.SetActive(false);
                mainMenuScore.enabled = true;
                mainMenuScore.text = score.ToString();
            } else
            {
                menuTitle.text = "Paused";
                resumeButton.SetActive(true);
                mainMenuScore.enabled = false;
            }
        }
    }
    public void hideMenu()
    {
        scoreText.enabled = true;
        if (mainMenu != null)
        {
            mainMenu.SetActive(false);
        }
    }
}
