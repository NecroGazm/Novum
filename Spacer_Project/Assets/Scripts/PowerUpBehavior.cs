using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehavior : MonoBehaviour
{

    public PowerUpController controller;

    [SerializeField]
    private PowerUp powerup;

    private Renderer renderer_;

    private Transform transform_;


    private void Awake()
    {
        transform_ = transform;
    }

    private void OntriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            ActivatePowerup();
            gameObject.SetActive(false);
        }
    }


    private void ActivatePowerup()
    {
        controller.ActivatePowerUp(powerup);
    }

    public void SetPowerup (PowerUp powerup)
    {
        this.powerup = powerup;
        gameObject.name = powerup.name;
    }


   
}
