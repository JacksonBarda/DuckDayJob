using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeController : MonoBehaviour
{
    public static FadeController Instance { get; private set; }
    public GameObject fadeSquare;

    private Color desiredColor;
    private float currentAlpha;
    private float fadeDuration = 2f;
    private float elapsedTime;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(this);
            return;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Increment the elapsed time
        elapsedTime += Time.deltaTime;

        // Check if the fade effect is complete
        while(elapsedTime <= fadeDuration)
        { 
            Debug.Log("StartFade");
            // Update the fade material color
            Color initialColor = fadeSquare.GetComponent<Image>().color;
            fadeSquare.GetComponent<Image>().color = Color.Lerp(initialColor, desiredColor, elapsedTime / fadeDuration);
        }
    }
    public void FadeOut()
    {
        desiredColor = new Color(255,255,255,1);
        elapsedTime = 0;
    }
    public void FadeIn()
    {
        desiredColor = new Color(255,255,255,0);
        elapsedTime = 0;
    }
    public IEnumerator fadeOutSquare(bool fadeToBlack = true, float fadeSpeed = 1f)
    {

        Color objectColor = fadeSquare.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while(fadeSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                fadeSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
        else
        {
            while (fadeSquare.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                fadeSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
 
        yield return new WaitForEndOfFrame();
    }
}
