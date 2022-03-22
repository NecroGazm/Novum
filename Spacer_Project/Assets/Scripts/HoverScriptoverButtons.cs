using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverScriptoverButtons : MonoBehaviour
{
    public AudioSource button;
    public AudioClip AC_Hover;
  


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void HoverSound()
    {
        if (button != null)
        {
            button.PlayOneShot(AC_Hover);
        }
    }

    
}
