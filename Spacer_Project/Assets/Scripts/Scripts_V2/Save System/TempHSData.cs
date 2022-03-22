using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempHSData : MonoBehaviour
{
    public Dictionary<string, int> highscores = new Dictionary<string, int>();

    public void Save()
    {
        SaveSystem.SaveHighScore(this);
    }
}
