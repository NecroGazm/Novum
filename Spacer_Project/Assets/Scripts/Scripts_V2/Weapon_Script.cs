using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Script : MonoBehaviour
{
    public GameObject laserProjectile;
    public GameObject spreadProjectile;
    public GameObject explosiveProjectile;
    public LevelManager lm;
    public GameObject originalProjectileOfPlayer;
    public GameObject Laser;
    public GameObject Explosives;
    public GameObject Spread;
    public GameObject Default;
    private void Awake()
    {
        
        originalProjectileOfPlayer = gameObject.GetComponent<PlayerFire>().projectile.gameObject;
    }
   
    public void RevertWeapon()
    {
        gameObject.GetComponent<PlayerFire>().projectile = originalProjectileOfPlayer;
    }

    public void SetNewPlayerWeapon(int weaponToAdd)
    {
        switch (weaponToAdd)
        {
            case 1:
                {
                    if (laserProjectile != null)
                    {
                        // Laser
                        if (gameObject.GetComponent<PlayerFire>().projectile == laserProjectile)
                        {
                            if (FindObjectOfType<LevelManager>())
                            {
                                LevelManager lm = FindObjectOfType<LevelManager>();
                                lm.audiosource.PlayOneShot(lm.fxs[9]);
                                //foreach (AudioClip clip in lm.fxs)
                                //{
                                //    if (clip.name == "ErrorSound")
                                //    {
                                //        lm.audiosource.PlayOneShot(clip);
                                //    }
                                //}
                            }
                        }
                        
                        Laser.SetActive(true);
                        Explosives.SetActive(false);
                        Spread.SetActive(false);
                        Default.SetActive(false);
                        gameObject.GetComponent<PlayerFire>().projectile = laserProjectile;
                    }
                    return;
                }
            case 2:
                {
                    if (laserProjectile != null)
                    {
                        // Spread
                        if (gameObject.GetComponent<PlayerFire>().projectile == spreadProjectile)
                        {
                            if (FindObjectOfType<LevelManager>())
                            {
                                LevelManager lm = FindObjectOfType<LevelManager>();

                                 lm.audiosource.PlayOneShot(lm.fxs[6]);
                            }
                        }
                       
                        gameObject.GetComponent<PlayerFire>().projectile = spreadProjectile;
                        Laser.SetActive(false);
                        Explosives.SetActive(false);
                        Spread.SetActive(true);
                        Default.SetActive(false);
                    }
                    return;
                }
            case 3:
                {
                    if (laserProjectile != null)
                    {
                        // Explosive
                        if (gameObject.GetComponent<PlayerFire>().projectile == explosiveProjectile)
                        {
                            if (FindObjectOfType<LevelManager>())
                            {
                                LevelManager lm = FindObjectOfType<LevelManager>();

                                lm.audiosource.PlayOneShot(lm.fxs[10]);
                            }
                        }
                        
                        gameObject.GetComponent<PlayerFire>().projectile = explosiveProjectile;
                        Laser.SetActive(false);
                        Explosives.SetActive(true);
                        Spread.SetActive(false);
                        Default.SetActive(false);
                    }
                    return;
                }
        }
    }
}
