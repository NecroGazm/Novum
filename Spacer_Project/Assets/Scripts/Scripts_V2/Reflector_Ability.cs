using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector_Ability : MonoBehaviour
{
    public float abilityCooldown = 3;
    public AudioClip reflectorSound;
    public AudioClip reflectorChargedSound;
    private bool reflectorCharged = true;
    public GameObject Render;
    public GameObject reflectorLight;
    public float Speed = 15f;

    public GameObject reflectProjectile;

     void Start()
    {
        if (Render == null)
        {
            Render = gameObject.transform.GetChild(0).gameObject;
        }
    }



    void Update()
    {
        if (abilityCooldown > 0)
        {
            abilityCooldown -= Time.deltaTime;
        }
        else if (abilityCooldown <= 0)
        {
            if (reflectorLight != null)
            {
                if (reflectorLight.activeSelf == false)
                {
                    reflectorLight.SetActive(true);
                }

                if (Render.activeSelf == false)
                {
                    reflectorLight.SetActive(false);
                }

            }
            if (Input.GetKeyDown(KeyCode.F) && gameObject.transform.GetChild(0).gameObject.activeSelf == true && reflectorCharged == true)
            {
                ReflectorAbilityUseage();
                abilityCooldown = 3;
            }

            if (reflectorCharged == false)
            {
                ReflectorCharged();
                if (reflectorLight != null)
                {
                    reflectorLight.SetActive(true);
                }
                if (Render.activeSelf == false)
                {
                    reflectorLight.SetActive(false);
                }


            }
        }
    }

    public void PlayRecharge()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(reflectorChargedSound, FindObjectOfType<SoundManager>().soundEffectsFloat);
    }

    public void ReflectorCharged()
    {
        reflectorCharged = true;
    }

    public void ReflectorAbilityUseage()
    {
        RaycastHit[] objectsHit = Physics.SphereCastAll(gameObject.transform.position, 2, transform.forward);
        List<GameObject> bulletsNearby = new List<GameObject>();
        if (gameObject.GetComponent<AudioSource>())
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(reflectorSound, FindObjectOfType<SoundManager>().soundEffectsFloat);
        }
        if (reflectorLight != null)
        {
            reflectorLight.SetActive(false);
        }
        Invoke("PlayRecharge", 3);
        if (gameObject.GetComponent<ParticleSystem>())
        {
            gameObject.GetComponent<ParticleSystem>().Play();
        }
        for (int i = 0; i < objectsHit.Length; i++)
        {
            if (objectsHit[i].collider.gameObject.GetComponent<Projectile_V2>())
            {
                if (objectsHit[i].collider.gameObject.GetComponent<Projectile_V2>().isEnemyProjectile == true)
                {
                    if (objectsHit[i].collider.gameObject != null)
                    {
                        GameObject reflectedProjectile = Instantiate(reflectProjectile, objectsHit[i].collider.gameObject.transform.position, Quaternion.identity, null);
                        reflectedProjectile.transform.LookAt(gameObject.transform.position); // Makes the bullet face the player in its current position.
                        reflectedProjectile.transform.Rotate(new Vector3(0, 180, 0)); // Flips the bullet back around.
                        reflectedProjectile.GetComponent<Projectile_V2>().isEnemyProjectile = false; // Transforms the projectile into a player projectile.
                        reflectedProjectile.GetComponent<Projectile_V2>().reflected = true; // Transforms the projectile into a player projectile.
                        Destroy(objectsHit[i].collider.gameObject);
                        reflectedProjectile = null;
                    }
                }
            }
            else if (objectsHit[i].collider.gameObject.GetComponent<PinkFloaty>())
            {
                objectsHit[i].collider.gameObject.GetComponent<PinkFloaty>().transform.LookAt(gameObject.transform.position);
                objectsHit[i].collider.gameObject.GetComponent<PinkFloaty>().transform.Rotate(new Vector3(0, 180, 0));
                objectsHit[i].collider.gameObject.GetComponent<PinkFloaty>().isMoving = true;
            }

        }


    }
   
}