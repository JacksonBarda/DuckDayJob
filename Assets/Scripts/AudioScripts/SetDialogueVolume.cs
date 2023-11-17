using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetDialogueVolume : MonoBehaviour
{
    [SerializeField] private AudioSource DialogueSource;
    [SerializeField] private Slider DialogueSlider;

    public void SetVolume() 
    {
        float volume = DialogueSlider.value;
        DialogueSource.volume = volume;
    }
}
