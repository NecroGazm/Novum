using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Boss_Behavior_Script : MonoBehaviour
{
    //------------Scipts Needed for this to operate--------------------------------------
    [HideInInspector]
    public BossHealth bossHPScript;
    [HideInInspector]
    public SpawnManager_V2 levelWaveSpawner;


    //------------Navigation Related--------------------------------------
    public List<GameObject> navPoints = new List<GameObject>(); // These are the points the ship will move between while on screen.

    public List<GameObject> flybyAboveNavpoints = new List<GameObject>(); // The inital nav point should be the one the ship idles in (hidden off screen).
    private GameObject offScreenPoint; // This is where the ship boss will hide when not in play.

    [HideInInspector]
    public GameObject destination; // The bosses next movement positon.
    private int destinationInt;

    //------------Weapon Related--------------------------------------
    private float fireProjectileTimer = 3; // The actual timer for shooting.
    [Tooltip("How much time between shots should the boss have under usual circumstances?")]
    public float fireCooldownAmount = 1;

    public GameObject projectileToFire;

    //------------Reflector Related--------------------------------------
    private GameObject reflectorLight; // The object pair that shows the reflector is operational.
    private float reflectortimer = 0; // The timer for the reflector.
    private float reflectorCooldown = 5; // The time the reflector needs before it's ready to be used again.

    //------------Shield Related--------------------------------------
    private GameObject overShield; // The actual shield object.
    private Vector3 shieldScale; // The maximum values for the shield scale size.

    private float overShieldReturnTimer;

    //------------State Related--------------------------------------
    private bool shouldBeInPlayspace = true; // This controls if the boss should be in the playspace or not.

    private bool shouldBeMoving = true; // This controls if the boss should be moving to the next position or not.

    private bool bossHasLeft; // This is just a way for this script to tell if the boss has left playspace once yet already.







    private void Start()
    {
        offScreenPoint = flybyAboveNavpoints[0]; // Sets the offscreenPoint to be whatever the first flyby point is.
        bossHPScript = FindObjectOfType<BossHealth>(); // Sets the bossHP script to the one found in the level.
        destination = navPoints[0];
        reflectorLight = gameObject.transform.Find("ReflectorIndicator").gameObject;

        overShield = gameObject.transform.Find("Ship_Shield").gameObject;
        shieldScale = new Vector3(overShield.transform.localScale.x, overShield.transform.localScale.y, overShield.transform.localScale.z);
        overShield.transform.localScale = new Vector3(0, 0, 0);

        if (GameObject.FindObjectOfType<SpawnManager_V2>())
        {
            levelWaveSpawner = GameObject.FindObjectOfType<SpawnManager_V2>();
        }
        else
        {
            Debug.LogWarning("SPAWNER NOT FOUND IN LEVEL");
        }
        SpawnManager_V2.readyToBegin = false;
        fireProjectileTimer = 7;
    }

    private void FixedUpdate()
    {
        if (bossHasLeft == false)
        {
            SpawnManager_V2.readyToBegin = false;
        }
        else if (bossHasLeft == true && SpawnManager_V2.readyToBegin == false)
        {
            SpawnManager_V2.readyToBegin = true;
        }
        if (bossHasLeft == true && shouldBeInPlayspace == false)
        {
            if (levelWaveSpawner.waveOver == true)
            {
                if (levelWaveSpawner.enemiesAliveInScene.Count == 2)
                {
                    EnterPlaySpace();
                }
            }
        }

        if (reflectortimer <= 0 || overShield.activeSelf == true)
        {
            RaycastHit[] objectsHit = null;
            if (overShield.activeSelf == true)
            {
                objectsHit = Physics.SphereCastAll(gameObject.transform.position, 4, transform.forward);
            }
            else
            {
                objectsHit = Physics.SphereCastAll(gameObject.transform.position, 3, transform.forward);
            }

            List<GameObject> bulletsNearby = new List<GameObject>();

            for (int i = 0; i < objectsHit.Length; i++)
            {
                if (objectsHit[i].collider.gameObject.GetComponent<Projectile_V2>())
                {
                    if (objectsHit[i].collider.gameObject.GetComponent<Projectile_V2>().reflected == true && overShield.activeSelf == true)
                    {
                        DisableShield();
                        return;
                    }

                    if (objectsHit[i].collider.gameObject.GetComponent<Projectile_V2>().isEnemyProjectile == false)
                    {
                        objectsHit[i].collider.gameObject.transform.LookAt(gameObject.transform.position); // Makes the bullet face the ship in its current position.
                        objectsHit[i].collider.gameObject.transform.Rotate(new Vector3(0, 180, 0)); // Flips the bullet back around.
                        objectsHit[i].collider.gameObject.GetComponent<Projectile_V2>().isEnemyProjectile = true; // Transforms the projectile into a player projectile.
                        objectsHit[i].collider.gameObject.tag = "Enemy_Bullet";
                        reflectorLight.SetActive(false);
                        reflectortimer = reflectorCooldown;
                        if (gameObject.GetComponent<ParticleSystem>() != null)
                        {
                            gameObject.GetComponent<ParticleSystem>().Play();
                        }
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            if ((bossHPScript.HP <= bossHPScript.defaultHealth / 2) && bossHasLeft == false) // Check to see if the boss should leave the screen for a second
            {
                LeavePlaySpace();
                bossHasLeft = true;
            }
            else if (bossHPScript.HP <= 0)
            {
                Destroy(gameObject);
            }

            if (shouldBeInPlayspace == true) // Shooting
            {
                fireProjectileTimer -= Time.deltaTime;

                if (fireProjectileTimer <= 0)
                {
                    FireProjectile(false);
                }
            }

            if (reflectortimer <= 0) // Reflector Status
            {
                if (reflectorLight.activeSelf == false)
                {
                    reflectorLight.SetActive(true);
                }
            }
            else
            {
                if (reflectorLight.activeSelf == true)
                {
                    reflectorLight.SetActive(false);
                }
                reflectortimer -= Time.deltaTime;
            }


            if (shouldBeMoving == true) // Moving check
            {
                MoveToNextNavPoint();
            }


            if (overShield.activeSelf == true) // Shield status check
            {
                if (overShield.transform.localScale.x != shieldScale.x)
                {
                    RegenerateOverShield();
                }
            }
            else
            {
                if (shouldBeInPlayspace == true && overShieldReturnTimer > 0)
                {
                    overShieldReturnTimer -= Time.deltaTime;
                }

                if (overShieldReturnTimer <= 0)
                {
                    overShield.SetActive(true);
                }
            }
        }

    }


    public void FireProjectile(bool aimed) // This is where the boss will shoot. If aimed is true, it will fire aiming at the player.
    {
        if (bossHasLeft == false)
        {
            //Fire the projectile
            int FireSpread = Random.Range(0, 4);

            if (FireSpread == 1)
            {
                //Fire single projectile
                GameObject bullet = Instantiate(projectileToFire, gameObject.transform.GetChild(0).transform.position, Quaternion.identity, null);
                bullet.GetComponent<Projectile_V2>().isEnemyProjectile = true;
                bullet.tag = "Enemy_Bullet";
                bullet.transform.LookAt(gameObject.transform.position);
                bullet.transform.Rotate(0, 180, 0);
                bullet = null;

                fireProjectileTimer = fireCooldownAmount * Random.Range(.5f, 1.25f);
            }
            else
            {
                GameObject bullet = Instantiate(projectileToFire, gameObject.transform.GetChild(0).transform.position + new Vector3(.25f, 0, 0), Quaternion.identity, null);
                bullet.GetComponent<Projectile_V2>().isEnemyProjectile = true;
                bullet.tag = "Enemy_Bullet";
                bullet.transform.Rotate(0, 160, 0);
                bullet = null;

                bullet = Instantiate(projectileToFire, gameObject.transform.GetChild(0).transform.position + new Vector3(0, 0, 0), Quaternion.identity, null);
                bullet.GetComponent<Projectile_V2>().isEnemyProjectile = true;
                bullet.tag = "Enemy_Bullet";
                bullet.transform.Rotate(0, 180, 0);
                bullet = null;

                bullet = Instantiate(projectileToFire, gameObject.transform.GetChild(0).transform.position + new Vector3(-.25f, 0, 0), Quaternion.identity, null);
                bullet.GetComponent<Projectile_V2>().isEnemyProjectile = true;
                bullet.tag = "Enemy_Bullet";
                bullet.transform.Rotate(0, 200, 0);
                bullet = null;

                //Fire spread projectile
                fireProjectileTimer = fireCooldownAmount;
            }
        }
        else
        {
            //Fire the projectile
            int FireSpread = Random.Range(0, 8);

            if (FireSpread == 0)
            {
                Debug.Log("Big shot");
                for (int i = 0; i < Random.Range(3,8); i++)
                {
                    GameObject bullet = Instantiate(projectileToFire, gameObject.transform.GetChild(0).transform.position, Quaternion.identity, null);
                    bullet.GetComponent<Projectile_V2>().isEnemyProjectile = true;
                    bullet.tag = "Enemy_Bullet";
                    bullet.transform.LookAt(gameObject.transform.position);
                    bullet.transform.Rotate(0, Random.Range(130, 210), 0);
                    bullet = null;
                }
                fireProjectileTimer = fireCooldownAmount * Random.Range(1f, 1.25f);
            }
            else
            {
                GameObject bullet = Instantiate(projectileToFire, gameObject.transform.GetChild(0).transform.position, Quaternion.identity, null);
                bullet.GetComponent<Projectile_V2>().isEnemyProjectile = true;
                bullet.tag = "Enemy_Bullet";
                bullet.transform.LookAt(gameObject.transform.position);
                bullet.transform.Rotate(0, Random.Range(140, 200), 0);
                bullet = null;

                fireProjectileTimer = fireCooldownAmount * Random.Range(.25f, .75f);
            }
        }
    }


    public void LeavePlaySpace() // Make the boss leave the play space and sit in their idle position.
    {
        shouldBeMoving = true;

        // Hide the boss UI.
        RBossStarter bossUIScript;
        if (FindObjectOfType<RBossStarter>())
        {
            bossUIScript = FindObjectOfType<RBossStarter>();
            bossUIScript.HideBossUI();

            destination = offScreenPoint;
            shouldBeInPlayspace = false;

            SpawnManager_V2.readyToBegin = true;
        }
        else
        {
            Debug.LogWarning("No boss UIScript found");
        }

        destinationInt = 0;
        destination = flybyAboveNavpoints[0];
        //Make the boss leave the playspace
    }


    public void MoveToNextNavPoint()
    {
        string whatSettingIsValid = "";
        if (navPoints.Contains(destination))
        {
            whatSettingIsValid = "navPoints";
        }
        else if (flybyAboveNavpoints.Contains(destination))
        {
            whatSettingIsValid = "flyByAboveNavPoints";
        }
        switch (whatSettingIsValid)
        {
            case "navPoints":
                {
                    if (Vector3.Distance(gameObject.transform.position, navPoints[destinationInt].gameObject.transform.position) <= .1f)
                    {
                        if ((destinationInt + 1) == navPoints.Count)
                        {
                            destination = navPoints[0];
                            destinationInt = 0;
                        }
                        else
                        {
                            destinationInt++;
                            destination = navPoints[destinationInt];
                        }
                    }

                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, destination.transform.position, 5 * Time.deltaTime);
                    break;
                }
            case "flyByAboveNavPoints":
                {
                    if (shouldBeInPlayspace == false)
                    {
                        if (Vector3.Distance(gameObject.transform.position, flybyAboveNavpoints[destinationInt].gameObject.transform.position) > .1f)
                        {
                            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, destination.transform.position, 15 * Time.deltaTime);
                        }
                    }
                    else
                    {
                        if (Vector3.Distance(gameObject.transform.position, flybyAboveNavpoints[destinationInt].gameObject.transform.position) <= .1f)
                        {
                            if ((destinationInt + 1) == flybyAboveNavpoints.Count)
                            {
                                destination = navPoints[0];
                                destinationInt = 0;
                                gameObject.transform.localScale = new Vector3(1, 1, 1);
                            }
                            else
                            {
                                destinationInt++;
                                destination = flybyAboveNavpoints[destinationInt];
                                gameObject.transform.localScale = new Vector3(2,2,2);
                            }
                        }
                        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, destination.transform.position, 15 * Time.deltaTime);
                    }

                    break;
                }
            case "":
                {
                    Debug.LogWarning("Out of setting switch function!");
                    break;
                }
        }
    }


    public void EnterPlaySpace() // Fly down below the play space
    {
        destination = offScreenPoint;
        gameObject.transform.position = offScreenPoint.transform.position;

        shouldBeInPlayspace = true;
        fireProjectileTimer = 7;
        overShield.SetActive(true);
    }


    public void DisableShield()
    {
        overShield.transform.localScale = new Vector3(0,0,0);
        overShield.SetActive(false);
        overShieldReturnTimer = 8;
    }

    public void RegenerateOverShield()
    {
        overShield.transform.localScale = Vector3.Lerp(overShield.transform.localScale, shieldScale, .05f);
    }
}
// Mace Jimino 2020