using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits_Block : MonoBehaviour
{
    void Update()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 250 * Time.deltaTime, gameObject.transform.position.z);

        if (gameObject.transform.position.y <= -500)
        {
            gameObject.transform.position = gameObject.transform.parent.transform.position;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Projectile_V2>() != null)
        {
            gameObject.GetComponentInChildren<Text>().color = new Color32((byte)Random.Range(50, 255), (byte)Random.Range(50, 255), (byte)Random.Range(50, 255), 255);
            Destroy(other.gameObject);
            Destroy(gameObject.GetComponent<BoxCollider>());
        }
    }
}
