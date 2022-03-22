using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet_ : MonoBehaviour
{
    public float bSpeed = 7f;

    public enum Targets
    {
        PLAYER
    }

    public Targets target;

    
    void Start()
    {
        Destroy(gameObject, 2f);
    }

   
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * bSpeed);
    }

    void OnTriggerEnter(Collider col)
    {
        if (target == Targets.PLAYER)
        {
            if (col.tag == "Player")
            {
                Destroy(gameObject);
            }
        }
    }

}
