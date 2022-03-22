using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    //tutorial: https://www.youtube.com/watch?v=TJCOC0gcU4k&t=18s
    public GameObject[] NavPoint;
    int current = 0;
    public float speed;
    float NearN = 1;
   // private BossAttack BA;


    private void Start()
    {
      //  BA = GameObject.FindObjectOfType<BossAttack>();
    }


    void Update()
    {
        if(Vector3.Distance(NavPoint[current].transform.position, transform.position) < NearN)
        {
            current++;
            

            if (current >= NavPoint.Length)
            {
                current = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, NavPoint[current].transform.position, Time.deltaTime * speed);
    }
}
