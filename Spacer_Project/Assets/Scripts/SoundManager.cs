using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    //Author: Elizabeth Cherry
    //Tutorial:https://www.youtube.com/watch?v=9ROolmPSC70 

    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string BackgroundPref = "BGMP";
    private static readonly string SoundEffectsPref = "SFXP";
    private int firstPlayInt;
    public Slider backgroundSlider;
    public Slider soundEffectsSlider;
    [HideInInspector]
    public float backgroundFloat;
    [HideInInspector]
    public float soundEffectsFloat;
    public AudioSource[] backgroundAudio;
    public AudioSource[] soundEffectsAudio;

    public void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0)
        {
            backgroundFloat = .25f;
            soundEffectsFloat = .25f;
            backgroundSlider.value = backgroundFloat;
            soundEffectsSlider.value = soundEffectsFloat;
            PlayerPrefs.SetFloat(BackgroundPref, backgroundFloat);
            PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else
        {
            backgroundSlider.value = PlayerPrefs.GetFloat(BackgroundPref);
            soundEffectsSlider.value = PlayerPrefs.GetFloat(SoundEffectsPref);
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(BackgroundPref, backgroundSlider.value);
        PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsSlider.value);
    }

    private void OnApplicationFocus(bool inFocus)
    {
        if (!inFocus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        for (int i = 0; i < backgroundAudio.Length; i++)
        {
             backgroundAudio[i].volume = backgroundSlider.value;
        }
       
        if (soundEffectsAudio[0].isPlaying == true)
        {
            soundEffectsAudio[0].Stop();
        }
        for (int i = 0; i < soundEffectsAudio.Length; i++)
        {
            soundEffectsAudio[i].volume = soundEffectsSlider.value;
        }
        soundEffectsAudio[0].Play();
    }

    void Awake()
    {
        ContinueSound();
    }

    // Update is called once per frame
    public void ContinueSound()
    {
        backgroundFloat = PlayerPrefs.GetFloat(BackgroundPref);
        soundEffectsFloat = PlayerPrefs.GetFloat(SoundEffectsPref);
        for (int i = 0; i < backgroundAudio.Length; i++)
        {
            backgroundAudio[i].volume = backgroundFloat;
        }
        

        for (int i = 0; i < soundEffectsAudio.Length; i++)
        {
            soundEffectsAudio[i].volume = soundEffectsFloat;
        }
    }
}

