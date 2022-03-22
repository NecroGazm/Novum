using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Path_V2 : MonoBehaviour
{
    public int overload;
    public Color pathColor = Color.green;

    Transform[] objArray;
    public bool seePath;

    public List<Transform> pathObjlist = new List<Transform>();

    [Range(1, 20)] public int lineDensity = 1;


    void OnDrawGizmos()  //This is now able to be turned On/Off in inspectore with the seePath bool;
    {
        if (seePath)
        {
            Gizmos.color = pathColor;
            objArray = GetComponentsInChildren<Transform>();
            pathObjlist.Clear(); //Clear list due to updates to keep the list from constantly updating/cycling

            foreach (Transform obj in objArray)
            {
                if (obj != this.transform) //if the pathObjlist doesn't have objects for the path, create one.
                {

                    pathObjlist.Add(obj);

                }
            }

            for (int e = 0; e < pathObjlist.Count; e++)
            {

                Vector3 position = pathObjlist[e].position;

                if (e > 0)
                {
                    Vector3 previous = pathObjlist[e - 1].position;            //Can only do this when e = 0, this prevents out of range errors.
                    Gizmos.DrawLine(previous, position);
                    Gizmos.DrawWireSphere(position, 0.5f);                      //Indicator for seeing the path.
                }

            }

            if (pathObjlist.Count % 2 == 0)
            {
                pathObjlist.Add(pathObjlist[pathObjlist.Count - 1]);
                overload = 2;
            }
            else
            {
                pathObjlist.Add(pathObjlist[pathObjlist.Count - 1]);
                pathObjlist.Add(pathObjlist[pathObjlist.Count - 1]);
                overload = 3;
            }
        }
    }
}