using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStarter : MonoBehaviour
{
    private SpawnManager_V2 currentSpawner;
    public GameObject boss;
    public GameObject BossBar;
    public GameObject BossMusic;
    public GameObject MinionMusic;

    // Start is called before the first frame update
    void Start()
    {

       BossMusic.SetActive(false);
        boss.SetActive(false);
        BossBar.SetActive(false);
        if (FindObjectOfType<SpawnManager_V2>())
        {
            currentSpawner = FindObjectOfType<SpawnManager_V2>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(currentSpawner.expendedWaves == true && currentSpawner.enemiesAliveInScene.Count <= 0)
        {
            BossMusic.SetActive(true);
            MinionMusic.SetActive(false);
            if (boss != null)
            {
                boss.SetActive(true);
            }
            BossBar.SetActive(true);
        }
    }
}
