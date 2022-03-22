using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Script : MonoBehaviour
{
    [Header("If this is a destroyable valuable obstacle")]
    public bool breakable;
    public int scoreValueOnBreak;
    [Space]

    private float lowerThreshold = -15;

    void Update()
    {
        if (Time.timeScale > 0)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - Time.deltaTime * 3);

            if (gameObject.transform.position.z < lowerThreshold)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.GetComponent<Projectile_V2>() != null)
        {
            collision.collider.GetComponent<Projectile_V2>().spawnExplosion();
            if (breakable == true)
            {
                if (collision.collider.GetComponent<Projectile_V2>().isEnemyProjectile == false)
                {
                    GameObject.FindObjectOfType<GameManager_V2>().AddScore(scoreValueOnBreak);
                    Destroy(collision.collider.gameObject);
                    Destroy(gameObject);
                }
            }

            Destroy(collision.collider.gameObject);
        }
    }
}
