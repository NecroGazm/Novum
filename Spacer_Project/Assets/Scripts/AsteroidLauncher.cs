using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidLauncher : MonoBehaviour
{
    [Header("Items to spawn in current level")]
    public List<GameObject> obstacles = new List<GameObject>();
    [Header("High value breakable obstacle")]
    public GameObject breakableObstacle; // The one worth points.
    private List<GameObject> boundryPoints = new List<GameObject>();

    private float launchTimer = 10; // The timer behind debries spawning.

    private void Awake()
    {
        //These set up the outer bounds of the spawning area
        boundryPoints.Add(gameObject.transform.GetChild(0).gameObject);
        boundryPoints.Add(gameObject.transform.GetChild(1).gameObject);


        if (obstacles.Count == 0)
        {
            Destroy(this);
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale > 0)
        {
            if (launchTimer >= 0)
            {
                launchTimer -= Time.deltaTime;
            }
            else
            {
                if (breakableObstacle != null)
                {
                    int spawnChance = Random.Range(0, 5);

                    if (spawnChance == 5)
                    {
                        float zPositionToSpawn = Random.Range(boundryPoints[0].transform.position.z, boundryPoints[1].transform.position.z);
                        Vector3 positionToSpawn = new Vector3(boundryPoints[0].transform.position.x, boundryPoints[0].transform.position.y, zPositionToSpawn);
                        GameObject spawnedObject = Instantiate(breakableObstacle, positionToSpawn, Quaternion.identity, null);
                        spawnedObject.gameObject.transform.Rotate(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

                        launchTimer = Random.Range(2, 6);
                    }
                    else
                    {
                        Launch();
                    }
                }
                else
                {
                    Launch();
                }
            }
        }
    }

    public void Launch()
    {
        int obstacleEvent = Random.Range(0, 100);

        if (obstacleEvent >= 0 && obstacleEvent <= 50)
        {
            // Normal spawn
            float xPositionToSpawn = Random.Range(boundryPoints[0].transform.position.x, boundryPoints[1].transform.position.x);
            Vector3 positionToSpawn = new Vector3(xPositionToSpawn, boundryPoints[0].transform.position.y, boundryPoints[0].transform.position.z);

            GameObject spawnedObject = Instantiate(obstacles[Random.Range(0, obstacles.Count - 1)], positionToSpawn, Quaternion.identity, null);
            spawnedObject.gameObject.transform.Rotate(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

            launchTimer = Random.Range(2,6);
        }
        else if (obstacleEvent > 50 && obstacleEvent <= 90)
        {
            // Multi Spawn
            StartCoroutine(Shower(Random.Range(2, 3), .5f));
            launchTimer = Random.Range(4, 6);
        }
        else if ( obstacleEvent > 90 && obstacleEvent <= 100)
        {
            // Shower of debries
            StartCoroutine(Shower(Random.Range(5,8), .35f));
            launchTimer = Random.Range(8, 10);
        }
    }

    IEnumerator Shower(int count, float delay)
    {
        Debug.Log("MultiSpawn");

        for (int i = 0; i < count; i++)
        {
            int spawnType = Random.Range(0, obstacles.Count - 1);
            float xPositionToSpawn = Random.Range(boundryPoints[0].transform.position.x, boundryPoints[1].transform.position.x);
            Vector3 positionToSpawn = new Vector3(xPositionToSpawn, boundryPoints[0].transform.position.y, boundryPoints[0].transform.position.z);

           GameObject spawnedObject = Instantiate(obstacles[spawnType].gameObject, positionToSpawn, Quaternion.identity, null);
            spawnedObject.gameObject.transform.Rotate(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            yield return new WaitForSeconds(delay);
        }
    }
}
