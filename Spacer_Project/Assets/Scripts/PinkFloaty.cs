using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkFloaty : MonoBehaviour
{
    public float bSpeed = 15f;
    public Collider collide;
    public ArchHealth AH;
    public LevelManager lm;
    public float Speed = 15;

    public bool isMoving;

    void Start()
    {
        AH = GameObject.FindObjectOfType<ArchHealth>();
        lm = GameObject.FindObjectOfType<LevelManager>();
    }

    private void FixedUpdate()
    {
        SpawnManager_V2.readyToBegin = false;
    }

    private void Update()
    {
        if (isMoving == true && Time.deltaTime > 0)
        {
            gameObject.transform.position += gameObject.transform.forward * 3 * Time.deltaTime;
        }
        if (isMoving == true && gameObject.GetComponent<MeshRenderer>().enabled == false)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }


    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Blocker"))
        {
            bounce();
        }

        if (other.gameObject.CompareTag("Barrier"))
        {
            BounceAttack(other.gameObject);
        }

        if (other.gameObject.tag == "Projectile")
        {
            Destroy(other.gameObject);
        }
    }

    public void bounce()
    {
        collide.gameObject.transform.Rotate(new Vector3(0, 180, 0));
    }

    public void BounceAttack(GameObject objectToDestroy)
    {
            AH.ADealDamage();
            lm.audiosource.PlayOneShot(lm.fxs[3]);
        Destroy(objectToDestroy);
        Destroy(gameObject);
    }
}
