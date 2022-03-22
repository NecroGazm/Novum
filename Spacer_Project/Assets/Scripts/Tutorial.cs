using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    
    public GameObject PauseUI;

    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        PauseUI.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Begin()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseUI.SetActive(true);
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    private void Update() // God I hate this
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
