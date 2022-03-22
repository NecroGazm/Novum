using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duplication_Script : MonoBehaviour
{
    public GameObject leftDuplicate;
    public GameObject rightDuplicate;

    public bool isDuplicateObject;

    public void CreateDuplicate()
    {
        if (isDuplicateObject == false)
        {
            if (leftDuplicate.gameObject.activeSelf == false && rightDuplicate.gameObject.activeSelf == false)
            {
                //Create left duplicate
                leftDuplicate.SetActive(true);
            }
            else if (leftDuplicate.gameObject.activeSelf == true && rightDuplicate.gameObject.activeSelf == false)
            {
                //Create right duplicate
                rightDuplicate.SetActive(true);
            }
            else if (leftDuplicate.gameObject.activeSelf == true && rightDuplicate.gameObject.activeSelf == true)
            {
                //Gain extra life
                FindObjectOfType<GameManager_V2>().GiveLife();
            }
        }
    }

    public void RemoveDuplicate()
    {
        if (isDuplicateObject == false)
        {
            if (leftDuplicate.gameObject.activeSelf == false && rightDuplicate.gameObject.activeSelf == false)
            {
                // Kill the player?
                FindObjectOfType<Respawn>().StartCoroutine("Die", 1);
            }
            else if (leftDuplicate.gameObject.activeSelf == true && rightDuplicate.gameObject.activeSelf == false)
            {
                //Destroy left duplicate
                leftDuplicate.SetActive(false);
            }
            else if (leftDuplicate.gameObject.activeSelf == true && rightDuplicate.gameObject.activeSelf == true)
            {
                //Destroy right duplicate
                rightDuplicate.SetActive(false);
            }
        }
        else
        {
            gameObject.transform.parent.GetComponent<Duplication_Script>().RemoveDuplicate();
        }
    }
}
