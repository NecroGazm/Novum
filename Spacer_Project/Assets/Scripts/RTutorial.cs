using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RTutorial : MonoBehaviour
{
    public GameObject TutorialBox;
    public GameObject WTutorialBox;
    public GameObject Empty;
    public GameObject ButtonT;
   
    public GameObject PanelT;
    public GameObject Bullet;
    public GameObject PauseUI;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
          if (other.gameObject.CompareTag("Enemy_Bullet") && Bullet.gameObject.GetComponent<Projectile_V2>().isEnemyProjectile == true && SceneManager.GetActiveScene().name == "Stage0")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            PauseUI.SetActive(false);
            TutorialBox.SetActive(true);
            Empty.SetActive(false);
            PanelT.SetActive(true);
            ButtonT.SetActive(true);
        }
        if (other.gameObject.CompareTag("BBoost")|| other.gameObject.CompareTag("IBoost")||other.gameObject.CompareTag("SpeedBoost")|| other.gameObject.CompareTag("Weapon"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            PauseUI.SetActive(false);
            TutorialBox.SetActive(true);
            Empty.SetActive(false);
            PanelT.SetActive(true);
            ButtonT.SetActive(true);
        }
      
       
    }


}
