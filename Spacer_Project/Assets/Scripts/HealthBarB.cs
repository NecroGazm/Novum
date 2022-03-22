using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class HealthBarB : MonoBehaviour
{ // tutorial: https://www.youtube.com/watch?v=NE5cAlCRgzo
    public float defaultHealth;
    public float HP;
    public Image HealthB;
    BossHealth boss;
    // Start is called before the first frame update
    void Start()
    {
        HealthB = GetComponent<Image>();
        boss = FindObjectOfType<BossHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        HP = boss.HP;
        HealthB.fillAmount = HP / defaultHealth; 
    }
}
