using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Overshield_Script : MonoBehaviour
{
    private GameObject audioObject;
    private Vector3 shieldScale; // The maximum values for the shield scale size.

    public AudioClip reflectorSound;
    public AudioClip reflectorBreakSound;

    public void Awake()
    {
        audioObject = FindObjectOfType<SoundManager>().gameObject;
        shieldScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        gameObject.transform.localScale = new Vector3(0, 0, 0);

        gameObject.transform.parent.GetComponent<Collider>().enabled = false;
    }

    public void Update()
    {
        if (gameObject.activeSelf == true) // Shield status check
        {
            ActiveOvershieldStatus();
        }
    }

    public void ActiveOvershieldStatus()
    {
        if (gameObject.transform.localScale.x != shieldScale.x)
        {
            RegenerateOverShield();
        }

        RaycastHit[] objectsHit = Physics.SphereCastAll(gameObject.transform.position, gameObject.GetComponent<MeshRenderer>().bounds.extents.x, transform.forward);

        List<GameObject> bulletsNearby = new List<GameObject>();

        for (int i = 0; i < objectsHit.Length; i++)
        {
            if (objectsHit[i].collider.gameObject.GetComponent<Projectile_V2>())
            {
                if (objectsHit[i].collider.gameObject.GetComponent<Projectile_V2>().reflected == true && gameObject.activeSelf == true)
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
                    gameObject.GetComponent<AudioSource>().PlayOneShot(reflectorSound, audioObject.GetComponent<SoundManager>().soundEffectsSlider.value);
                }
            }
        }
    }

    public void EnableShield()
    {
        gameObject.SetActive(true);
    }

    public void DisableShield()
    {
        if (gameObject.transform.childCount > 0)
        {
                Destroy(gameObject.transform.GetChild(0).gameObject);
        }
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        GameObject audioObject = FindObjectOfType<SoundManager>().gameObject;
        gameObject.GetComponent<AudioSource>().PlayOneShot(reflectorBreakSound, audioObject.GetComponent<SoundManager>().soundEffectsSlider.value * .75f);
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        gameObject.transform.parent.GetComponent<Collider>().enabled = true;
        Destroy(gameObject, .25f);
    }

    

    public void RegenerateOverShield()
    {
        gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, shieldScale, .05f);
    }
}
