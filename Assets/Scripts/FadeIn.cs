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

    public void Start()
    {
        //image = this.gameObject.GetComponent<Image>();

        // Store the original alpha value of the image
        originalAlpha = image.color.a;
    }
    public void FadeImageInOverTime(float fadeTime, Interactable targetObject, bool callComplete)
    {
        // Start the fade-out coroutine
        StartCoroutine(FadeInCoroutine(fadeTime, targetObject, callComplete));
    }
    public IEnumerator FadeInCoroutine(float fadeTime, Interactable task, bool callComplete)
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
        if (!task.isCompleted && callComplete)
        {
            task.Complete();
        }
        
    }
}
