using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{
    private TextWriterSingle textWriterSingle;

    public void AddWriter(Text uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
    {
        textWriterSingle = new TextWriterSingle(uiText, textToWrite, timePerCharacter, invisibleCharacters);
        
    }

    private void Update()
    {
        if (textWriterSingle != null)
        {
            textWriterSingle.Update();
        }
    }


    public class TextWriterSingle
    {
    private Text uiText;
    private string textToWrite;
    private float timePerCharacter;
    private float timer;
    private int characterIndex;
    private bool invisibleCharacters;
    public TextWriterSingle(Text uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
    { 
        this.uiText = uiText;
        this.textToWrite = textToWrite;
        this.timePerCharacter = timePerCharacter;
        this.invisibleCharacters = invisibleCharacters;
        characterIndex = 0;
    }
        
        public void Update()
        {
            if (uiText != null)
            {
                timer -= Time.deltaTime;
                while (timer <= 0f)
                {
                    //Display next character
                    timer += timePerCharacter;
                    characterIndex++;
                    string text = textToWrite.Substring(0, characterIndex);
                    if (invisibleCharacters)
                    {
                        text += "<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
                    }
                    uiText.text = text;

                    if (characterIndex >= textToWrite.Length)
                    {
                        //Entire string displayed
                        uiText = null;
                        return;
                    }
                }
            }
        }
    }


    

}
