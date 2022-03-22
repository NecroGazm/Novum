using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float bSpeed = 8f;

    void Start()
    {
        Destroy(gameObject, 5f);   
    }


    void Update()
    {
        if (Time.timeScale > 0)
        {
            transform.Translate(-Vector3.forward * Time.deltaTime * bSpeed);
        }
    }

  

    void OnCollisionEnter(Collision other)
    {
        

        if (other.collider.tag == "Player")
        {
            Debug.Log("PlayerHit!");
           
            Destroy(gameObject);

        }

        if (other.collider.tag == "Blocker")
        {
            Debug.Log("Bullet avoided!");
           
            Destroy(gameObject);
           
        }

       
    }

}
