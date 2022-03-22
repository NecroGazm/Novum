using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits_Master_Script : MonoBehaviour
{
    public List<GameObject> creditsBlocks = new List<GameObject>();
    private int creditsIndexToSpawn;

    private float spawnNextCreditsBlockTimer;

    private void OnEnable()
    {
        spawnNextCreditsBlockTimer = 1;
        foreach (GameObject block in creditsBlocks)
        {
            block.transform.position = gameObject.transform.position;
            block.SetActive(false);
        }
        creditsIndexToSpawn = creditsBlocks.Count;
    }

    private void FixedUpdate()
    {
        if (Time.timeScale > 0)
        {
            if (spawnNextCreditsBlockTimer > 0)
            {
                spawnNextCreditsBlockTimer -= Time.deltaTime;
            }
            else if (spawnNextCreditsBlockTimer <= 0)
            {
                if (creditsIndexToSpawn > 0)
                {
                    creditsBlocks[creditsIndexToSpawn - 1].SetActive(true);
                    creditsIndexToSpawn--;
                    spawnNextCreditsBlockTimer = 1f;
                }
                else
                {
                    return;
                }
            }
        }
    }
}
