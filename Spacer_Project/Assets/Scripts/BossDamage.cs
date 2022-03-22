using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    public bool IsHit;
    public Renderer Render;
    public Collider Collide;
    private BossHealth BH;
    public LevelManager lm;
        public GameObject Boss;
    // Start is called before the first frame update
    void Start()
    {
        lm = GameObject.FindObjectOfType<LevelManager>(); 
        BH = FindObjectOfType<BossHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision Other)
    {
        IsHit = true;

        if (Other.gameObject.CompareTag("Projectile"))
        {
            StartCoroutine(Hit(1));
        }


    }

    private void OnCollisionExit(Collision collision)
    {
        IsHit = false;
    }


    public IEnumerator Hit(float time)
    {
        lm.audiosource.PlayOneShot(lm.fxs[1]);
        GameObject exp = (GameObject)Instantiate(lm.explosionfx[1], Boss.gameObject.transform.position, Quaternion.identity) as GameObject;
        Collide.enabled = false;
        BH.DealDamage();
        yield return new WaitForSeconds(1);
        Collide.enabled = true;
    }
}
