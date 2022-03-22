using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFire : MonoBehaviour
{
    public Rigidbody projectile;
    public Transform spawnpoint;
    private bool hasFired;
    public float timepassed = 1f;
    private LevelManager lm;

    void Start()
    {
        lm = GameObject.FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !hasFired)
        {
            Rigidbody clone;
            clone = (Rigidbody)Instantiate(projectile, spawnpoint.position, Quaternion.identity);
            clone.velocity = spawnpoint.TransformDirection(Vector3.forward * 10f);
            hasFired = true;
            lm.audiosource.PlayOneShot(lm.fxs[0]);
        }

        if (hasFired == true)
        {
            timepassed -= Time.deltaTime;

            if (timepassed <= 0f)
            {
                hasFired = false;
                timepassed = 1f;
            }
        }
    }
}
