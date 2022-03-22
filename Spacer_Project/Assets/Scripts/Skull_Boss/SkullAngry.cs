using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullAngry : MonoBehaviour
{
    public BossHealth BH;
    public BossMovement BM;
    public Skull_Attack SA;
   
    public GameObject Roar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (BH.HP <= 5)
        {
            Roar.SetActive(true);
            BM.speed = 10F;
            SA.NFire = 8F;
            SA.bSpeed = 10F;
        }
    }
}
