using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusEnemySpawn : MonoBehaviour
{
    
  
    public GameObject bonusEnemy =null;

    

    void Start()
    {
        Invoke("showGameObject", 15);
    }

    private void Update()
    {
        

    }

    void showGameObject()
    {
        bonusEnemy.SetActive(true);
    }

   
}
