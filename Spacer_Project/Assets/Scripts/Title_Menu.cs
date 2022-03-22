using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Title_Menu : MonoBehaviour
{
  

    public void NewGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);

    }

    public void Options()
    {

    }

    public void Credits()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
