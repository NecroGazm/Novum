using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duplicate_Pickup : MonoBehaviour
{
    private int speed = 2;
    private float lowerThreshold;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<Duplication_Script>() != null)
            {
                other.gameObject.GetComponent<Duplication_Script>().CreateDuplicate();
            }
            Destroy(gameObject);
        }
    }
}
