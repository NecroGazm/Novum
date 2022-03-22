using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossReflector : MonoBehaviour
{
    public float abilityCooldown = 10;
    public AudioClip reflectorSound;
    public AudioClip reflectorChargedSound;
    private bool reflectorCharged = true;

    void Update()
    {
        if (abilityCooldown > 0)
        {
            abilityCooldown -= Time.deltaTime;
        }
            if (reflectorCharged == true)
            {
                ReflectorAbilityUseage();
                abilityCooldown = 10;
            }
        if (reflectorCharged == false)
        {
            ReflectorCharged();
        
        }
    }
    public void PlayRecharge()
    {
        if (gameObject != null)
        {
            if (reflectorChargedSound != null)
            {
                if (gameObject.GetComponent<AudioSource>() != null)
                {
                    gameObject.GetComponent<AudioSource>().PlayOneShot(reflectorChargedSound);
                }
            }
        }
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
            gameObject.GetComponent<AudioSource>().PlayOneShot(reflectorSound);
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
                if (objectsHit[i].collider.gameObject.GetComponent<Projectile_V2>().isEnemyProjectile == false)
                {
                    objectsHit[i].collider.gameObject.transform.LookAt(gameObject.transform.position); // Makes the bullet face the player in its current position.
                    objectsHit[i].collider.gameObject.transform.Rotate(new Vector3(0, 180, 0)); // Flips the bullet back around.
                    objectsHit[i].collider.gameObject.GetComponent<Projectile_V2>().isEnemyProjectile = true; // Transforms the projectile into a player projectile.
                }
            }
        }

    }
}
