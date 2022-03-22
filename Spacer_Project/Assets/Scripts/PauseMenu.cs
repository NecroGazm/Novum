using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //Author:ElizabethCherry
    //Tutorial used:https://www.youtube.com/watch?v=JivuXdrIHK0
    // Start is called before the first frame update

    public GameObject PauseUI;
    public GameObject SettingsUI;
    public GameObject ControlsUI;
    public bool IsPaused = false;

    void Pause()
    {
        PauseUI.SetActive(true);

        Time.timeScale = 0f;
        IsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
    public void Resume()
    {
        PauseUI.SetActive(false);
        SettingsUI.SetActive(false);
        ControlsUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                Resume();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Pause();
            }
        }
    }
}