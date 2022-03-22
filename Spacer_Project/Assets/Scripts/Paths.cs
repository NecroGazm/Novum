using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Paths : MonoBehaviour
{
    public int overload;
    public Color pathColor = Color.green;

    Transform[] objArray;
    public bool seePath;

    public List<Transform> pathObjlist = new List<Transform>();

    public List<Vector3> bezierObjectList = new List<Vector3>();

    




    [Range(1,20)]public int lineDensity = 1;




    // Start is called before the first frame update
    void Start()
    {
        CreatePath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void OnDrawGizmos()  //This is now able to be turned On/Off in inspectore with the seePath bool;
    {
        if (seePath)
        {




            Gizmos.color = pathColor;
            objArray = GetComponentsInChildren<Transform>();
            //Clear list due to updates to keep the list from constantly updating/cycling
            pathObjlist.Clear();

            foreach (Transform obj in objArray)
            {
                if (obj != this.transform) //if the pathObjlist doesn't have objects for the path, create one.
                {

                    pathObjlist.Add(obj);

                }



            }

            //generate the objects on the path
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
            //Check for even/uneven numbers on list for Overload

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

            //Curved Path behaviour begins
            bezierObjectList.Clear();
            Vector3 lineStart = pathObjlist[0].position;

            for (int e = 0; e < pathObjlist.Count - overload; e += 2)
            {
                for (int j = 0; j <= lineDensity; j++)
                {
                    Vector3 lineEnd = GetPoint(pathObjlist[e].position, pathObjlist[e + 1].position, pathObjlist[e + 2].position, j / (float)lineDensity);



                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(lineStart, lineEnd);

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(lineStart, 0.3f);

                    lineStart = lineEnd;

                    bezierObjectList.Add(lineStart);
                }
            }
        }

        else
        {
            //pathObjlist.Clear();
            //bezierObjectList.Clear();
        }
    }

    Vector3 GetPoint(Vector3 p0,Vector3 p1,Vector3 p2,float t)
    {
        return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
        
    }

    void CreatePath()
    {
        objArray = GetComponentsInChildren<Transform>();
        //Clear list due to updates to keep the list from constantly updating/cycling
        pathObjlist.Clear();

        foreach (Transform obj in objArray)
        {
            if (obj != this.transform) //if the pathObjlist doesn't have objects for the path, create one.
            {

                pathObjlist.Add(obj);

            }



        }

        
        
        //Check for even/uneven numbers on list for Overload

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

        //Curved Path behaviour begins
        bezierObjectList.Clear();

        Vector3 lineStart = pathObjlist[0].position;

        for (int e = 0; e < pathObjlist.Count - overload; e += 2)
        {
            for (int j = 0; j <= lineDensity; j++)
            {
                Vector3 lineEnd = GetPoint(pathObjlist[e].position, pathObjlist[e + 1].position, pathObjlist[e + 2].position, j / (float)lineDensity);



                lineStart = lineEnd;

                bezierObjectList.Add(lineStart);
            }
        }
    }
}

