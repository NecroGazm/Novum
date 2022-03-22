using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits_Player : MonoBehaviour
{
    public GameObject projectileToSpawn;
    private float position;

    void Update()
    {
        if (Time.timeScale > 0)
        {
            position = Mathf.Clamp(position, -760f, 600);
        }

        if (Input.GetKey(KeyCode.A))
        {
            position -= 1000 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            position += 1000 * Time.deltaTime;
        }

        gameObject.transform.position = new Vector3(position + 1000, gameObject.transform.position.y, gameObject.transform.position.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //SPAWN PROJECTILE
            GameObject bulletFired = Instantiate(projectileToSpawn, gameObject.transform.position + new Vector3(0, 100, 0), Quaternion.identity, null);
            bulletFired.transform.Rotate(-90, 0, 0);
            bulletFired.transform.localScale = new Vector3(100, 100, 100);
            bulletFired.GetComponent<Projectile_V2>().projectileSpeed = 500;
        }
    }
}
