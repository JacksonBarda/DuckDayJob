using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetSFXVolume : MonoBehaviour
{
    [SerializeField] private AudioSource ASFXSource1, ASFXSource2, ISFXSource, movementSource;
    [SerializeField] private Slider SFXSlider;

    public void SetVolume() 
    {
        float volume = SFXSlider.value;
        ASFXSource1.volume = volume;
        ASFXSource2.volume = volume;
        ISFXSource.volume = volume;
        movementSource.volume = volume;
    }
}
