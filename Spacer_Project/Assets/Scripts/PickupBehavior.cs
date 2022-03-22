using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehavior : MonoBehaviour
{
    private int speed = 2;
    private float lowerThreshold;

    [Tooltip("0 is don't change, 1 is laser, 2 is spread, 3 is explosive")]
    public int weaponTypeToAdd;

    [HideInInspector]
    public GameObject player;

    void Start()
    {
        lowerThreshold = GameObject.FindGameObjectWithTag("Player").transform.position.z - 1;
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - Time.deltaTime * speed);
        }

        if (gameObject.transform.position.z < lowerThreshold)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (player == null)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }
        else
        {
            if (Vector3.Distance(gameObject.transform.position, player.transform.position) < .25f)
            {
                if (player.gameObject.GetComponent<BoostPower>() != null)
                {
                    switch (gameObject.tag)
                    {
                        case ("IBoost"):
                            {
                                player.gameObject.GetComponent<BoostPower>().StopCoroutine("Invincible");
                                player.gameObject.GetComponent<BoostPower>().StartCoroutine("Invincible", 1);
                                Destroy(gameObject);
                                return;
                            }
                        case ("BBoost"):
                            {
                                player.gameObject.GetComponent<BoostPower>().StopCoroutine("Bullet");
                                player.gameObject.GetComponent<BoostPower>().StartCoroutine("Bullet", 1);
                                Destroy(gameObject);
                                return;
                            }
                        case ("SpeedBoost"):
                            {
                                player.gameObject.GetComponent<BoostPower>().StopCoroutine("SpeedB");
                                player.gameObject.GetComponent<BoostPower>().StartCoroutine("SpeedB", 1);
                                Destroy(gameObject);
                                return;
                            }
                    }
                }

                if (weaponTypeToAdd != 0 && (gameObject.tag != "SpeedBoost" || gameObject.tag != "IBoost" || gameObject.tag != "BBoost"))
                {
                    if (player != null)
                    {
                        player.GetComponent<Weapon_Script>().SetNewPlayerWeapon(weaponTypeToAdd);
                    }
                    
                }

                Destroy(gameObject);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<BoostPower>() != null)
            {
                switch (gameObject.tag)
                {
                    case ("IBoost"):
                        {
                            other.gameObject.GetComponent<BoostPower>().StopCoroutine("Invincible");
                            other.gameObject.GetComponent<BoostPower>().StartCoroutine("Invincible", 1);
                            Destroy(gameObject);
                            return;
                        }
                    case ("BBoost"):
                        {
                            other.gameObject.GetComponent<BoostPower>().StopCoroutine("Bullet");
                            other.gameObject.GetComponent<BoostPower>().StartCoroutine("Bullet", 1);
                            Destroy(gameObject);
                            return;
                        }
                    case ("SpeedBoost"):
                        {
                            other.gameObject.GetComponent<BoostPower>().StopCoroutine("SpeedB");
                            other.gameObject.GetComponent<BoostPower>().StartCoroutine("SpeedB", 1);
                            Destroy(gameObject);
                            return;
                        }
                }
            }

            if (weaponTypeToAdd != 0 && (gameObject.tag != "SpeedBoost" || gameObject.tag != "IBoost" || gameObject.tag != "BBoost"))
            {
                if (other != null)
                {
                    if (other.GetComponent<Weapon_Script>())
                    {
                        other.GetComponent<Weapon_Script>().SetNewPlayerWeapon(weaponTypeToAdd);
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}
