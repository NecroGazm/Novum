using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull_Attack : MonoBehaviour
{
    float CurrentTime = 0.0f;
    public GameObject bullet;
    public GameObject Sbullet;
    public float NFire = 3f;
    public Transform spawnPoint;
    public Transform spawnPoint2;
    // public bool IsAttack;
    public float bSpeed = 5f;
    public GameObject Player;
    public Transform TPlayer;
    public Collider Collide;
    public GameObject Rlight;
    // Update is called once per frame

    private void Update()
    {
        Invoke("NormalAttack", 8.0f);
        Invoke("SpecialAttack", 16.0f);
        InvokeRepeating("Attackable", 5.0f,15f);
        InvokeRepeating("NotAttackable", 2.0f, 20f);
    }


    public void NormalAttack()
    {
        CurrentTime += Time.deltaTime;
        if (CurrentTime > NFire)
        {
            NFire += CurrentTime;
            //Hey Elizabeth! I tweaked your boss code to spawn my second generation bullets and made absolutely sure they fire correctly, haha
            GameObject normalBullet = Instantiate(bullet, spawnPoint.position, Quaternion.identity);
            if (normalBullet != null)
            {
                if (normalBullet.GetComponent<Projectile_V2>() != null)
                {
                    normalBullet.GetComponent<Projectile_V2>().isEnemyProjectile = true; // This sets the projectile to be flagged as an enemy version so it doesn't harm the enemy that spawned it.
                }
            }
            normalBullet.transform.LookAt(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position);
            NFire -= CurrentTime;
            CurrentTime = 0.0f;
          
        }
    }

    public void SpecialAttack()
    {

        CurrentTime += Time.deltaTime;
        if (CurrentTime > NFire)
        {
            NFire += CurrentTime;
            Instantiate(Sbullet, spawnPoint2.position, Quaternion.identity);
            NFire -= CurrentTime;
            CurrentTime = 0.0f;
            
        }
    }
   public void Attackable()
    {
      Collide.enabled = true;
      Rlight.SetActive(true);
    }

    public void NotAttackable()
    {
        Collide.enabled = false;
        Rlight.SetActive(false);
    }
}
