using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class NameSaver : MonoBehaviour
{
    public string PlayerName;
    public GameObject InputField;
    public GameObject textDisplay;


    public void submitName()
    {
        PlayerName = InputField.GetComponent<Text>().text;
        if (FindObjectOfType<TempHSData>())
        {
            HSTable table = FindObjectOfType<HSTable>();
            table.NewScore(PlayerName, GameManager_V2.scoreValue);

            TempHSData temp = FindObjectOfType<TempHSData>();
            if (temp.highscores == null)
            {
                temp.highscores = new Dictionary<string, int>();
            }
            temp.highscores.Add(PlayerName, GameManager_V2.scoreValue);


            temp.Save();
        }
        else
        {
            Debug.LogWarning("Forgot to add TempHS script to an object!");
        }
    }
    
}
