using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeControl : MonoBehaviour
{
    //------------Exterior Script Related--------------------------------------
    private BossHealth bossScript;
    private SpawnManager_V2 spawner;

    //------------Shield Related--------------------------------------
    private Vector3 shieldScale; // The maximum values for the shield scale size.
    private GameObject overShield; // The actual shield
    private float overShieldReturnTimer;

    //------------Navigation Related--------------------------------------
    private Vector3 destination;
    private int destinationInt;
    public List<GameObject> navPoints = new List<GameObject>();
    public GameObject hideySpot;
    private bool hiding = false;

    private bool hidAlready = false;

    //-----------Audio Related----------------------------------------
    public AudioClip[] MasterBonesSpecificSounds;

    //------------Combat Related--------------------------------------
    private float shootingTimer;
    public GameObject projectile;
    public GameObject bossteroid;

    //------------Animation Related--------------------------------------
    private Animation ownAnimator;

    private void Awake()
    {
        ownAnimator = gameObject.transform.GetComponentInChildren<Animation>();
        overShield = gameObject.transform.Find("Ship_Shield").gameObject;
        shieldScale = new Vector3(overShield.transform.localScale.x, overShield.transform.localScale.y, overShield.transform.localScale.z);
        overShield.transform.localScale = new Vector3(0, 0, 0);
        bossScript = FindObjectOfType<BossHealth>();
        destination = navPoints[0].transform.position;
        spawner = FindObjectOfType<SpawnManager_V2>();
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            if (ownAnimator.isPlaying == false)
            {
                ownAnimator.Play("Idle");
            }

            if (hiding == false)
            {
                if (Vector3.Distance(gameObject.transform.position, destination) > .25f)
                {
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, destination, Mathf.Clamp(Vector3.Distance(gameObject.transform.position, destination), 3, 20) * Time.deltaTime);
                }
                else
                {
                    if (destinationInt + 1 > navPoints.Count)
                    {
                        destinationInt = 0;
                        destination = navPoints[0].transform.position;
                    }
                    else
                    {
                        destination = navPoints[destinationInt].transform.position;
                        destinationInt++;
                    }
                }
            }
            else
            {
                if (destination != hideySpot.transform.position)
                {
                    destination = hideySpot.transform.position;
                }

                if (Vector3.Distance(gameObject.transform.position, destination) > .25f)
                {
                    gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, destination, 8 * Time.deltaTime);
                }
            }

            if (hiding == false)
            {
                shootingTimer -= Time.deltaTime;

                if (shootingTimer <= 0)
                {
                    if (ownAnimator.IsPlaying("Laugh") == true)
                    {
                        shootingTimer = 1;
                        return;
                    }
                    int attack = Random.Range(1, 8);

                    switch (attack)
                    {
                        case 1: //Shoot
                            {
                                GameObject spawnedBullet = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, null);
                                spawnedBullet.GetComponent<Projectile_V2>().isEnemyProjectile = true;
                                spawnedBullet.transform.LookAt(FindObjectOfType<Player>().transform.position);
                                if (hidAlready == true)
                                {
                                    shootingTimer = .6f;
                                }
                                else
                                {
                                    shootingTimer = 1;
                                }
                                return;
                            }
                        case 2: //Spawn
                            {
                                SpawnMinion();
                                shootingTimer = 1;
                                return;
                            }
                        case 3: //Lightning
                            {
                                StartCoroutine("Lightning");
                                if (hidAlready == true)
                                {
                                    shootingTimer = 1f;
                                }
                                else
                                {
                                    shootingTimer = 2;
                                }
                                return;
                            }
                        case 4: //Asteroid
                            {
                                GameObject audioObject = FindObjectOfType<SoundManager>().gameObject;
                                gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[3], audioObject.GetComponent<SoundManager>().soundEffectsSlider.value);
                                Instantiate(bossteroid, gameObject.transform.position, Quaternion.identity, null);
                                return;
                            }
                        case 5: //LotsOfShots
                            {
                                StartCoroutine("FireCurse");
                                if (hidAlready == true)
                                {
                                    shootingTimer = 2f;
                                }
                                else
                                {
                                    shootingTimer = 3;
                                }
                                return;
                            }
                        case 6: //Shield
                            {
                                if (overShield.activeSelf == false)
                                {
                                    ActiveOvershieldStatus();
                                    if (hidAlready == true)
                                    {
                                        shootingTimer = .6f;
                                    }
                                    else
                                    {
                                        shootingTimer = 1;
                                    }
                                    return;
                                }
                                else
                                {
                                    shootingTimer = 0;
                                    return;
                                }

                            }
                        case 7: // Laugh
                            {
                                Laugh();
                                shootingTimer = 3;
                                return;
                            }

                    }
                    //shootingTimer = .5f;
                }
            }

            if (overShield.activeSelf == true) // Shield status check
            {
                ActiveOvershieldStatus();
            }

           if (bossScript.HP <= (bossScript.defaultHealth / 2) && hidAlready == false && hiding == false)
            {
                hiding = true;
                hidAlready = true;
                spawner.timeBetweenEnemySpawns = 0;
                spawner.currentWave = 2;
                spawner.expendedWaves = false;
                spawner.waveOver = false;
                spawner.repeatWaves = false;

                if (hideySpot != null)
                {
                    destination = hideySpot.transform.position;
                    Invoke("ReturnToBattle", 5);
                }
            }
        }
    }

    public void SpawnMinion()
    {
        if (spawner.enemiesAliveInScene.Count < 3)
        {
            ownAnimator.Play("Cast");
            GameObject audioObject = FindObjectOfType<SoundManager>().gameObject;
            gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[3], audioObject.GetComponent<SoundManager>().soundEffectsSlider.value);
            spawner.currentWave = 1;
            spawner.timeBetweenEnemySpawns = 0;
            spawner.SpawnNextEnemyInWave();
            if (FindObjectOfType<EnemyBehavior_V2>())
            {
                GameObject buddy = FindObjectOfType<EnemyBehavior_V2>().gameObject;
                buddy.transform.position = gameObject.transform.position;
                buddy.GetComponent<EnemyBehavior_V2>().currentMovementIndex = 3;
            }
            spawner.waveOver = true;
        }
    }

    public void ActiveOvershieldStatus()
    {
        if (overShield.transform.localScale.x != shieldScale.x)
        {
            RegenerateOverShield();
        }

        RaycastHit[] objectsHit = Physics.SphereCastAll(gameObject.transform.position, overShield.GetComponent<MeshRenderer>().bounds.extents.x, transform.forward);

        List<GameObject> bulletsNearby = new List<GameObject>();

        for (int i = 0; i < objectsHit.Length; i++)
        {
            if (objectsHit[i].collider.gameObject.GetComponent<Projectile_V2>())
            {
                if (objectsHit[i].collider.gameObject.GetComponent<Projectile_V2>().reflected == true && overShield.activeSelf == true)
                {
                    DisableShield();
                    Destroy(objectsHit[i].collider.gameObject);
                    return;
                }
                if (objectsHit[i].collider.gameObject.GetComponent<Projectile_V2>().isEnemyProjectile == false)
                {
                    objectsHit[i].collider.gameObject.transform.LookAt(gameObject.transform.position); // Makes the bullet face the ship in its current position.
                    objectsHit[i].collider.gameObject.transform.Rotate(new Vector3(0, 180, 0)); // Flips the bullet back around.
                    objectsHit[i].collider.gameObject.GetComponent<Projectile_V2>().isEnemyProjectile = true; // Transforms the projectile into a player projectile.
                    objectsHit[i].collider.gameObject.tag = "Enemy_Bullet";
                    if (gameObject.GetComponent<ParticleSystem>() != null)
                    {
                        gameObject.GetComponent<ParticleSystem>().Play();
                    }
                    GameObject audioObject = FindObjectOfType<SoundManager>().gameObject;
                    gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[4], audioObject.GetComponent<SoundManager>().soundEffectsSlider.value);
                }
            }
        }
    }
    public void EnableShield()
    {
        overShield.SetActive(true);
        Laugh();
    }
    public void DisableShield()
    {
        GameObject audioObject = FindObjectOfType<SoundManager>().gameObject;
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[5], audioObject.GetComponent<SoundManager>().soundEffectsSlider.value);
        overShield.transform.localScale = new Vector3(0, 0, 0);
        overShield.SetActive(false);
    }
    public void RegenerateOverShield()
    {
        overShield.transform.localScale = Vector3.Lerp(overShield.transform.localScale, shieldScale, .05f);
    }

    public void ReturnToBattle()
    {
        hiding = false;
        destination = navPoints[0].transform.position;
        EnableShield();
    }

    public IEnumerator FireCurse()
    {
        GameObject audioObject = FindObjectOfType<SoundManager>().gameObject;
        if (FindObjectOfType<Player>() == null)
        {
            yield break;
        }
        ownAnimator.Play("Cast");
        GameObject player = FindObjectOfType<Player>().gameObject;
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[1], audioObject.GetComponent<SoundManager>().soundEffectsSlider.value);
        yield return new WaitForSeconds(1f);
        GameObject firedCurseProjectile = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, null);
        firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
        firedCurseProjectile.transform.LookAt(player.transform.position);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[3], audioObject.GetComponent<SoundManager>().soundEffectsSlider.value);
        yield return new WaitForSeconds(.1f);
        firedCurseProjectile = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, null);
        firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
        firedCurseProjectile.transform.LookAt(player.transform.position);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[3], audioObject.GetComponent<SoundManager>().soundEffectsSlider.value);
        yield return new WaitForSeconds(.1f);
        firedCurseProjectile = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, null);
        firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
        firedCurseProjectile.transform.LookAt(player.transform.position);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[3], audioObject.GetComponent<SoundManager>().soundEffectsSlider.value);
        yield return new WaitForSeconds(.1f);
        firedCurseProjectile = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, null);
        firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
        firedCurseProjectile.transform.LookAt(player.transform.position);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[3], audioObject.GetComponent<SoundManager>().soundEffectsSlider.value);
        yield return new WaitForSeconds(.1f);
        firedCurseProjectile = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, null);
        firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
        firedCurseProjectile.transform.LookAt(player.transform.position);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[3], audioObject.GetComponent<SoundManager>().soundEffectsSlider.value);
        yield return new WaitForSeconds(.1f);
        firedCurseProjectile = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, null);
        firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
        firedCurseProjectile.transform.LookAt(player.transform.position);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[3], audioObject.GetComponent<SoundManager>().soundEffectsSlider.value);
        yield return new WaitForSeconds(.1f);
        firedCurseProjectile = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, null);
        firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
        firedCurseProjectile.transform.LookAt(player.transform.position);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[3], audioObject.GetComponent<SoundManager>().soundEffectsSlider.value);
        yield return new WaitForSeconds(.1f);
        firedCurseProjectile = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, null);
        firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
        firedCurseProjectile.transform.LookAt(player.transform.position);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[3], audioObject.GetComponent<SoundManager>().soundEffectsSlider.value);
    }

    public IEnumerator Lightning()
    {
        GameObject audioObject = FindObjectOfType<SoundManager>().gameObject;
        GameObject player = FindObjectOfType<Player>().gameObject;

        if (FindObjectOfType<Light>())
        {
            GameObject light = FindObjectOfType<Light>().gameObject;
            gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[3], audioObject.GetComponent<SoundManager>().soundEffectsSlider.value);
            GameObject firedCurseProjectile = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, null);
            firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
            firedCurseProjectile.transform.LookAt(player.transform.position);
            firedCurseProjectile.GetComponent<Projectile_V2>().projectileSpeed = 10;
            firedCurseProjectile = null;

            firedCurseProjectile = Instantiate(projectile, player.transform.position + new Vector3(Random.Range(-7, 7), 0, 15), Quaternion.identity, null);
            firedCurseProjectile = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, null);
            firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
            firedCurseProjectile.transform.LookAt(player.transform.position);
            firedCurseProjectile.GetComponent<Projectile_V2>().projectileSpeed = 10;
            firedCurseProjectile = null;

            firedCurseProjectile = Instantiate(projectile, player.transform.position + new Vector3(Random.Range(-7, 7), 0, 15), Quaternion.identity, null);
            firedCurseProjectile = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, null);
            firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
            firedCurseProjectile.transform.LookAt(player.transform.position);
            firedCurseProjectile.GetComponent<Projectile_V2>().projectileSpeed = 10;
            firedCurseProjectile = null;
            light.gameObject.SetActive(false);
            yield return new WaitForSeconds(.25f);
            light.gameObject.SetActive(true);
            yield return new WaitForSeconds(.1f);
            light.gameObject.SetActive(false);
            yield return new WaitForSeconds(.15f);
            light.gameObject.SetActive(true);
        }
    }

    public void Laugh()
    {
        ownAnimator.Play("Laugh");
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[0], FindObjectOfType<SoundManager>().soundEffectsSlider.value);
    }

    //THE FOLLOWING IS LEGACY CODE, BASICALL JUST A BACKUP IN CASE WE COULDN'T GET NEW MASTER BONES UP AND RUNNING
    /*
    public GameObject[] eyes;
    public GameObject cursingHand;
    public GameObject curseFinger;
    public GameObject player;

    public GameObject[] waypoints;
    private int waypointIndex;

    public AudioClip[] MasterBonesSpecificSounds;

    private float lightningCooldown = 5;

    public Animator handAnimator;
    private float curseProjectileTimer = 7;
    public GameObject curseProjectile;

    public float laughTimer = 15;
    public Animator jawAnimator;

    void Update()
    {
        if (Time.timeScale > 0)
        {
            foreach (GameObject eyeball in eyes)
            {
                eyeball.transform.LookAt(player.transform);
            }
            cursingHand.transform.LookAt(player.transform.position + new Vector3(3.5f,0,0));

            MoveBetweenWaypoints();

            lightningCooldown -= Time.deltaTime;
            if (lightningCooldown <= 0)
            {
                StartCoroutine("Lightning");
                lightningCooldown = Random.Range(7, 11);
            }

            curseProjectileTimer -= Time.deltaTime;
            if (curseProjectileTimer <= 0)
            {
                handAnimator.SetBool("ReadyToCurse", true);
                StartCoroutine("FireCurse");
                curseProjectileTimer = Random.Range(3, 11);
            }

            laughTimer -= Time.deltaTime;
            if (laughTimer <= 0)
            {
                curseProjectileTimer = Random.Range(7, 15);
                lightningCooldown = Random.Range(9, 11);
                StartCoroutine("Laugh");
                laughTimer = Random.Range(15, 25);
            }
        }
    }

    private void MoveBetweenWaypoints()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, waypoints[waypointIndex].transform.position, 2 * Time.deltaTime);

        if (Vector3.Distance(gameObject.transform.position, waypoints[waypointIndex].transform.position) < .1f)
        {
            if (waypointIndex >= waypoints.Length - 1)
            {
                waypointIndex = 0;
            }
            else
            {
                waypointIndex++;
            }
        }
    }

    public IEnumerator Lightning()
    {
        if (FindObjectOfType<Light>())
        {
            GameObject light = FindObjectOfType<Light>().gameObject;
            gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[0]);
            GameObject firedCurseProjectile = Instantiate(curseProjectile, player.transform.position + new Vector3(Random.Range(-5, 5), 0,15), Quaternion.identity, null);
            firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
            firedCurseProjectile.GetComponent<Projectile_V2>().projectileSpeed = 7;
            firedCurseProjectile.transform.localScale = new Vector3(2, 2, 2);
            firedCurseProjectile.transform.LookAt(player.transform.position);
            firedCurseProjectile = Instantiate(curseProjectile, player.transform.position + new Vector3(Random.Range(-7, 7), 0, 15), Quaternion.identity, null);
            firedCurseProjectile.transform.localScale = new Vector3(Random.Range(1, 2), Random.Range(1, 2), Random.Range(1, 2));
            firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
            firedCurseProjectile.transform.LookAt(player.transform.position);
            firedCurseProjectile.GetComponent<Projectile_V2>().projectileSpeed = 10;
            firedCurseProjectile = Instantiate(curseProjectile, player.transform.position + new Vector3(Random.Range(-10, 10), 0, 15), Quaternion.identity, null);
            firedCurseProjectile.transform.localScale = new Vector3(Random.Range(1, 2), Random.Range(1, 2), Random.Range(1, 2));
            firedCurseProjectile.GetComponent<Projectile_V2>().projectileSpeed = 5;
            firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
            firedCurseProjectile.transform.LookAt(player.transform.position);
            light.gameObject.SetActive(false);
            yield return new WaitForSeconds(.25f);
            light.gameObject.SetActive(true);
            yield return new WaitForSeconds(.1f);
            light.gameObject.SetActive(false);
            yield return new WaitForSeconds(.15f);
            light.gameObject.SetActive(true);
        }
    }

    public IEnumerator FireCurse()
    {
        handAnimator.SetBool("ReadyToCurse", true);
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[1]);
        yield return new WaitForSeconds(1f);
        handAnimator.SetBool("ReadyToCurse", true);
        GameObject firedCurseProjectile = Instantiate(curseProjectile, curseFinger.transform.position, Quaternion.identity, null);
        firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
        firedCurseProjectile.transform.LookAt(player.transform.position);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[2]);
        yield return new WaitForSeconds(.1f);
        firedCurseProjectile = Instantiate(curseProjectile, curseFinger.transform.position, Quaternion.identity, null);
        firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
        firedCurseProjectile.transform.LookAt(player.transform.position);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[2]);
        yield return new WaitForSeconds(.1f);
        firedCurseProjectile = Instantiate(curseProjectile, curseFinger.transform.position, Quaternion.identity, null);
        firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
        firedCurseProjectile.transform.LookAt(player.transform.position);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[2]);
        yield return new WaitForSeconds(.1f);
        firedCurseProjectile = Instantiate(curseProjectile, curseFinger.transform.position, Quaternion.identity, null);
        firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
        firedCurseProjectile.transform.LookAt(player.transform.position);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[2]);
        yield return new WaitForSeconds(.1f);
        firedCurseProjectile = Instantiate(curseProjectile, curseFinger.transform.position, Quaternion.identity, null);
        firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
        firedCurseProjectile.transform.LookAt(player.transform.position);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[2]);
        yield return new WaitForSeconds(.1f);
        firedCurseProjectile = Instantiate(curseProjectile, curseFinger.transform.position, Quaternion.identity, null);
        firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
        firedCurseProjectile.transform.LookAt(player.transform.position);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[2]);
        yield return new WaitForSeconds(.1f);
        firedCurseProjectile = Instantiate(curseProjectile, curseFinger.transform.position, Quaternion.identity, null);
        firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
        firedCurseProjectile.transform.LookAt(player.transform.position);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[2]);
        yield return new WaitForSeconds(.1f);
        firedCurseProjectile = Instantiate(curseProjectile, curseFinger.transform.position, Quaternion.identity, null);
        firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
        firedCurseProjectile.transform.LookAt(player.transform.position);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[2]);
        handAnimator.SetBool("ReadyToCurse", false);
    }

    public IEnumerator Laugh()
    {
        jawAnimator.SetBool("IsLaughing", true);
        gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[3]);
        jawAnimator.gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(1f);
        foreach (GameObject eyeball in eyes)
        {
            GameObject firedCurseProjectile = Instantiate(curseProjectile, eyeball.transform.position, Quaternion.identity, null);
            firedCurseProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = true;
            firedCurseProjectile.transform.LookAt(player.transform.position);
            gameObject.GetComponent<AudioSource>().PlayOneShot(MasterBonesSpecificSounds[2]);
        }
        yield return new WaitForSeconds(4f);
        jawAnimator.SetBool("IsLaughing", false);
        jawAnimator.gameObject.GetComponent<BoxCollider>().enabled = true;
    }
    */
}
