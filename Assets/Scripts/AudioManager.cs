using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEditor;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSounds, sfxSounds, dialogueSounds;
    public AudioSource MusicSource, sfxSource, dialogueSource;
    private string duckname;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayMusic("Lobby");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x=> x.audioname == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            MusicSource.clip = s.aclip;
            MusicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.audioname == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            sfxSource.PlayOneShot(s.aclip);

        }
    }

    public void PlayDialogue(string name)
    {
        Sound s = Array.Find(dialogueSounds, x => x.audioname == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            
            dialogueSource.PlayOneShot(s.aclip);
              
        }
           
    }
}
