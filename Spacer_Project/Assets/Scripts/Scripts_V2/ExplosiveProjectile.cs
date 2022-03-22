using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{
    private float distance;
    public LevelManager lm;
    public GameObject Boss;
    public int HP;
    public BossHealth BH;
    [HideInInspector]
    public GameManager_V2 gameManager;

    public float projectileLife = 5;
    [HideInInspector]
    public float projectileSpeed = 5;

    public bool isEnemyProjectile;

    public bool destroyOnNextCollision;

    public bool affectedByScoreMod;

    public GameObject explosion;

    void Start()
    {
        lm = GameObject.FindObjectOfType<LevelManager>();
        BH = GameObject.FindObjectOfType<BossHealth>();
        gameManager = GameObject.FindObjectOfType<GameManager_V2>();
        Debug.Log(gameManager);
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
                if (affectedByScoreMod == true)
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
                            Destroy(gameObject);
                        }
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                //
                break;
            case false:

                int enemyReward = 25;
                int bossReward = 50;

                if (destroyOnNextCollision == true)
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
                        ExplodeAndDoDamage();
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
                        ExplodeAndDoDamage();
                        Destroy(gameObject);
                    }
                    else if (other.collider.tag == "Blocker")
                    {
                        if (transform.parent != null && transform.parent.childCount <= 1)
                        {
                            Destroy(transform.parent.gameObject);
                        }
                        if (affectedByScoreMod == true)
                        {
                            gameManager.ResetScoreMultipler();
                        }
                        ExplodeAndDoDamage();
                        Destroy(gameObject);
                    }
                    //
                }
                else
                {
                    if (other.collider.tag == "Enemy1" || other.collider.tag == "Enemy")
                    {
                        GameObject exp = Instantiate(lm.explosionfx[1], other.gameObject.transform.position, Quaternion.identity) as GameObject;
                        gameManager.AddScore(50);

                        Destroy(other.gameObject);
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
                        gameManager.AddScore(250);
                        Destroy(exp, 2);
                    }
                    //
                }
                break;
        }
    }

    private void ExplodeAndDoDamage()
    {
        GameObject bigex = Instantiate(lm.explosionfx[0], gameObject.transform.position, Quaternion.identity) as GameObject;
        lm.audiosource.PlayOneShot(lm.fxs[1]);

        RaycastHit[] objectsHit = Physics.SphereCastAll(gameObject.transform.position, 2, gameObject.transform.position);

        for (int i = 0; i < objectsHit.Length; i++)
        {
            if (objectsHit[i].collider.gameObject.GetComponent<EnemyBehavior_V2>())
            {
                switch (isEnemyProjectile)
                {
                    case true:
                        //
                        if (objectsHit[i].collider.tag == "Player")
                        {
                            Destroy(gameObject);
                        }
                        //
                        break;
                    case false:

                        int enemyReward = 25;
                        int bossReward = 50;

                        if (objectsHit[i].collider.tag == "Enemy1" || objectsHit[i].collider.tag == "Enemy")
                        {
                            GameObject exp = Instantiate(lm.explosionfx[1], objectsHit[i].collider.gameObject.transform.position, Quaternion.identity) as GameObject;
                            gameManager.AddScore(enemyReward);
                            Destroy(objectsHit[i].collider.gameObject);
                            Destroy(exp, 2);
                            if (objectsHit[i].collider.GetComponent<DropItemsScript>() != null)
                            {
                                objectsHit[i].collider.GetComponent<DropItemsScript>().CheckIfDroppingItem();
                            }

                            lm.audiosource.PlayOneShot(lm.fxs[1]);
                            Destroy(gameObject);
                        }
                        else if (objectsHit[i].collider.tag == "Boss")
                        {
                            BH.DealDamage();
                            lm.audiosource.PlayOneShot(lm.fxs[3]);
                            GameObject exp = Instantiate(lm.explosionfx[3], objectsHit[i].collider.gameObject.transform.position, Quaternion.identity) as GameObject;
                            gameManager.AddScore(bossReward);
                            Destroy(exp, 2);
                            Destroy(gameObject);
                        }
                        else if (objectsHit[i].collider.tag == "Blocker")
                        {
                            Destroy(gameObject);
                        }
                        //
                        break;
                }
                break;
            }
        }

    }

}
