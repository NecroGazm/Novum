using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    float CurrentTime = 0.0f ;
    public GameObject bullet;
    public GameObject Sbullet;
    public float NFire = 3f;
    public Transform spawnPoint;
    public Transform spawnPoint2;
   // public bool IsAttack;
    public float bSpeed = 5f;
    public GameObject Player;
    public Transform TPlayer;

    // Update is called once per frame

    private void Update()
    {
       Invoke("NormalAttack", 8.0f);
       Invoke("SpecialAttack", 16.0f);
       
    }
    

    public void NormalAttack()
    {
        CurrentTime += Time.deltaTime;
        if(CurrentTime > NFire)
        {
            NFire += CurrentTime;
            //Hey Elizebeth! I tweaked your boss code to spawn my second generation bullets and made absolutely sure they fire correctly, haha
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
        if(CurrentTime > NFire)
        {
            NFire += CurrentTime;
            Instantiate(Sbullet, spawnPoint2.position, Quaternion.identity);
     NFire -= CurrentTime;
            CurrentTime = 0.0f;
        }
    }

}

