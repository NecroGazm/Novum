using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int finalscore;
    public Text finalscoretext;
   
   
    

    public void Start()
    {
        finalscore = PlayerPrefs.GetInt("scoreValue");
        finalscoretext.text = finalscore.ToString();
    }











}

