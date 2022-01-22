using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance
    {
        get;
        private set;
    }

    [Header("UI Panels")]
    public GameObject GUI;
    public GameObject MainMenu;
    public GameObject PauseMenu;

    [Header("Updated Items")]
    public Text ScoreText;
    public Text HealthText;
    public Text MaxScoreText;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this);

        gameManager = GameManager.Instance;

        if (gameManager)
        {
            gameManager.ScoreUpdated += UpdateScore;
            gameManager.MaxScoreUpdated += UpdateMaxScore;
            gameManager.HealthPointsUpdated += UpdateHealthPoints;
        }
    }
    
    void UpdateScore(int _value)
    {
        if(ScoreText)
            ScoreText.text = _value.ToString();
    }

    void UpdateMaxScore(int _value)
    {
        if(MaxScoreText)
            MaxScoreText.text = _value.ToString();
    }

    void UpdateHealthPoints(int _value)
    {
        if(HealthText)
            HealthText.text = _value.ToString();
    }

    public void Play()
    {
        if(GUI && gameManager)
        {
            if (MainMenu)
                MainMenu.SetActive(false);

            GUI.SetActive(true);
            gameManager.StartGame();
        }
    }

    public void Pause()
    {
        if(PauseMenu && gameManager)
        {
            if(GUI)
                GUI.SetActive(false);

            PauseMenu.SetActive(true);
            gameManager.PauseGame(true);
        }
    }

    public void Continue()
    {
        if(gameManager)
            gameManager.PauseGame(false);

        if(GUI && gameManager)
        {
            if(PauseMenu)
                PauseMenu.SetActive(false);

            GUI.SetActive(true);
            gameManager.PauseGame(false);
        }
    }

    public void Restart()
    {
        if (GUI && gameManager)
        {
            if (PauseMenu)
                PauseMenu.SetActive(false);

            GUI.SetActive(true);
            gameManager.RestartGame();
        }
    }

    public void BackToMainMenu()
    {
        if(MainMenu && gameManager)
        {
            if(GUI)
                GUI.SetActive(false);
            if(PauseMenu)
                PauseMenu.SetActive(false);

            MainMenu.SetActive(true);
            gameManager.StopGame();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
