using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Debug_Menu : MonoBehaviour

{
    public bool GameIsDebug = false;
    public GameObject DebugMenu;
    public Player player;
    public Respawn respawn;
    public bool isImmortal;
    public Collider playr;
    public LevelManager lm;





    void Start()
    {
      
        DebugMenu.SetActive(false);
        GameIsDebug = false;
        //isRespawn = FindObjectOfType<Respawn>();
        isImmortal = false;
        //player = FindObjectOfType<Player>();
        //playr = GameObject.Find("Player").GetComponent<MeshCollider>();
        lm = GameObject.FindObjectOfType<LevelManager>();

    }

    public void Update()
    {

        if(player == null)
        {
            player = FindObjectOfType<Player>();
            playr = GameObject.Find("Player").GetComponent<Collider>();
            respawn = FindObjectOfType<Respawn>();
            return;
        }



        // Call the backquote key to open menu

        if(Input.GetKeyDown(KeyCode.BackQuote) && GameIsDebug)
        {
            DebugMenu.SetActive(false);
            GameIsDebug = false;
        }

        else if (Input.GetKeyDown(KeyCode.BackQuote))
        {
           
            DebugMenu.SetActive(true);
            GameIsDebug = true;
        }


        // This is immortality

        if (Input.GetKeyDown(KeyCode.Alpha0) && GameIsDebug && isImmortal)
        {
            Debug.Log("immortality off");
            isImmortal = false;
            playr.enabled = true;
        }

        else
        if (Input.GetKeyDown(KeyCode.Alpha0) && GameIsDebug)
        {
            Debug.Log("immortality on");
            isImmortal = true;
            playr.enabled = false;
        }





        // This is skipping scenes
        if (Input.GetKeyDown(KeyCode.Alpha1) && GameIsDebug)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            if (Input.GetKeyDown(KeyCode.Alpha1) && GameIsDebug && SceneManager.GetActiveScene().name == "Highscore")
            {
                PlayerPrefs.SetInt("ScoreValue",0);
                SceneManager.LoadScene(0);
            }
        }






        // This is resetting the current stage
        if (Input.GetKeyDown(KeyCode.Alpha2) && GameIsDebug)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }





        // Kill Player
        if (Input.GetKeyDown(KeyCode.Alpha3) && GameIsDebug)
        {
            
            StartCoroutine(respawn.Die(1));
        }






        // Kill Enemies
        if (Input.GetKeyDown(KeyCode.Alpha4) && GameIsDebug)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
                Destroy(enemy);

            GameObject[] enemies1 = GameObject.FindGameObjectsWithTag("Enemy1");
            foreach (GameObject enemy1 in enemies1)
                Destroy(enemy1);

            lm.audiosource.PlayOneShot(lm.fxs[1]);


        }
    }

}