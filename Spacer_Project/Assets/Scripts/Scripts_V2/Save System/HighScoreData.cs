using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighScoreData
{
    public Dictionary<string, int> highscores;

    public HighScoreData(TempHSData data)
    {
        highscores = data.highscores;
    }
}
