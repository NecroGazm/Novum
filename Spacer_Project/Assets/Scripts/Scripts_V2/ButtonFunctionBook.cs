using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctionBook : MonoBehaviour
{
    public void NextLevel(AudioClip clipToPlay)
    {
        StartCoroutine("NextLevelAct", clipToPlay);
    }

    private IEnumerator NextLevelAct(AudioClip clipToPlay)
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(clipToPlay);
        yield return new WaitForSeconds(.50f);
        if (SceneManager.GetActiveScene().name == "TimerLevel" || SceneManager.GetActiveScene().name == "HookShotLevel" || SceneManager.GetActiveScene().name == "EnduranceLevel" || SceneManager.GetActiveScene().name == "BossM4")
        {
            SceneManager.LoadScene("Highscore");
        }
        else if (SceneManager.sceneCount < SceneManager.GetActiveScene().buildIndex + 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            Debug.Log("No next scene in build settings, returning player to main menu...");
            PlayerPrefs.SetInt("ScoreValue", 0);
            SceneManager.LoadScene(0);
            Time.timeScale = 1.0f;
        }
    }

    public void ReturnToMainMenu(AudioClip clipToPlay)
    {
        PlayerPrefs.SetInt("Lives", 3);
        PlayerPrefs.SetInt("ScoreValue", 0);
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }

    public void RetryLevel(AudioClip clipToPlay)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void PlayAudio(AudioClip clipToPlay)
    {
        if (gameObject.GetComponent<AudioSource>())
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(clipToPlay);
        }
    }

    public void HighScoreScene()
    {
        SceneManager.LoadScene("Highscore");
        Time.timeScale = 1.0f;
    }

    public void ClearScores()
    {
        HSTable hstable = FindObjectOfType<HSTable>();

        hstable.ClearHighScoreList();
        SaveSystem.ClearSaveData();
        hstable.SortHighScores();
        hstable.RefreshScores();
    }
}
