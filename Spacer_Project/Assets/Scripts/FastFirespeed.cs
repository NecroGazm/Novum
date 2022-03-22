using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastFirespeed : MonoBehaviour
{
    public GameObject projectile;
    public Transform spawnpoint;
    private bool hasFired;
    public float timepassed = .5f;
    private LevelManager lm;

    void Start()
    {
        lm = GameObject.FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !hasFired)
        {
            GameObject projectileFired;
            projectileFired = Instantiate(projectile, spawnpoint.gameObject.transform.position, Quaternion.identity, null);
            projectileFired.tag = "Untagged";
            if (projectileFired.GetComponent<Projectile_V2>() != null)
            {
                projectileFired.GetComponent<Projectile_V2>().isEnemyProjectile = false;
            }
            if (projectileFired.GetComponent<Rigidbody>() != null)
            {
                projectileFired.GetComponent<Rigidbody>().velocity = spawnpoint.TransformDirection(Vector3.forward * 10f);
            }
            hasFired = true;
            lm.audiosource.PlayOneShot(lm.fxs[0]);
        }

        if (hasFired == true)
        {
            timepassed -= Time.deltaTime;

            if (timepassed <= 0f)
            {
                hasFired = false;
                timepassed = .3f;
            }
        }
    }
}

