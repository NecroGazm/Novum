using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager_V2 : MonoBehaviour
{
    public GameObject winObject; // The you win banner
    public GameObject loseObject; // The you lose banner
    public GameObject Pausemenu;
    private SpawnManager_V2 currentSpawner;
    private bool gameFinished;
    //
    public bool isKillAllEnemiesLevel;
    public bool isSurviveUntilTimerLevel;
    public bool isBossFight;
    //
    //
    public Text livesText;

    [HideInInspector]
    public int lives;
    public int defaultlives;
    //
    public static int scoreValue;
    public Text scoreText;
    public Text modifierText;
    public int bonusModifier = 1;
    public float scoreModifyer = 1; // The bonus modifyer for consistant scoring
    public int bonus = 1000;
    public Text EnemyCount;
   
    private int lifeBonusCheckInt;
    //
    private int modifierResetInt = 2;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("Lives") != lives && PlayerPrefs.GetInt("Lives") != 0)
        {
            lives = PlayerPrefs.GetInt("Lives");
        }
        else
        {
            if (defaultlives <= 0)
            {
                defaultlives = 3;
            }

            lives = defaultlives;
        }
    }


    void Start()
    {
        if (FindObjectOfType<SpawnManager_V2>())
        {
            currentSpawner = FindObjectOfType<SpawnManager_V2>();
        }

        if (FindObjectOfType<Canvas_Assignment>())
        {
            livesText = FindObjectOfType<Canvas_Assignment>().livesText;
            scoreText = FindObjectOfType<Canvas_Assignment>().scoreText;

            livesText.text = "x" + lives;
            scoreText.text = PlayerPrefs.GetInt("ScoreValue", scoreValue).ToString();
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale > 0)
        {
            if (isKillAllEnemiesLevel == true)
            {
                if (currentSpawner.expendedWaves == true && currentSpawner.enemiesAliveInScene.Count <= 0 && gameFinished == false)
                {
                    StartCoroutine("WinGameNormally", 0);
                    gameFinished = true;
                }
            }

            if (isSurviveUntilTimerLevel == true)
            {

            }

            if (isBossFight == true)
            {
                if (FindObjectOfType<BossHealth>().HP <= 0 && gameFinished == false)
                {
                    StartCoroutine("WinGameNormally", 0);
                    gameFinished = true;
                }
            }

            if (lives <= 0 && gameFinished == false)
            {
                StartCoroutine("LoseGameNormally", 0);
                gameFinished = true;
            }


            SaveLives();
            UpdateScore();
        }
        if (EnemyCount != null)
        {
            if (currentSpawner != null)
            {
                EnemyCount.text = currentSpawner.enemiesAliveInScene.Count.ToString();
                if(currentSpawner.enemiesAliveInScene.Count == 0)
                {
                    EnemyCount.text = "No Enemies"; 
                }
            }
        }
        
    }

    public IEnumerator WinGameNormally(float time)
    {
        Projectile_V2[] bulletsOnScreen = FindObjectsOfType<Projectile_V2>();
        foreach (Projectile_V2 bullet in bulletsOnScreen)
        {
            Destroy(bullet.gameObject);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Pausemenu.SetActive(false);
        Instantiate(winObject, gameObject.transform.position, Quaternion.identity, null);
        SaveLives();
        yield return new WaitForSeconds(4);
       
        Time.timeScale = 1f;
        if (SceneManager.GetActiveScene().name == "TimerLevel" || SceneManager.GetActiveScene().name == "HookShotLevel" || SceneManager.GetActiveScene().name == "EnduranceLevel" || SceneManager.GetActiveScene().name == "BossM4")
        {
            SceneManager.LoadScene("Highscore");
        }
        else if (SceneManager.sceneCount < SceneManager.GetActiveScene().buildIndex + 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public IEnumerator LoseGameNormally(float time)
    {
        Projectile_V2[] bulletsOnScreen = FindObjectsOfType<Projectile_V2>();
        foreach (Projectile_V2 bullet in bulletsOnScreen)
        {
            Destroy(bullet.gameObject);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Pausemenu.SetActive(false);
        Instantiate(loseObject, gameObject.transform.position, Quaternion.identity, null);
        Time.timeScale = 0;
        yield return new WaitForSeconds(2);
        lives = defaultlives;
    }

    public void AddScore(int scoreToAdd)
    {
        lifeBonusCheckInt += scoreToAdd;
        scoreModifyer *= 1.1f;

        if (scoreModifyer > modifierResetInt)
        {
            SoundManager sm = FindObjectOfType<SoundManager>().GetComponent<SoundManager>();
            sm.soundEffectsAudio[0].PlayOneShot(sm.soundEffectsAudio[0].gameObject.GetComponent<AudioSource>().clip);
            modifierResetInt++;
        }
        PlayerPrefs.SetInt("ScoreValue", scoreValue += Mathf.RoundToInt(scoreToAdd * scoreModifyer));
        UpdateScore();
    }

    public void ResetScoreMultipler()
    {
        scoreModifyer = 1;
        modifierResetInt = 2;
        if (modifierText != null)
        {
            modifierText.text = "x " + scoreModifyer.ToString();
        }
    }

    public void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = scoreValue.ToString();
        }
        if (modifierText != null)
        {
            modifierText.text = "x " + scoreModifyer.ToString("0.00");
        }

       
        if (lifeBonusCheckInt == 1000)
        {
            bonusModifier++;
            bonus = bonus * bonusModifier;
            GiveLife();

            PlayerPrefs.SetInt("BonusAmount", bonus);
            PlayerPrefs.SetInt("BonusMod", bonusModifier);
            lifeBonusCheckInt -= 1000;
        }

       
        scoreText.text = PlayerPrefs.GetInt("ScoreValue", scoreValue).ToString();
    }

    public void SaveLives()
    {
        PlayerPrefs.SetInt("Lives", lives);
    }

    public void DealDeath()
    {
        lives--;
        if (livesText != null)
        {
            livesText.text = "x" + lives;
        }
    }

    public void GiveLife()
    {
        if (GameObject.FindObjectOfType<LevelManager>())
        {
            GameObject levelManager = GameObject.FindObjectOfType<LevelManager>().gameObject;
            levelManager.GetComponent<LevelManager>().audiosource.PlayOneShot(levelManager.GetComponent<LevelManager>().fxs[2]);
        }

        lives++;
        if (livesText != null)
        {
            livesText.text = "x" + lives;
        }
    }
}
