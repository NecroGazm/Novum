using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public float defaultHealth;
    public float HP;
    
    public Renderer Render;

    public LevelManager lm;
    public GameObject Barrier;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Destroy(Barrier.gameObject);
        }
    }

    public void ADealDamage()
    {
        HP--;
    }

}
