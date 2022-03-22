using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoostPower : MonoBehaviour
{
    public Player player;
    public Renderer Render;
    public Collider Collide;
    public Component PlayerFire;
    public Rigidbody RB;
    public LevelManager lm;
    public GameObject Speed;
    public GameObject NoDamage;
    public GameObject BulletBoost;
    public GameObject PickUpVA;
    public PowerUpTimer PA;

    public GameObject BulletSpeed;
    public GameObject PlayerSpeed;
    public GameObject IFrame;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        lm = GameObject.FindObjectOfType<LevelManager>();
        PA = GameObject.FindObjectOfType<PowerUpTimer>();
        RB = GetComponent<Rigidbody>();
        Render = GetComponent<Renderer>();
        Render.enabled = true;
        Collide.enabled = true;
        player.enabled = true;

        if (SceneManager.GetActiveScene().name == "PowerupLevel")
        {
            EasyMode();
        }
    }

    public IEnumerator Bullet(float time)
    {
        lm.audiosource.PlayOneShot(lm.fxs[12]);
        BulletSpeed.SetActive(true);
        PickUpVA.SetActive(true);
        GetComponent<PlayerFire>().enabled = false;
        
        GetComponent<FastFirespeed>().enabled = true;
        if (PA != null)
        {
            if (PickUpVA != null)
            {
                Instantiate(PA.BulletSpeed, PickUpVA.gameObject.transform.position, Quaternion.identity);
            }
        }
        yield return new WaitForSeconds(5);

        PickUpVA.SetActive(false);
        BulletSpeed.SetActive(false);
        PickupOver();
       
        GetComponent<PlayerFire>().enabled = true;
        GetComponent<FastFirespeed>().enabled = false;
    }

    public IEnumerator Invincible(float time)
    {
        lm.audiosource.PlayOneShot(lm.fxs[11]);
        IFrame.SetActive(true);
        PickUpVA.SetActive(true);
        if (PA != null)
        {
            if (PickUpVA != null)
            {
                Instantiate(PA.NoDamage, PickUpVA.gameObject.transform.position, Quaternion.identity);
            }
        }
      
        Collide.enabled = false;

        yield return new WaitForSeconds(5);
        PickUpVA.SetActive(false);
        IFrame.SetActive(false);
        PickupOver();
        Collide.enabled = true;
      

    }


    public IEnumerator SpeedB(float time)
    {
        lm.audiosource.PlayOneShot(lm.fxs[13]);
        PlayerSpeed.SetActive(true);
        PickUpVA.SetActive(true);
        if (PA != null)
        {
            if (PickUpVA != null)
            {
                Instantiate(PA.PlayerSpeed, PickUpVA.gameObject.transform.position, Quaternion.identity);
            }
        }
        Player stats = player.GetComponent<Player>();
       
        stats.speed = 20f;

        yield return new WaitForSeconds(3);
        PickUpVA.SetActive(false);
        PlayerSpeed.SetActive(false);
        PickupOver();
        stats.speed = 13f;
       

    }

   

    public void PickupOver()
    {
        lm.audiosource.PlayOneShot(lm.fxs[7]);
    }

    public void EasyMode()
    {
        GetComponent<Respawn>().enabled = false;
        GetComponent<PlayerFire>().enabled = false;
        GetComponent<FastFirespeed>().enabled = true;
        GetComponent<MeshCollider>().enabled = false;
        Player stats = player.GetComponent<Player>();
        stats.speed = 20f;
    }
}
