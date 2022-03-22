using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputManager : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<InputField>().onEndEdit.AddListener(displayText);
    }

    private void displayText (string textInField)
    {
        print(textInField);
    }


}
