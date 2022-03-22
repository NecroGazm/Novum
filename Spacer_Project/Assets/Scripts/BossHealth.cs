using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    public float defaultHealth;
    public float HP;
    public bool IsHit;
    public Renderer Render;
    public GameObject YOUWINText;
    public Collider Collide;
    public LevelManager lm;
    public GameObject Boss;
    // Start is called before the first frame update
    void Start()
    {
        lm = GameObject.FindObjectOfType<LevelManager>();
        if (YOUWINText != null)
        {
            YOUWINText.SetActive(false);
        }
        if (Render != null)
        {
            Render.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <=0 )
        {
            if (Boss != null)
            {
                Destroy(Boss.gameObject);
            }
        }
    }

    public void DealDamage()
    {
        HP--; 
    }


  

  
}
