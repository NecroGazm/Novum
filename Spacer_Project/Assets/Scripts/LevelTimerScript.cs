using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimerScript : MonoBehaviour
{
    [Tooltip("The display text of the timer.")]
    public Text timerText;

    [Tooltip("How much time the player has to complete the level.")]
    public float timeToCompleteLevel;
    private float originalTime;

    [HideInInspector]
    public bool gameStarted;

    public GameObject winObject;

    private void Awake()
    {
        originalTime = timeToCompleteLevel;

        if (timerText != null)
        {
            timerText.text = "Time to Survive: " + timeToCompleteLevel;
        }
    }

    public void AddTime(int timeToAdd) // If we want an add time pickup
    {
        if ((timeToCompleteLevel + timeToAdd) > originalTime)
        {
            timeToCompleteLevel = originalTime;
        }
        else
        {
            timeToCompleteLevel += timeToAdd;
        }
    }

    private void Update()
    {
        if (gameStarted == true)
        {
            if (Time.timeScale != 0)
            {
                timeToCompleteLevel -= Time.deltaTime;
            }

            RefreshTimer();

            if (timeToCompleteLevel <= 0)
            {
                //WIN
                var enemies = FindObjectsOfType<EnemyBehavior_V2>();

                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].KillSelf();
                }

                winObject.gameObject.SetActive(true);
            }
        }
    }

    public void RefreshTimer()
    {
        if (timerText != null)
        {
            if (timeToCompleteLevel > 60)
            {
                int minsLeft = Mathf.RoundToInt(timeToCompleteLevel / 60);
                int secondsLeftover = Mathf.RoundToInt(timeToCompleteLevel - (minsLeft * 60));
                if (secondsLeftover > 9)
                {
                    timerText.text = minsLeft + ":" + secondsLeftover;
                }
                else
                {
                    timerText.text = minsLeft + ":0" + secondsLeftover;
                }
            }
            else if (timeToCompleteLevel < 60 && timeToCompleteLevel > 0)
            {
                timerText.text = timeToCompleteLevel.ToString("0:0#");
            }
            else
            {
                timerText.text = "0:00";
            }
        }
    }
}
