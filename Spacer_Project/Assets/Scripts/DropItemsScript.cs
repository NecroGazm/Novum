using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemsScript : MonoBehaviour
{
    public List<GameObject> itemsToDrop = new List<GameObject>();
    [Tooltip("How likely, from 0 to 100, a drop is likely to happen for this enemy.")]
    public int percentChance;

    private void Awake()
    {
        if (percentChance <= 0)
        {
            Destroy(this);
        }
    }

    public void CheckIfDroppingItem()
    {
        if (percentChance > 0 && itemsToDrop.Count > 0)
        {
            int chanceInt = Random.Range(0, 100);
            if (chanceInt <= percentChance)
            {
                DropItem();
            }
            else
            {
                Destroy(this);
            }
        }
    }

    public void DropItem()
    {
        int itemToDrop = Random.Range(0, itemsToDrop.Count);
        Instantiate(itemsToDrop[itemToDrop], gameObject.transform.position, Quaternion.identity, null);
        Destroy(this);
    }
}
