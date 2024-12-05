using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shot : MonoBehaviour
{
    public Vector3 cameraPosition;
    public Vector3 cameraRotation;
    [Tooltip("Use if using an image instead of camera angle")]
    public Image stillImage;
    public bool isStillImage = false;
    [Tooltip("First line of shot. 0 is first line of scene.")]
    public int indexFirstLine;
    [Tooltip("Last line of shot. 0 is first line of scene.")]
    public int indexLastLine;

    [Tooltip("Fade to black, then fade in to new shot if true, hard cut if false. Transition refers to end of this shot, beginning of next shot")]
    public bool fadeTransition;
    public float fadeTime = 0.5f;

    public bool isActive;

    [Tooltip("Developer use; does not affect gameplay")]
    [Multiline(5)]
    public string notes;


    // Start is called before the first frame update
    void Start()
    {
        if (stillImage != null)
        {
            stillImage.gameObject.SetActive(false);
            stillImage.color = new Color(1f, 1f, 1f, 1f);
        }
            
    }

    //public void FadeIn()
    //{
    //    blackoutFadeIn.FadeImageInOverTime(fadeTime);
    //    Debug.Log("fade IN");
    //}
    //public void FadeOut()
    //{
    //    blackoutFadeOut.FadeImageOverTime(fadeTime);
    //    Debug.Log("fade OUT");
    //}



    //public IEnumerator FadeInCoroutine(float fadeTime)
    //{
    //    float targetAlpha = 255;    //fully visible

    //    // Calculate the alpha increment per frame
    //    float alphaIncrement = (targetAlpha + stillImage.color.a) / (fadeTime / 2);

    //    // Fade in the image
    //    while (stillImage.color.a > targetAlpha)
    //    {
    //        // Update the alpha value of the image
    //        Color currentColor = stillImage.color;
    //        currentColor.a -= alphaIncrement * Time.deltaTime;
    //        stillImage.color = currentColor;

    //        yield return null;
    //    }
    //}

    //public IEnumerator FadeOutCoroutine(float fadeTime)
    //{
    //    float targetAlpha = 0;    //fully transparent

    //    // Calculate the alpha increment per frame
    //    float alphaIncrement = (targetAlpha + stillImage.color.a) / (fadeTime / 2);

    //    // Fade in the image
    //    while (stillImage.color.a > targetAlpha)
    //    {
    //        // Update the alpha value of the image
    //        Color currentColor = stillImage.color;
    //        currentColor.a -= alphaIncrement * Time.deltaTime;
    //        stillImage.color = currentColor;

    //        yield return null;
    //    }
    //}
}
