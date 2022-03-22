using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public bool NewGame = true;


    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "Title_SpacerProject")
        {
            PlayerPrefs.SetInt("Lives", 0);
            PlayerPrefs.SetInt("ScoreValue", 0);
        }
    }
}
