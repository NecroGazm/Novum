using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAction : MonoBehaviour
{
    [SerializeField]
    private PlayerFire playerFire;

    public void HighSpeedStartAction()
    {
        playerFire.timepassed *= 2;
    }


    public void HighSpeedEndAction()
    {
        playerFire.timepassed = 0.5f;
    }
}
