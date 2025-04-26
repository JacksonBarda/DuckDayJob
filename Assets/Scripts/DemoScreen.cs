using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScreen : Interactable
{
    [SerializeField]
    private GameObject credits;
    [SerializeField]
    private GameObject endingNote;
    [SerializeField]
    private float beginningY;
    [SerializeField]
    private float endingY;
    [SerializeField]
    private float creditsDurationInSeconds;

    private bool scrollCredits = true;

    private void Awake()
    {
        puzzleUI.SetActive(false);
        credits.SetActive(false);
    }

    public override void Interact()
    {
        base.Interact();
        StartCoroutine(creditsCoroutine());
    }

    private IEnumerator creditsCoroutine()
    {
        puzzleUI.SetActive(true);
        credits.SetActive(true);
        credits.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, beginningY);

        float increment = (endingY - beginningY) / creditsDurationInSeconds;
        Vector2 currentPosition = credits.GetComponent<RectTransform>().anchoredPosition;

        while (currentPosition.y < endingY)
        {
            currentPosition.y += increment * Time.deltaTime;
            credits.GetComponent<RectTransform>().anchoredPosition = currentPosition;
            yield return null;
        }

        //credits.SetActive(false);
        //endingNote.SetActive(true);
    }
}
