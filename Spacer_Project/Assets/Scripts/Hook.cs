using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [HideInInspector]
    public GameObject player;
    private float speed = 8;
    private LevelManager levelManager;
    private BossHealth BH;
    [HideInInspector]
    public bool retractHook;

    private float hookLifetime = 1;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        if (FindObjectOfType<BossHealth>())
        {
            BH = GameObject.FindObjectOfType<BossHealth>();
        }
    }

    public void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Blocker")
        {
            EnmbedInto(collider.gameObject);
            hookLifetime = 1;
        }

        //
        if (collider.collider.tag == "Enemy1" || collider.collider.tag == "Enemy")
        {
            Debug.Log("Enemy shot!");
            GameObject exp = (GameObject)Instantiate(levelManager.explosionfx[1], collider.gameObject.transform.position, Quaternion.identity) as GameObject;
           // GameManager_V2.scoreValue += 50;
            Destroy(collider.gameObject);
            Destroy(exp, 2);

            levelManager.audiosource.PlayOneShot(levelManager.fxs[1]);
        }
        else if (collider.collider.tag == "Boss")
        {
            Debug.Log("Boss shot!");
            BH.DealDamage();
            levelManager.audiosource.PlayOneShot(levelManager.fxs[3]);
            GameObject exp = (GameObject)Instantiate(levelManager.explosionfx[3], collider.gameObject.transform.position, Quaternion.identity) as GameObject;
           // GameManager_V2.scoreValue += 250;
            Destroy(exp, 2);
        }
        //
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            if (gameObject.transform.parent == null && retractHook == false)
            {
                hookLifetime -= Time.deltaTime;
                if (hookLifetime <= 0)
                {
                    Destroy(gameObject);
                }

                gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * Time.deltaTime * speed;
            }

            if (gameObject.transform.parent != null && retractHook == false) // If still embedded
            {
                if (player.GetComponent<BoostPower>())
                {
                    if (player.GetComponent<Player>().speed != 20)
                    {
                        player.GetComponent<BoostPower>().StopCoroutine("SpeedB");

                        player.GetComponent<BoostPower>().StartCoroutine("SpeedB", 3);
                    }
                }
            }

            if (retractHook == true)
            {
                if (Vector3.Distance(gameObject.transform.position, player.transform.position) > .25f)
                {
                    player.GetComponent<BoostPower>().StopCoroutine("SpeedB");
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, player.transform.position, Time.deltaTime * (speed * 1.5f));
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            gameObject.GetComponent<LineRenderer>().SetPosition(0, gameObject.transform.position);
            gameObject.GetComponent<LineRenderer>().SetPosition(1, player.transform.position);
        }
    }

    private void EnmbedInto(GameObject objectHit)
    {
        gameObject.transform.parent = objectHit.transform;

        if (player.GetComponent<BoostPower>())
        {
            player.GetComponent<BoostPower>().StopCoroutine("SpeedB");

            player.GetComponent<BoostPower>().StartCoroutine("SpeedB",3);
        }
    }
}
