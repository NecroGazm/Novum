using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{

    public GameObject powerupROF;
    public List<PowerUp> powerups;
    public Dictionary<PowerUp, float> activePowerups = new Dictionary<PowerUp, float>();
    public List<PowerUp> keys = new List<PowerUp>();

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleActivePowerups();
    }

        

    public void HandleActivePowerups()
    {
        bool changed = false;

        if(activePowerups.Count > 0)
        {
            foreach(PowerUp powerup in keys)
            {
                if (activePowerups[powerup] > 0)
                {
                    activePowerups[powerup] -= Time.deltaTime;
                }
                else
                {
                    changed = true;

                    activePowerups.Remove(powerup);

                    powerup.End();
                }
            }
        }

        if (changed)
        {
            keys = new List<PowerUp>(activePowerups.Keys);
        }
    }

    public void ActivatePowerUp(PowerUp powerup)
    {
        if(!activePowerups.ContainsKey(powerup))
        {
            powerup.Start();
            activePowerups.Add(powerup, powerup.duration);
        }
        else
        {
            activePowerups[powerup] += powerup.duration;
        }

        keys = new List<PowerUp>(activePowerups.Keys);
    }


    public GameObject SpawnPowerup (PowerUp powerup, Vector3 position)
    {
        GameObject powerupGameObject = Instantiate(powerupROF);


        var powerupBehavior = powerupGameObject.GetComponent<PowerUpBehavior>();

        powerupBehavior.controller = this;

        powerupBehavior.SetPowerup(powerup);

        powerupGameObject.transform.position = position;

        return powerupGameObject;
    }

   
}
