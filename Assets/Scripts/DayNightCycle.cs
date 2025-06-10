using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DayNightCycle : MonoBehaviour
{
    [SerializeField]
    private float darkTime;
    private Image image;
    private float originalAlpha;
    //[SerializeField]
    //private FadeController[] fadeControllers = null;

    public void Start()
    {
        image = GetComponent<Image>();

        // Store the original alpha value of the image
        originalAlpha = image.color.a;
    }
    public void FadeImageOverTime(float fadeTime, Interactable targetObject)
    {
        // Start the fade-out coroutine
        //foreach()
        StartCoroutine(FadeOutCoroutine(fadeTime, targetObject));
    }

    private IEnumerator FadeOutCoroutine(float fadeTime, Interactable targetObject)
    {
        // Calculate the target alpha (fully transparent)
        float targetAlpha = 1f;

        // Calculate the alpha increment per frame
        float alphaIncrement = (originalAlpha + targetAlpha) / fadeTime;

        // Fade out the image
        while (image.color.a < targetAlpha)
        {
            // Update the alpha value of the image
            Color currentColor = image.color;
            currentColor.a += alphaIncrement * Time.deltaTime;
            image.color = currentColor;

            yield return null;
        }

        // Call the function on the target object
        targetObject.Action();

        // Start the fade-in coroutine
        StartCoroutine(Wait(fadeTime));
    }
    private IEnumerator Wait(float fadeTime)
    {
        float time = 0;
        while (time < darkTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(FadeInCoroutine(fadeTime));
    }
    private IEnumerator FadeInCoroutine(float fadeTime)
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
