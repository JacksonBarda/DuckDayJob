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

        try
        { ASFXSource1.volume = volume; }
        catch (UnassignedReferenceException) { }

        try
        { ASFXSource2.volume = volume; }
        catch (UnassignedReferenceException) { }

        try
        { ISFXSource.volume = volume; }
        catch (UnassignedReferenceException) { }

        try
        { movementSource.volume = volume; }
        catch (UnassignedReferenceException) { }
    }
}
