using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior_V2 : MonoBehaviour
{
    [HideInInspector]
    public EnemyGridPositionObject_V2 gridPosition;

    [HideInInspector]
    public List<Vector3> spawnPath = new List<Vector3>();
    [HideInInspector]
    public List<Vector3> divePath = new List<Vector3>();
    [HideInInspector]
    public bool freshlySpawned = true;
    [HideInInspector]
    public int currentMovementIndex;
    [HideInInspector]
    public bool idle = false;
    public GameObject projectileToSpawn;
    float curDelay;
    public float fireRate = 1f;
    
    public bool aimsAtPlayer;

    private void Awake()
    {
        FindObjectOfType<SpawnManager_V2>().enemiesAliveInScene.Add(this.gameObject);
    }

    private void OnDestroy()
    {
        if (FindObjectOfType<SpawnManager_V2>())
        {
            FindObjectOfType<SpawnManager_V2>().enemiesAliveInScene.Remove(this.gameObject);
        }
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            if (freshlySpawned == true)
            {
                FollowSpawnPath();
            }
            else if (freshlySpawned == false && idle == false) //Diving
            {
                Dive();
            }
        }
    }

    public void FollowSpawnPath()
    {
        if (currentMovementIndex >= spawnPath.Count)
        {
            Debug.Log("Done moving!");
            freshlySpawned = false;
            gameObject.transform.parent = gridPosition.gameObject.transform;
            gameObject.transform.position = gameObject.transform.parent.transform.position;
            idle = true;
            currentMovementIndex = 0;
        }
        else if (Vector3.Distance(gameObject.transform.position, spawnPath[currentMovementIndex]) < .1f)
        {
            currentMovementIndex++;
        }
        else
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, spawnPath[currentMovementIndex], 7 * Time.deltaTime);
        }
    }

    public void Dive()
    {
        if (currentMovementIndex >= divePath.Count)
        {
            Debug.Log("Done diving!");
            freshlySpawned = false;
            gameObject.transform.parent = gridPosition.gameObject.transform;
            gameObject.transform.position = gameObject.transform.parent.transform.position;
            idle = true;
            currentMovementIndex = 0;
        }
        else if (Vector3.Distance(gameObject.transform.position, divePath[currentMovementIndex]) < .1f)
        {
            currentMovementIndex++;
        }
        else
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, divePath[currentMovementIndex], 7 * Time.deltaTime);

            if (gameObject.transform.position.z + 2 > GameObject.FindGameObjectWithTag("Player").transform.position.z && (currentMovementIndex <= divePath.Count - 5))
            {
                curDelay += Time.deltaTime;

                if (curDelay >= fireRate && projectileToSpawn != null)
                {
                    if (FindObjectOfType<LevelManager>())
                    {
                        GameObject lm = FindObjectOfType<LevelManager>().gameObject;
                        lm.GetComponent<AudioSource>().PlayOneShot(lm.GetComponent<LevelManager>().fxs[1]);
                    }
                    GameObject bullet = Instantiate(projectileToSpawn, gameObject.transform.position, Quaternion.identity, null);
                    bullet.GetComponent<Projectile_V2>().isEnemyProjectile = true;
                    if (aimsAtPlayer == true)
                    {

                        bullet.transform.LookAt(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position);
                    }
                    else
                    {
                        bullet.transform.LookAt(gameObject.transform.position + new Vector3(0, 0, -5));
                    }
                    curDelay = 0;
                }
            }
        }
    }

    public void KillSelf()
    {
        Destroy(gameObject);
    }
}
