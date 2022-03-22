using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGridPositionObject_V2 : MonoBehaviour
{
    public GameObject residentEnemy;

    private void Awake()
    {
        if (gameObject.GetComponent<MeshRenderer>().enabled)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
