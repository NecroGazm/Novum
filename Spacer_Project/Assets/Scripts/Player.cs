using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10f;
    Rigidbody r;
    private GameObject myplayer;
    public Vector3 position;
    void Awake()
    {
        r = GetComponent<Rigidbody>();
        myplayer = GameObject.Find("Player");

    }

    void Start()
    {
        
    }

    public void OnEnable()
    {
        position = new Vector3(0.8f, -67f, -14.45f);
    }
    
    void Update()
    {
        position += new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0f, 0f);
        transform.position = new Vector3(Mathf.Clamp(position.x, -5.2f, 5.79f), -67f, -14.45f);
        

        if(position.x < -5.2f)
        {
           position.x = -5.2f;
        }
        
        if (position.x > 5.79f)
        {
            position.x = 5.79f;
        }
    }
}
