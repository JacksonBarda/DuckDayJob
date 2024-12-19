using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    [SerializeField]
    private float darkTime;
    [SerializeField]
    private Image image;
    private float originalAlpha;
    public bool running = false;

    public void Start()
    {
        //image = this.gameObject.GetComponent<Image>();

        // Store the original alpha value of the image
        originalAlpha = image.color.a;
        //Debug.Log("FO - originalAlpha: " + originalAlpha);
    }
    public void FadeImageOutOverTime(float fadeTime, Interactable targetObject)
    {
        // Start the fade-out coroutine
        StartCoroutine(FadeOutCoroutine(fadeTime, targetObject));
    }

    private IEnumerator FadeOutCoroutine(float fadeTime, Interactable targetObject)
    {
        running = true;
        // Calculate the target alpha (fully transparent)
        float targetAlpha = 1f;

        // Calculate the alpha increment per frame
        float alphaIncrement = (originalAlpha + targetAlpha) / fadeTime;
        //Debug.Log("FO - alphaIncrement: " + alphaIncrement);

        // Fade out the image
        //Debug.Log("FO - image.color: " + image.color);
        while (image.color.a < targetAlpha)
        {
            // Update the alpha value of the image
            Color currentColor = image.color;
            currentColor.a += alphaIncrement * Time.deltaTime;
            image.color = currentColor;
            //Debug.Log("FO - currentColor: " + currentColor);

            yield return null;
        }
        running = false;
        // Call the function on the target object
        if (targetObject != null)
        {
            targetObject.Action();
        }
        // Start the fade-in coroutine
        //FadeImageOutOverTime(fadeTime);
    }


    // < -----   Cinematic use only   ----->
    public void FadeImageOutOverTime(float fadeTime)
    {
        // Start the fade-out coroutine
        StartCoroutine(FadeOutCoroutine2(fadeTime));
        
    }

    public void InstantFadeOut()
    {
        image.color = new Color(0, 0, 0, 1f);
    }

    private IEnumerator FadeOutCoroutine2(float fadeTime)
    {

        running = true;
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
        running = false;
        // Start the fade-in coroutine
        //StartCoroutine(Wait(fadeTime));
    }
}
