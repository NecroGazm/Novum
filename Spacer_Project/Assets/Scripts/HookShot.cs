using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShot : MonoBehaviour
{
    public GameObject hookToFire;
    public Hook firedHooksScript;
    public LevelManager lm;

     void Start()
    {
        lm = GameObject.FindObjectOfType<LevelManager>();
    }




    private void Update()
    {
        if (Time.timeScale > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (firedHooksScript == null)
                {
                    GameObject spawnedHook = Instantiate(hookToFire, gameObject.transform.position, Quaternion.identity, null);
                    lm.audiosource.PlayOneShot(lm.fxs[4]);
                    firedHooksScript = spawnedHook.GetComponent<Hook>();
                    firedHooksScript.player = gameObject;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (firedHooksScript.retractHook == false)
                {
                    lm.audiosource.PlayOneShot(lm.fxs[4]);
                    firedHooksScript.retractHook = true;
                }
            }
        }
    }

    
}
