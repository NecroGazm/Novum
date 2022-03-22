using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PowerUpTimer : MonoBehaviour
{

    [SerializeField] float startTime = 9999999f;
    [SerializeField] Slider slider1;
    [SerializeField] Text timerText1;


    public Image BulletSpeed;
    public Image PlayerSpeed;
    public Image NoDamage;

    float timer1 = 0f;

    
    void Awake()
    {
        StartCoroutine(Timer1());
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
           
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