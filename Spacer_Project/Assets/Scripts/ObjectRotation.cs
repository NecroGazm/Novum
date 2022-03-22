using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    //tutorial:https://www.youtube.com/watch?v=q-BY4G5Rkoo
    // Start is called before the first frame update
    public float rSpeed =1f;

    private void OnMouseDrag()
    {
        float XaxisRotation = Input.GetAxis("Mouse X") * rSpeed;
        float YaxisRotation = Input.GetAxis("Mouse Y") * rSpeed;

        transform.Rotate(Vector3.down, XaxisRotation, Space.World);
        transform.Rotate(Vector3.right, YaxisRotation, Space.World);
    }

    public void Reset()
    {
        transform.rotation = Quaternion.identity; 
    }
}
