using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin_Script : MonoBehaviour
{
    private void Update()
    {
        if (Time.timeScale > 0)
        {
            gameObject.transform.Rotate(0, 50 * Time.deltaTime, 0);
        }
    }
}
