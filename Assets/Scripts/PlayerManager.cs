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

    public CutSceneManager cutSceneManager;

    public int difficultySet = 0;

    public HumanSpawner humanSpawner;

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

        Debug.Log(score);
        if (score >= 20 && difficultySet == 0)
        {
            humanSpawner.setDifficulty(1);
            difficultySet = 1;
            if (cutSceneManager != null)
                cutSceneManager.PlayCutScene(0);
        } 
        else if (score >= 50 && difficultySet == 1)
        {
            humanSpawner.setDifficulty(2);
            difficultySet = 2;
            if (cutSceneManager != null)
                cutSceneManager.PlayCutScene(1);
        }
        else if (score >= 80 && difficultySet == 2)
        {
            humanSpawner.setDifficulty(3);
            difficultySet = 3;
            if (cutSceneManager != null)
                cutSceneManager.PlayCutScene(2);
        }
        else if (score >= 110 && difficultySet == 3)
        {
            humanSpawner.setDifficulty(4);
            difficultySet = 4;
            if (cutSceneManager != null)
                cutSceneManager.PlayCutScene(3);
        }
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        Debug.Log("GameOver");
        showMenu(true);
        AudioManager.instance.PlaySound(0);
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
