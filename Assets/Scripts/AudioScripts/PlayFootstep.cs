using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFootstep : MonoBehaviour
{
    public void PlaySoundOnce()
    {
        AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[4], SoundType.Footstep, "Duckstep");
    }
}
