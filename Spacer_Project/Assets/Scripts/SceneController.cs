using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadScene()
    {
        PlayerPrefs.SetInt("Lives", 3);
        PlayerPrefs.SetInt("ScoreValue", 0);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void LoadSpecificScene(string sceneToLoad)
    {
        PlayerPrefs.SetInt("Lives", 3);
        PlayerPrefs.SetInt("ScoreValue", 0);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1.0f;
    }

    public void NextScene()
    {
        PlayerPrefs.SetInt("Lives", 3);
        PlayerPrefs.SetInt("ScoreValue", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1.0f; 
    }

    public void Highscore()
    {
        SceneManager.LoadScene("Highscore");
        Time.timeScale = 1.0f;

    }

    public void BOSSMODE()
    {
        PlayerPrefs.SetInt("Lives", 3);
        PlayerPrefs.SetInt("ScoreValue", 0);
        SceneManager.LoadScene("BossM1");
        Time.timeScale = 1.0f;

    }

    public void TimeMode()
    {
        PlayerPrefs.SetInt("Lives", 3);
        PlayerPrefs.SetInt("ScoreValue", 0);
        SceneManager.LoadScene("TimerLevel");
        Time.timeScale = 1.0f;

    }

    public void Tutorial()
    {
        PlayerPrefs.SetInt("Lives", 3);
        PlayerPrefs.SetInt("ScoreValue", 0);
        SceneManager.LoadScene("Tutorial1");
        Time.timeScale = 1.0f;

    }

    public void EnduranceMode()
    {
        PlayerPrefs.SetInt("Lives", 3);
        PlayerPrefs.SetInt("ScoreValue", 0);
        SceneManager.LoadScene("EnduranceLevel");
        Time.timeScale = 1.0f;

    }
    public void Gallery()
    {
        PlayerPrefs.SetInt("Lives", 3);
        PlayerPrefs.SetInt("ScoreValue", 0);
        SceneManager.LoadScene("Gallery");
        Time.timeScale = 1.0f;

    }
}
