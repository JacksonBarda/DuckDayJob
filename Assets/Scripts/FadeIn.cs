using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField]
    private float darkTime;
    [SerializeField]
    private Image image;
    private float originalAlpha;
    private bool running = false;
    [SerializeField]
    private PlayerMove player;

    // -------------  FADE IN COMES LAST ------------------------

    public void Start()
    {
        //image = this.gameObject.GetComponent<Image>();

        // Store the original alpha value of the image
        originalAlpha = 0; //= image.color.a;
        //Debug.Log("FI - originalAlpha: " + originalAlpha);
    }
    public void FadeImageInOverTime(float fadeTime, Interactable targetObject, bool callComplete)
    {
        if (!running)
        {
            // Start the fade-out coroutine
            StartCoroutine(FadeInCoroutine(fadeTime, targetObject, callComplete));
            Debug.Log("FadeIn.cs: FadeInCoroutine >>>>>>>>>>>>>>>>>>>");
        }
    }
    private IEnumerator FadeInCoroutine(float fadeTime, Interactable task, bool callComplete)
    {
        running = true;
        PlayerMove.puzzleMode = true;
        Debug.Log("FadeIn.cs: FadeInCoroutine >>>>>>>>>>>>>>>>>>>");
        // Calculate the target alpha (original alpha)
        float targetAlpha = originalAlpha;

        // Calculate the alpha increment per frame
        float alphaIncrement = (targetAlpha + image.color.a) / (fadeTime / 2);
        //Debug.Log("FI - alphaIncrement: " + alphaIncrement);

        // Fade in the image
        //Debug.Log("FI - image.color: " + image.color);
        while (image.color.a > targetAlpha)
        {
            // Update the alpha value of the image
            Color currentColor = image.color;
            currentColor.a -= alphaIncrement * Time.deltaTime;
            image.color = currentColor;
            //Debug.Log("FI - currentColor: " + currentColor);

            yield return null;
        }

        // Ensure the final alpha value is exactly the original alpha
        Color finalColor = image.color;
        finalColor.a = originalAlpha;
        image.color = finalColor;

        running = false;
        PlayerMove.puzzleMode = false;

        if (!task.isCompleted && callComplete)
        {
            task.Complete();
        }
        
    }


    // < -----   Cinematic use only   ----->
    public void FadeImageInOverTime(float fadeTime)
    {
        // Start the fade-out coroutine
        StartCoroutine(FadeInCoroutine(fadeTime));
    }

    public IEnumerator FadeInCoroutine(float fadeTime)
    {
        // Calculate the target alpha (original alpha)
        float targetAlpha = originalAlpha;

        // Calculate the alpha increment per frame
        float alphaIncrement = (targetAlpha + image.color.a) / (fadeTime / 2);

        // Fade in the image
        while (image.color.a > targetAlpha)
        {
            // Update the alpha value of the image
            Color currentColor = image.color;
            currentColor.a -= alphaIncrement * Time.deltaTime;
            image.color = currentColor;

            yield return null;
        }

        // Ensure the final alpha value is exactly the original alpha
        Color finalColor = image.color;
        finalColor.a = originalAlpha;
        image.color = finalColor;

    }
}
