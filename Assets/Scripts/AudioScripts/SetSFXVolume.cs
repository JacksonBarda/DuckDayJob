using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetSFXVolume : MonoBehaviour
{
    [SerializeField] private AudioSource SFXSource,movementSource;
    [SerializeField] private Slider SFXSlider;

    public void SetVolume() 
    {
        float volume = SFXSlider.value;
        SFXSource.volume = volume;
        movementSource.volume = volume;
    }
}
