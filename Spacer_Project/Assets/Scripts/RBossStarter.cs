using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBossStarter : MonoBehaviour
{
   
    public GameObject boss;
    public GameObject BossBar;
    public GameObject BossMusic;
    public GameObject MinionMusic;
    public GameObject Barrier;

    void Start()
    {

        BossMusic.SetActive(false);
        boss.SetActive(false);
        BossBar.SetActive(false);
    }

    void Update()
    {
        if (Barrier == null)
        {
            BossMusic.SetActive(true);
            MinionMusic.SetActive(false);
            if (Barrier == null)
            {
                if (boss != null)
                {
                    boss.SetActive(true);
                }
            }
            BossBar.SetActive(true);
        }
    }

    public void HideBossUI() // Call this to hide the boss UI when valid
    {
        boss.SetActive(false);
        BossBar.SetActive(false);
    }

    public void ShowBossUI() // Call this to show the boss UI when valid
    {
        boss.SetActive(true);
        BossBar.SetActive(true);
    }
}
