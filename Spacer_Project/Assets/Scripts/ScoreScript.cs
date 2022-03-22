using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public static int scoreValue = 000000;
    Text textScore;
    
    public int bonusModifier = 1;
    public int bonus = 1000;
    public LifeController lives;
    public GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        textScore = GetComponent<Text>();
        lives = FindObjectOfType<LifeController>();
        

        if (PlayerPrefs.HasKey("BonusAmount"))
        {
            if (PlayerPrefs.HasKey("BonusMod"))
            {
                bonusModifier = PlayerPrefs.GetInt("BonusMod");
                bonus = PlayerPrefs.GetInt("BonusAmount");
            }
        }


        if (lives == null)
        {
            return;
        }
    }


    void Update()
    {
        textScore.text = scoreValue.ToString();
        PlayerPrefs.SetInt("ScoreValue", scoreValue);
        if (scoreValue > PlayerPrefs.GetInt("Highscore", 0))
        {
            PlayerPrefs.SetInt("Highscore", scoreValue);
            
        }

        if (scoreValue == bonus)
        {
            bonusModifier++;
            bonus = bonus * bonusModifier;
            lives.GiveLife();

            PlayerPrefs.SetInt("BonusAmount", bonus);
            PlayerPrefs.SetInt("BonusMod", bonusModifier);
        }
    }
}
