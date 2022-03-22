using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_V2 : MonoBehaviour
{
    private float distance;
    public LevelManager lm;
    public GameObject Boss;
    public int HP;
    public BossHealth BH;
    public ArchHealth AH;
    [HideInInspector]
    public GameManager_V2 gameManager;

    public float projectileLife = 5;
    [HideInInspector]
    public float projectileSpeed = 5;

    public bool isEnemyProjectile;

    private int numberofCollisionsOriginal;
    public int numberOfCollisionBeforeDestruction; // How many hits a projectile will allow before it dies.

    public bool reflected;

    public bool affectedByScoreMod;

    public GameObject explosion;

    void Start()
    {
        lm = GameObject.FindObjectOfType<LevelManager>();
        BH = GameObject.FindObjectOfType<BossHealth>();
        AH = GameObject.FindObjectOfType<ArchHealth>();
        gameManager = GameObject.FindObjectOfType<GameManager_V2>();

        if (isEnemyProjectile == true)
        {
            affectedByScoreMod = false;
        }
        numberofCollisionsOriginal = numberOfCollisionBeforeDestruction;
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * Time.deltaTime * projectileSpeed;
            projectileLife -= Time.deltaTime;

            if (projectileLife <= 0)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                if (affectedByScoreMod == true && numberOfCollisionBeforeDestruction == numberofCollisionsOriginal)
                {
                    gameManager.ResetScoreMultipler();
                }
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        switch (isEnemyProjectile)
        {
            case true:
                //
                if (other.collider.tag == "Player")
                {
                    if (other.transform.parent != null)
                    {
                        if (other.transform.parent.GetComponent<Duplication_Script>() != null)
                        {
                            other.transform.parent.GetComponent<Duplication_Script>().RemoveDuplicate();
                            if (affectedByScoreMod == true)
                            {
                                gameManager.ResetScoreMultipler();
                            }
                            Destroy(gameObject);
                        }
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                else if (other.collider.tag == "Blocker")
                {
                    if (transform.parent != null && transform.parent.childCount <= 1)
                    {
                        Destroy(transform.parent.gameObject);
                    }
                    Destroy(gameObject);
                }
               
                //
                break;
            case false:

                int enemyReward = 25;
                int bossReward = 50;
                int bonusReward = 100;

                if (numberOfCollisionBeforeDestruction <= 0)
                {
                    //
                    if (other.collider.tag == "Enemy1" || other.collider.tag == "Enemy")
                    {
                        GameObject exp = Instantiate(explosion, other.gameObject.transform.position, Quaternion.identity) as GameObject;
                        gameManager.AddScore(enemyReward);
                        Destroy(other.gameObject);
                        Destroy(exp, 2);
                        if (other.collider.GetComponent<DropItemsScript>() != null)
                        {
                            other.collider.GetComponent<DropItemsScript>().CheckIfDroppingItem();
                        }

                        lm.audiosource.PlayOneShot(lm.fxs[1]);
                        if (transform.parent != null && transform.parent.childCount <= 1)
                        {
                            Destroy(transform.parent.gameObject);
                        }
                        Destroy(gameObject);
                    }
                    else if (other.collider.tag == "Boss")
                    {
                        BH.DealDamage();
                        lm.audiosource.PlayOneShot(lm.fxs[3]);
                        GameObject exp = Instantiate(explosion, other.gameObject.transform.position, Quaternion.identity) as GameObject;
                        gameManager.AddScore(bossReward);
                        Destroy(exp, 2);
                        if (transform.parent != null)
                        {
                            Destroy(transform.parent.gameObject);
                        }
                        Destroy(gameObject);
                    }
                    else if (other.collider.tag == "Barrier")
                    {
                        if (transform.parent != null && transform.parent.childCount <= 1)
                        {
                            Destroy(transform.parent.gameObject);
                        }
                        if (affectedByScoreMod == true && numberOfCollisionBeforeDestruction == numberofCollisionsOriginal)
                        {
                            gameManager.ResetScoreMultipler();
                        }
                        Destroy(gameObject);
                    }

                    else if (other.collider.tag == "Blocker" || other.collider.tag == "BossBlocker")
                    {
                        if (transform.parent != null && transform.parent.childCount <= 1)
                        {
                            Destroy(transform.parent.gameObject);
                        }
                        if (affectedByScoreMod == true)
                        {
                            gameManager.ResetScoreMultipler();
                        }
                        lm.audiosource.PlayOneShot(lm.fxs[4]);
                        Destroy(gameObject);

                    }

                    else if (other.collider.tag == "BonusE")
                    {
                        GameObject exp = Instantiate(lm.explosionfx[1], other.gameObject.transform.position, Quaternion.identity) as GameObject;
                        gameManager.AddScore(bonusReward);
                        Destroy(other.gameObject);
                        Destroy(exp, 2);
                        if (other.collider.GetComponent<DropItemsScript>() != null)
                        {
                            other.collider.GetComponent<DropItemsScript>().CheckIfDroppingItem();
                        }

                        lm.audiosource.PlayOneShot(lm.fxs[1]);
                        if (transform.parent != null && transform.parent.childCount <= 1)
                        {
                            Destroy(transform.parent.gameObject);
                        }
                        Destroy(gameObject);
                    }
                }
                else
                {
                    if (other.collider.tag == "Enemy1" || other.collider.tag == "Enemy")
                    {
                        GameObject exp = Instantiate(lm.explosionfx[1], other.gameObject.transform.position, Quaternion.identity) as GameObject;
                        gameManager.AddScore(50 / numberOfCollisionBeforeDestruction);

                        Destroy(other.gameObject);
                        numberOfCollisionBeforeDestruction--;
                        Destroy(exp, 2);
                        if (other.collider.GetComponent<DropItemsScript>() != null)
                        {
                            other.collider.GetComponent<DropItemsScript>().CheckIfDroppingItem();
                        }

                        lm.audiosource.PlayOneShot(lm.fxs[1]);
                    }
                    else if (other.collider.tag == "Boss")
                    {
                        BH.DealDamage();
                        lm.audiosource.PlayOneShot(lm.fxs[3]);
                        GameObject exp = (GameObject)Instantiate(lm.explosionfx[3], other.gameObject.transform.position, Quaternion.identity) as GameObject;
                        gameManager.AddScore(250 / numberOfCollisionBeforeDestruction);
                        Destroy(exp, 2);
                        Destroy(gameObject); // This is to stop multi-hitting on bosses
                    }
                    else if (other.collider.tag == "Barrier")
                    {
                        AH.ADealDamage();
                        lm.audiosource.PlayOneShot(lm.fxs[3]);
                        GameObject exp = (GameObject)Instantiate(lm.explosionfx[3], other.gameObject.transform.position, Quaternion.identity) as GameObject;
                        gameManager.AddScore(50 / numberOfCollisionBeforeDestruction);
                        Destroy(exp, 2);
                    }
                    //
                }
                break;
        }
    }

    public void spawnExplosion()
    {
        GameObject exp = Instantiate(explosion, gameObject.transform.position, Quaternion.identity) as GameObject;
    }
}