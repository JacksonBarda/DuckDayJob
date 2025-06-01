using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEditor;
using Enums;
using UnityEngine.SceneManagement;

public enum SoundType
{
    Music,
    AmbientSFX,
    InteractableSFX,
    Footstep
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private SoundList[] soundList;
    [SerializeField]
    public AudioSource[] sourceList;
    [SerializeField]
    private SourcePositionList[] spList;
    public bool disableSounds = false;
    

    private string[] lobbyofficeAmbiance = { "ASFX_ClearingThroat", "ASFX_Cough", "ASFX_KeyboardTyping", "ASFX_MouseClicks" };

    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (SceneManager.GetSceneByName("MainScene").isLoaded)
        {
            StartLobbyOfficeAmbiance();
        }
        else if (SceneManager.GetSceneByName("StartScreen").isLoaded)
        {
            PlaySoundContinuous(Instance.sourceList[0], SoundType.Music, "LobbyOfficeMusic");
        }
    }

    public static void StartLobbyOfficeAmbiance()
    {
        if (!TaskManager.TMInstance.isNight)
        {
            Debug.Log("AudioManager.StartLobbyOfficeSounds()");
            Instance.StartCoroutine(LobbyOfficeCoroutine());
        }
    }

    private static IEnumerator LobbyOfficeCoroutine()
    {
        
        Debug.Log("AudioManager.LobbyOfficeCoroutine()");
        while (true)
        {
            float wait = UnityEngine.Random.Range(10, 15);
            yield return new WaitForSeconds(wait);

            int i = (int)UnityEngine.Random.Range(0, Instance.spList[0].Transforms.Length);
            int j = (int)UnityEngine.Random.Range(0, Instance.lobbyofficeAmbiance.Length);

            Instance.sourceList[1].transform.position = Instance.spList[0].Transforms[i].position; // move audio source to random position
            PlaySoundOnce(Instance.sourceList[1], SoundType.AmbientSFX, Instance.lobbyofficeAmbiance[j]);   // play random lobbyoffice ambiance sound

        }
    }

    public static void StartBreakRoomAmbiance()
    {
        Debug.Log("AudioManager.StartBreakRoomAmbiance()");
        Instance.sourceList[1].transform.position = Instance.spList[1].Transforms[0].position;
        Instance.sourceList[2].transform.position = Instance.spList[1].Transforms[1].position;
        PlaySoundContinuous(Instance.sourceList[1], SoundType.AmbientSFX, "ASFX_ClockTicking");
        PlaySoundContinuous(Instance.sourceList[2], SoundType.AmbientSFX, "ASFX_VendingMachineHum");
    }

    public static void StartBathroomAmbiance()
    {
        if (!TaskManager.TMInstance.isNight && TaskManager.TMInstance.day < 5)
        {
            Debug.Log("AudioManager.StartBathroomAmbiance()");
            Instance.sourceList[1].transform.position = Instance.spList[2].Transforms[0].position;
            Instance.sourceList[2].transform.position = Instance.spList[2].Transforms[0].position;
            PlaySoundContinuous(Instance.sourceList[1], SoundType.AmbientSFX, "ASFX_CaseohStream");
            Instance.StartCoroutine(BathroomCoroutine());
        }
    }

    private static IEnumerator BathroomCoroutine()
    {
        Debug.Log("AudioManager.BathroomCoroutine()");

        while (true)
        {
            float wait = UnityEngine.Random.Range(20, 50);
            yield return new WaitForSeconds(wait);

            PlaySoundOnce(Instance.sourceList[2], SoundType.AmbientSFX, "ASFX_ToiletFlush");
        }
    }

    public static void StartBossRoomAmbiance()
    {
        PlaySoundContinuous(Instance.sourceList[0], SoundType.Music, "ElonOfficeMusic");
    }

    public static void PlaySoundOnce(AudioSource source, SoundType sound, string soundName)
    {
        Debug.Log("AudioManager.PlaySoundOnce(): " + soundName);

        AudioClip clip = GetAudioClip(sound, soundName);

        source.PlayOneShot(clip);
    }

    public static void PlaySoundContinuous(AudioSource source, SoundType sound, string soundName)
    {
        Debug.Log("AudioManager.PlaySoundContinuous(): " + soundName);

        AudioClip clip = GetAudioClip(sound, soundName);

        source.clip = clip;
        source.Play();
    }

    public static void StopSounds()
    {
        foreach (AudioSource source in Instance.sourceList)
        {
            source.Stop();
        }
        Instance.StopAllCoroutines();
    }

    //public static IEnumerator FadeOutSounds()
    //{
    //    Instance.audioSource.vol
    //}

    private static AudioClip GetAudioClip(SoundType sound, string soundName)
    {
        if (Instance.disableSounds)
        {
            Debug.LogError("AudioManager.GetAudioClip(): Sounds are disabled");
            return null;
        }

        else
        {
            try
            {
                AudioClip temp = Array.Find(Instance.soundList[(int)sound].Sounds, element => element.name == soundName);
                if (temp == null)
                {
                    Debug.LogError("AudioManager.GetAudioClip(): Audio clip was not found");
                }
                return temp;
            }
            catch (System.NullReferenceException)
            {
                Debug.LogError("AudioManager.GetAudioClip(): Audio clip was null");
                return null;
            }
        }
    }

    public static void PlayRoomSounds(Locations room)
    {
        if ((room == Locations.LOBBY || room == Locations.OFFICE)) StartLobbyOfficeAmbiance();
        else if (room == Locations.BREAKROOM) StartBreakRoomAmbiance();
        else if (room == Locations.BATHROOM) StartBathroomAmbiance();
        else if (room == Locations.BOSSROOM) StartBossRoomAmbiance();
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif
}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    public string name;
    [SerializeField]
    private AudioClip[] sounds;
}

[Serializable]
public struct SourcePositionList
{
    public Transform[] Transforms { get => transforms; }
    public string name;
    [SerializeField]
    private Transform[] transforms;
}