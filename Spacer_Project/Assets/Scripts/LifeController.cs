using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LifeController : MonoBehaviour
{
    public int defaultlives;
    public int lives;
    public bool IsOver = false;
    public GameObject GameOverUI; 
    public Text TextLives;
    public GameObject Player;
    public GameObject ReadyText;
    public GameObject GoText;
    public GameManager gameManager;
    public LevelManager levelManager; 


    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        levelManager = GameObject.FindObjectOfType<LevelManager>();

        if (PlayerPrefs.HasKey("Lives") == true)
        {
            if (gameManager != null)
            {
                if (!gameManager.NewGame)
                {
                    lives = PlayerPrefs.GetInt("Lives");
                }
                else
                {
                    lives = defaultlives;
                }
            }
        }
       
        TextLives.text = "x" + lives;
       
        Scene scenenum = SceneManager.GetActiveScene();
    }

    
    void Update()
    {
        
        if (lives <= 0)
        {
            ReadyText.SetActive(false);
            GoText.SetActive(false);
            GameOverUI.SetActive(true);

            Time.timeScale = 0f;
            IsOver = true;
        }


    }
    
    public void SaveLives()
    {
        PlayerPrefs.SetInt("Lives", lives);
    }

    public void DealDeath()
    {
        lives--;
        TextLives.text = "x" + lives;
    }

    public void GiveLife()
    {
        levelManager.audiosource.PlayOneShot(levelManager.fxs[2]);
        lives++;
        TextLives.text = "x" + lives;
    }
}


