using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetMusicVolume : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Slider musicSlider;

    public void SetVolume() 
    {
        float volume = musicSlider.value;
        musicSource.volume = volume;
    }
}
