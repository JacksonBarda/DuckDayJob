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
    public FadeIn blackoutFadeIn;
    public FadeOut blackoutFadeOut;
    [Tooltip("Fade effect if true, hard cut if false.")]
    public bool fadeIn;
    public float fadeInTime = 0.5f;
    [Tooltip("Fade effect if true, hard cut if false.")]
    public bool fadeOut;
    public float fadeOutTime = 0.5f;

    public bool isActive;

    [Tooltip("Developer use; does not affect gameplay")]
    [Multiline(5)]
    public string notes;


    // Start is called before the first frame update
    void Start()
    {
        if (stillImage != null)
            stillImage.color = new Color(255, 255, 255, 0);
    }




    public void FadeIn()
    {
        blackoutFadeIn.FadeImageInOverTime(fadeInTime);
    }
    public void FadeOut()
    {
        blackoutFadeOut.FadeImageOverTime(fadeOutTime);
    }



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
