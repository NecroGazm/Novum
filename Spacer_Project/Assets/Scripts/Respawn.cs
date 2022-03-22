using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Respawn : MonoBehaviour
{
    // tutorial: https://docs.unity3d.com/Manual/CollidersOverview.html
   
    [HideInInspector]
    public Player player;
    public GameObject model;
    [HideInInspector]
    public Collider Collide;
    public GameObject ReadyText;
    public GameObject GoText;
    public GameObject RespawnP; 
    public Transform RespawnPoint;
    public Rigidbody RB; 
    public AudioSource DeathSound;
    public bool IsHit;
    private GameManager_V2 GMV2;
    public LevelManager lm;
    public float multiplier = 10f;
    public GameObject Reflect;
    public bool HasReflect;
    public GameObject Default;
    public GameObject Laser;
    public GameObject Explosives;
    public GameObject Spread;



    private void Start()
    {
        player = GetComponent<Player>();
        lm = GameObject.FindObjectOfType<LevelManager>();
        GMV2 = FindObjectOfType<GameManager_V2>();
        RB = GetComponent<Rigidbody>();
        Collide = GetComponent<Collider>();
        ReadyText.SetActive(false);
        GoText.SetActive(false);
        model = gameObject.transform.GetChild(0).gameObject;
        model.SetActive(true);
        Collide.enabled = true;
        player.enabled = true;
        if (HasReflect == true)
        {
            GetComponent<Reflector_Ability>().enabled = true;
        }

    }
     void OnCollisionEnter(Collision other)
    {
       IsHit = true;

        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Enemy1") || other.gameObject.CompareTag("Enemy_Bullet") || other.gameObject.CompareTag("BossBullet") || other.gameObject.CompareTag("Boss")|| other.gameObject.CompareTag("Blocker"))
        {
            if (gameObject.GetComponent<Duplication_Script>() != null)
            {
                gameObject.GetComponent<Duplication_Script>().RemoveDuplicate();
            }
            else
            {
                StartCoroutine(Die(1));
            }
        }

        if (other.gameObject.CompareTag("Reflect"))
        {
            if (GetComponent<Reflector_Ability>() != null)
            {
                GetComponent<Reflector_Ability>().enabled = true;
               Object.Destroy(Reflect);
                HasReflect = true; 
            }
        }

        if (other.gameObject.CompareTag("Web"))
        {
            StartCoroutine(Freeze(1));
        }

        if (other.gameObject.CompareTag("BossFire"))
        {
            StartCoroutine(Hot(1));
        }

    }

    private void OnCollisionExit(Collision collision)
    {
         IsHit = false;
    }

    //DeathSound.Play();
    public IEnumerator Die(float time)
    {
        lm.audiosource.PlayOneShot(lm.fxs[1]);
        GameObject exp = (GameObject)Instantiate(lm.explosionfx[2], player.gameObject.transform.position, Quaternion.identity) as GameObject; 
        player.enabled = false;
        model.SetActive(false);
        Collide.enabled = false;
       
        Debug.Log("Damage off");
        if (GetComponent<Weapon_Script>() != null)
        {
            gameObject.GetComponent<Weapon_Script>().RevertWeapon();
        }
        if (GetComponent<PlayerFire>() != null)
        {
            GetComponent<PlayerFire>().enabled = false;
        }
        if (GetComponent<HookShot>() != null)
        {
            GetComponent<HookShot>().enabled = false;
        }
        Destroy(exp, 2);
        GMV2.DealDeath();
        ReadyText.SetActive(true);
        yield return new WaitForSeconds(2);     
        Debug.Log("Life lost");
        transform.position = RespawnPoint.position; 
        yield return new WaitForSeconds(1);       
        Debug.Log("player moved");      
        ReadyText.SetActive(false);
        GoText.SetActive(true);
        yield return new WaitForSeconds(2);      
        GoText.SetActive(false);
        model.SetActive(true);
        player.enabled = true;
        
        if (GetComponent<PlayerFire>() != null)
        {
            GetComponent<PlayerFire>().enabled = true;
        }
        if (GetComponent<HookShot>() != null)
        {
            GetComponent<HookShot>().enabled = true;
        }
        yield return new WaitForSeconds(4);
        Collide.enabled = true;
        Debug.Log("Damage on");
        if (gameObject.GetComponent<Weapon_Script>() != null)
        {
            gameObject.GetComponent<Weapon_Script>().RevertWeapon();
        }
        Default.SetActive(true);
        Laser.SetActive(false);
        Explosives.SetActive(false);
        Spread.SetActive(false);
        
    }


   



    public IEnumerator Freeze(float time)
    {
        player.enabled = false;
        GetComponent<PlayerFire>().enabled = false;
        yield return new WaitForSeconds(2);
        player.enabled = true;
        GetComponent<PlayerFire>().enabled = true;
    }
    public IEnumerator Hot(float time)
    {
        Player stats = player.GetComponent<Player>();
        stats.speed = 22f; 
    
        yield return new WaitForSeconds(3);
         stats.speed = 13f;

    }

}
