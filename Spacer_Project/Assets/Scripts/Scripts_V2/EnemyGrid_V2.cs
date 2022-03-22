using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrid_V2 : MonoBehaviour
{
    [HideInInspector]
    public List<EnemyGridPositionObject_V2> gridPositions = new List<EnemyGridPositionObject_V2>(); // The positions in the grid that can be occupied.

    public List<GameObject> divePatterns = new List<GameObject>(); // What patterns are available to be utilized for enemies.

    private Vector3 initalStartingPoint;
    private bool movingLeft;
    
 
    public float timeBetweenDives = 5;
    public float ogDiveTime;

    private bool firstEnemyHasEntered;

    private void Awake()
    {
        ogDiveTime = timeBetweenDives;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).GetComponent<EnemyGridPositionObject_V2>() != null)
            {
                gridPositions.Add(gameObject.transform.GetChild(i).gameObject.GetComponent<EnemyGridPositionObject_V2>());
               // Debug.Log("Added to grid: " + gameObject.transform.GetChild(i).gameObject.name);
            }
        }

        initalStartingPoint = gameObject.transform.position;
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            SlideBackAndForth();

            if (firstEnemyHasEntered == true)
            {
                timeBetweenDives -= Time.deltaTime;

                if (timeBetweenDives <= 0)
                {
                    SendDive();
                }
            }
        }
    }

    private void SlideBackAndForth()
    {
        if (movingLeft == true)
        {
            if (Vector3.Distance(gameObject.transform.position, initalStartingPoint + new Vector3(-1, 0, 0)) < .01f)
            {
                movingLeft = false;
            }
            transform.position = Vector3.MoveTowards(transform.position, initalStartingPoint + new Vector3(-1,0,0), 1 * Time.deltaTime);
        }
        else
        {
            if (Vector3.Distance(gameObject.transform.position, initalStartingPoint + new Vector3(+1, 0, 0)) < .01f)
            {
                movingLeft = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, initalStartingPoint + new Vector3(+1, 0, 0), 1 * Time.deltaTime);
        }
    }

    public (bool, int) CheckForOpenPosition()
    {
        if (firstEnemyHasEntered == false)
        {
            firstEnemyHasEntered = true;
        }

        if (gridPositions.Count > 0)
        {
            for (int i = 0; i < gridPositions.Count; i++)
            {
                if (gridPositions[i].residentEnemy == null)
                {
                    //Debug.Log("Found an empty position");
                    return (true, i);
                }
            }

            return (false, -1);
        }
        else
        {
            return (false, -1);
        }
    }

    public void SendDive() // This is pretty hard logic to follow, so if you're confused, follow the comments.
    {
        int numberOfFoesToSend = Random.Range(3, 8); // Chooses a number of foes to dive between the two ints
        List<GameObject> validPositionsToSend = new List<GameObject>(); // This will be what positions CAN be sent to dive (have an enemy in their slot)
        List<GameObject> pickedPositions = new List<GameObject>(); // This will be the actual list of volenteers sent to dive.
        int patternToPick = Random.Range(0, divePatterns.Count);

        for (int i = 0; i < gridPositions.Count; i++) // This runs through the whole list of grid positions to see if they have someone in them.
        {
            if (gridPositions[i].residentEnemy != null) // If they have an enemy in their slot...
            {
                validPositionsToSend.Add(gridPositions[i].residentEnemy); // Add said enemy to the valid list.
            }
        }
        if (validPositionsToSend.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < numberOfFoesToSend; i++) // Now this is where enemies are picked from the valid list.
        {
            int IDtoPick = Random.Range(0, validPositionsToSend.Count); // This picks a random ID between 0 and the amount of valid positions.
            if (validPositionsToSend[IDtoPick] != null)
            {
                if (pickedPositions.Count == 0)
                {
                    pickedPositions.Add(validPositionsToSend[IDtoPick]); // Add the enemy to the picked list
                }
                else if (!pickedPositions.Contains(validPositionsToSend[IDtoPick])) // If the picked list doesn't already contain an enemy...
                {
                    pickedPositions.Add(validPositionsToSend[IDtoPick]); // Add the enemy to the picked list
                }
            }
        }

        for (int i = 0; i < pickedPositions.Count; i++)
        {
            if (pickedPositions[i].gameObject.GetComponent<EnemyBehavior_V2>().idle == true)
            {
                StartCoroutine(SendDiver(pickedPositions[i].gameObject, divePatterns[patternToPick].gameObject, 1f));
            }
        }

        timeBetweenDives = ogDiveTime + 3;
    }


    IEnumerator SendDiver(GameObject unitToSend, GameObject pathToTake, float timeToWaitUntilNextDive)
    {
        yield return new WaitForSeconds(Random.Range(0.1f, .25f));

        if (unitToSend != null)
        {
            if (unitToSend.GetComponent<EnemyBehavior_V2>().idle == true)
            {
                for (int i = 0; i < pathToTake.transform.childCount; i++)
                {
                    unitToSend.GetComponent<EnemyBehavior_V2>().divePath.Add(pathToTake.transform.GetChild(i).gameObject.transform.position);
                }
                unitToSend.GetComponent<EnemyBehavior_V2>().divePath.Add(unitToSend.GetComponent<EnemyBehavior_V2>().gridPosition.transform.position);
                unitToSend.transform.parent = null;
                unitToSend.GetComponent<EnemyBehavior_V2>().idle = false;
                yield return new WaitForSeconds(timeToWaitUntilNextDive);
            }
        }
    }
}
