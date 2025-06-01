using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockPick : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Image>().sprite.name == "IMG_Picklock_BottomPin")
            AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[3], SoundType.InteractableSFX, "ISFX_LockPinContact");
    }
}
