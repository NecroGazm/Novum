using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager_V2 : MonoBehaviour
{
    public List<WaveScript_V2> wavesToSpawnInLevel = new List<WaveScript_V2>();

    [Tooltip("How much time between enemy spawns for this level?")]
    public float timeBetweenEnemySpawns;
    private float ogTimeBetweenEnemySpawns;
    [Tooltip("How much time between waves for this level?")]
    public float timeBetweenWaves;
    private float ogTimeBetweenWaves;
    [HideInInspector]
    public bool waveOver = false;

    public int currentWave = 0;
    private int enemyIndexToSpawn = 0;

    public List<GameObject> enemiesAliveInScene = new List<GameObject>();
    [HideInInspector]
    public bool expendedWaves = false;
    [HideInInspector]
    public static bool readyToBegin;

    [Tooltip("This is used for levels that don't end, it'll just keep repeating waves until another condition is met.")]
    public bool repeatWaves;

    private void Awake()
    {
        FixZeros(); // If the inspector was not attended to, fix values to something realistic.

        ogTimeBetweenEnemySpawns = timeBetweenEnemySpawns; // Sets the og time to be reset.
        ogTimeBetweenWaves = timeBetweenWaves; // Sets the og time for waves to be reset.
    }

    private void Update()
    {
        if (Time.timeScale > 0 && readyToBegin == true)
        {
            if (enemiesAliveInScene.Count == 0)
            {
                SpawnNextEnemyInWave();
            }

            if (waveOver == true && expendedWaves == false) // If there are waves and the last wave just finished.
            {
                timeBetweenWaves -= Time.deltaTime;

                if (timeBetweenWaves <= 0)
                {
                    SpawnNextEnemyInWave();
                    timeBetweenWaves = ogTimeBetweenWaves;
                    waveOver = false;
                }
            }
            else if (waveOver == false && expendedWaves == false) // If there are waves and it the current wave hasn't finished.
            {
                timeBetweenEnemySpawns -= Time.deltaTime;

                if (timeBetweenEnemySpawns <= 0)
                {
                    SpawnNextEnemyInWave();
                    timeBetweenEnemySpawns = ogTimeBetweenEnemySpawns;
                }
            }
        }
    }

    public void SpawnNextEnemyInWave()
    {
        if (repeatWaves == true)
        {
            if (currentWave >= wavesToSpawnInLevel.Count)
            {
                currentWave = 0;
            }
        }

        if (currentWave >= wavesToSpawnInLevel.Count && repeatWaves == false)
        {
           // Debug.Log("No more waves of enemies to be spawned!");
            expendedWaves = true;
        }
        else if (enemyIndexToSpawn < wavesToSpawnInLevel[currentWave].enemiesToSpawnInWave.Count) // If there are more enemies to spawn in the current wave.
        {
            if (gameObject.GetComponent<EnemyGrid_V2>().CheckForOpenPosition().Item1 == true)
            {
                int slot = gameObject.GetComponent<EnemyGrid_V2>().CheckForOpenPosition().Item2; // What slot of the grid is open.

                GameObject enemySpawned = Instantiate(wavesToSpawnInLevel[currentWave].enemiesToSpawnInWave[enemyIndexToSpawn]); // Spawn the next enemy and set them to a local variable

                enemyIndexToSpawn++; // Increment the enemy spawn index.

                gameObject.GetComponent<EnemyGrid_V2>().gridPositions[slot].residentEnemy = enemySpawned;
                enemySpawned.GetComponent<EnemyBehavior_V2>().gridPosition = gameObject.GetComponent<EnemyGrid_V2>().gridPositions[slot];
                enemySpawned.transform.position = enemySpawned.GetComponent<EnemyBehavior_V2>().gridPosition.gameObject.transform.position;
                for (int i = 0; i < wavesToSpawnInLevel[currentWave].pathToTake.transform.childCount; i++)
                {
                    enemySpawned.GetComponent<EnemyBehavior_V2>().spawnPath.Add(wavesToSpawnInLevel[currentWave].pathToTake.transform.GetChild(i).transform.position);
                }
                enemySpawned.GetComponent<EnemyBehavior_V2>().spawnPath.Add(gameObject.GetComponent<EnemyGrid_V2>().gridPositions[slot].gameObject.transform.position);
                enemySpawned.transform.position = enemySpawned.GetComponent<EnemyBehavior_V2>().spawnPath[0];
                enemySpawned = null;
                // Debug.LogWarning("Enemy spawned at index(" + enemyIndexToSpawn + ") in wave (" + currentWave + ")");
                if (repeatWaves == false)
                {
                    GameObject.FindObjectOfType<EnemyGrid_V2>().timeBetweenDives += .5f;
                }
            }
            else
            {
                timeBetweenEnemySpawns = ogTimeBetweenEnemySpawns;
               // Debug.Log("No open valid position");
            }
        }
        else if (enemyIndexToSpawn >= wavesToSpawnInLevel[currentWave].enemiesToSpawnInWave.Count) // If there are no more enemies to spawn in the current wave
        {
            if (currentWave <= wavesToSpawnInLevel.Count)
            {
                currentWave++;
            }

            waveOver = true;
           // Debug.Log("out of enemies in wave: " + currentWave);
            enemyIndexToSpawn = 0;
        }

    }

    private void FixZeros()
    {
        if (timeBetweenEnemySpawns <= 0)
        {
            timeBetweenEnemySpawns = .5f;
        }

        if (timeBetweenWaves <= 0)
        {
            timeBetweenWaves = 5f;
        }
    }
}
