using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemyScript : MonoBehaviour
{
    public float TimeToDestroy = 10f;
    private void Start()
    {
        StartCoroutine(destroyWait());

    }
    IEnumerator destroyWait()
    {

        yield return new WaitForSeconds(TimeToDestroy);
        Destroy(gameObject);
    }

}
