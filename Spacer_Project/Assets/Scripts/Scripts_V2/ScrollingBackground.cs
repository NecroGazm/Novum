using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float speedmodifier;

    private void Awake()
    {
        if (speedmodifier == 0)
        {
            speedmodifier = 1;
        }
    }
    void Update()
    {
        Vector2 textureOffset = new Vector2(0, Time.deltaTime * -.6f * speedmodifier);
        gameObject.GetComponent<Renderer>().material.mainTextureOffset = gameObject.GetComponent<Renderer>().material.mainTextureOffset + textureOffset;
    }
}
