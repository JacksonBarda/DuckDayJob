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

    public void Start()
    {
        //image = this.gameObject.GetComponent<Image>();

        // Store the original alpha value of the image
        originalAlpha = image.color.a;
    }
    public void FadeImageOverTime(float fadeTime, Interactable targetObject)
    {
        // Start the fade-out coroutine
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
        //StartCoroutine(Wait(fadeTime));
    }
}
