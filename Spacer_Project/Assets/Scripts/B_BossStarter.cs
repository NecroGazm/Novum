using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_BossStarter : MonoBehaviour
{

    private SpawnManager_V2 currentSpawner;
    public GameObject boss;
    public GameObject BossBar;
    public GameObject BossMusic;
    public GameObject MinionMusic;
    public GameObject Spawn;
    public BossHealth BH;
    // Start is called before the first frame update
    void Start()
    {
        BH = GameObject.FindObjectOfType<BossHealth>();
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
        if (currentSpawner.currentWave == 2)
        {
            BossMusic.SetActive(true);
            MinionMusic.SetActive(false);
            
            if (boss != null)
            {
                boss.SetActive(true);
            }
            BossBar.SetActive(true);
        }
       
        if (BH.HP == 1)
        {
            var enemies = FindObjectsOfType<EnemyBehavior_V2>();

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].KillSelf();
            }
            Spawn.SetActive(false);
        }
    }
}
