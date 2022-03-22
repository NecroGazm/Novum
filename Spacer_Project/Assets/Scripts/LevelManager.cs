using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LevelManager : MonoBehaviour
{
    public GameObject player;
    public GameObject canvas;
    public GameObject StartText;
    public GameObject StageText;
    public AudioSource audiosource;
    public AudioClip[] fxs;
    public GameObject[] explosionfx;
    public GameManager gm;
    public bool isMetrics;
    public GameObject pushToStart;
    public bool xbutton;

    void Awake()
    {
        //player = GameObject.Find("Player");
        canvas = GameObject.Find("Canvas");
        StartText = GameObject.Find("Start");
        StageText = GameObject.Find("Stage0");
        audiosource = this.GetComponent<AudioSource>();
        pushToStart = GameObject.Find("PushToStart");
        StopCoroutine("StageStart");
        if (player == null)
        {
            player = FindObjectOfType<Player>().gameObject;
        }
    }

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        StageText.SetActive(false);
        StartText.SetActive(false);
        player.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
         if ( SceneManager.GetActiveScene().name == "Stage0"|| SceneManager.GetActiveScene().name == "Stage1")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space) && SceneManager.GetActiveScene().name == "Stage0" && xbutton )
        {
            pushToStart.SetActive(false);
            isMetrics = false;
            xbutton = false;
           
            StartCoroutine("StageStart", 1);
        }
       


    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "Title_SpacerProject" || SceneManager.GetActiveScene().name == "Stage0")
        {

        }
        else
        {
            isMetrics = false;
            StartCoroutine("StageStart", 1);
        }
    }

    IEnumerator StageStart(float time)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yield return new WaitForSeconds(1);
        StartText.SetActive(true);
        StartCoroutine(StageOne(1));

    }

    IEnumerator StageOne(float time)
    {
        yield return new WaitForSeconds(1);
        StartText.SetActive(false);
        yield return new WaitForSeconds(1);
        StageText.SetActive(true);
        StartCoroutine(PlayerOn(1));
    }

    IEnumerator PlayerOn(float time)
    {
        yield return new WaitForSeconds(1);
        StageText.SetActive(false);
        yield return new WaitForSeconds(1);
        player.SetActive(true);
        if (FindObjectOfType<LevelTimerScript>() == true)
        {
            FindObjectOfType<LevelTimerScript>().gameStarted = true;
        }
        if (gm != null)
        {
            gm.NewGame = false;
        }
        SpawnManager_V2.readyToBegin = true;
        StopCoroutine("StageStart");
    }

}
