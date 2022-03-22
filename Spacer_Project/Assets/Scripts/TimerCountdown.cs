using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerCountdown : MonoBehaviour
{
    
    [SerializeField] float startTime = 0;
    [SerializeField] Slider slider1;
    [SerializeField] Text timerText1;
    private SpawnManager_V2 SP;
    float timer1 = 0f;
    private float weaponSwitchCooldown = 10;
    private int weaponToSwitchTo = 0;

    private bool gameEnded;

    void Awake()
    {
        StartCoroutine(Timer1());
    }

    public void Update()
    {
        if (Time.timeScale > 0)
        {
            if (timer1 <= 0 && gameEnded == false)
            {
                var enemies = FindObjectsOfType<EnemyBehavior_V2>();

                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].KillSelf();
                }
               
             FindObjectOfType<GameManager_V2>().StartCoroutine("WinGameNormally", 1);
                gameEnded = true;
               
            }

            weaponSwitchCooldown -= Time.deltaTime;

            if (weaponSwitchCooldown <= 0)
            {
                if (FindObjectOfType<Weapon_Script>())
                {
                    Weapon_Script playerScript = FindObjectOfType<Weapon_Script>();
                    playerScript.RevertWeapon();
                    if (weaponToSwitchTo < 3)
                    {
                        playerScript.SetNewPlayerWeapon(weaponToSwitchTo + 1);
                        weaponToSwitchTo++;
                    }
                    else
                    {
                        playerScript.SetNewPlayerWeapon(0);
                        weaponToSwitchTo = 0;
                    }
                    
                    weaponSwitchCooldown += 10;
                }
                else
                {
                    weaponSwitchCooldown += 10;
                }
            }
        }
    }

    private IEnumerator Timer1()
    {
        timer1 = startTime;

        do
        {
            //Execute
            timer1 -= Time.deltaTime;

            slider1.value = 1 - timer1 / startTime;
            FormatText1();

            yield return null;
        }
        while (timer1 > 0);
    }

    private void FormatText1()
    {
        int years = (int)(timer1 / 31536000) % 999999999;
        int months = (int)(timer1 / 2592000) % 12;
        int days = (int)(timer1 / 86400) % 365;
        int hours = (int)(timer1 / 3600) % 24;
        int minutes = (int)(timer1 / 60) % 60; ;
        int seconds = (int)(timer1 % 60);


        timerText1.text = "";
        if (years > 0) { timerText1.text += years + "Y:"; }
        if (months > 0) { timerText1.text += months + "M:"; }
        if (days > 0) { timerText1.text += days + "D:"; }
        if (minutes > 0) { timerText1.text += minutes + "m:"; }
        if (seconds > 0) { timerText1.text += seconds + "s"; }
    }
}




