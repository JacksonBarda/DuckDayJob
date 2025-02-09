using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CinematicSequenceTool : Interactable
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Camera cinematicCamera;
    [SerializeField]
    private DialogueTool dialogue;
    [SerializeField]
    private FadeIn blackoutFadeIn;
    [SerializeField]
    private FadeOut blackoutFadeOut;
    [Tooltip("Starts at zero when sequence begins, increases by 1 with each dialogue line")]
    public int dialogueIndex;
    [SerializeField]
    private CinemaManager CM;

    [SerializeField]
    private List<Shot> listOfShots;
    [SerializeField]
    private int shotIndex;
    [SerializeField]
    private GameObject DialogueButton;
    private Shot currentShot;
    private Shot lastShot;
    [SerializeField]
    private bool alreadyIterated = false;


    // Start is called before the first frame update
    void Start()
    {
        cinematicCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        
    }

    public override void Interact()
    {
        Debug.Log("CineSeqTool: Interact()");
        CM.ActivateSequence(this);
        dialogue.Interact();
        mainCamera.gameObject.SetActive(false);
        cinematicCamera.gameObject.SetActive(true);
        dialogueIndex = 0;
        NextLine();
        
    }


    public void NextLine()                                              // this code will run once each time a new dialogue line is displayed
    {
        Debug.Log("-------------------------------------------------------------------------------------");
        currentShot = listOfShots[shotIndex];
        
        alreadyIterated = false;
        Debug.Log("dialogueIndex: " + dialogueIndex);
        Debug.Log("shotIndex: " + shotIndex);
        Debug.Log("currentShot: " + currentShot);

        try
        {
            lastShot = listOfShots[shotIndex - 1];
            Debug.Log("lastshot: " + lastShot);
        }
        catch (ArgumentOutOfRangeException)
        {
            lastShot = currentShot;
            Debug.Log("lastshot = currentshot");
        }

        if (dialogueIndex > listOfShots[listOfShots.Count - 1].indexLastLine)
        {
            StartCoroutine(EndingFadeCoroutine());
        }

        if (!(dialogueIndex == currentShot.indexFirstLine && lastShot.fadeTransition && shotIndex != 0) && dialogueIndex != 0) IterateDialogue();

        //Debug.Log("dialogueIndex:" + dialogueIndex + "; currentShot.indexFirstLine:" + currentShot.indexFirstLine);
        if (dialogueIndex == currentShot.indexFirstLine)                            // if it is time for this shot to load
        {
            if (shotIndex == 0)                                                         // if this is the first shot of sequence
            {
                //blackoutFadeOut.InstantFadeOut();
                blackoutFadeIn.FadeImageInOverTime(currentShot.fadeTime);
                SwitchShot();
            }
            else if (shotIndex != 0)                                                    // if this is not the first shot                          
            {
                //Debug.Log("lastShot.fadeTransition: " + lastShot.fadeTransition);
                if (lastShot.fadeTransition)
                {
                    StartCoroutine(FadeCoroutine());
                }
                else
                {
                    SwitchShot();
                }

            }
        }

        else if (dialogueIndex == currentShot.indexLastLine)                        // if it is time for this shot to unload
        {
            //CinemaManager.IterateDialogue();
            //Debug.Log("CineSeqTool: last line index match");
            if (shotIndex < listOfShots.Count - 1)                                    // if this is not the last shot, next shot will be assigned to currentShot
            {                                                                       // this shot unloads after one more click
                shotIndex++;
            }
            else
            {
                Debug.Log("LAST LINE OF SEQUENCE <<<<<<<<<<<<<<<<<<<<<<<");
                //StartCoroutine(EndingFadeCoroutine());
            }
        }

        dialogueIndex++;
    }

    public void IterateDialogue()
    {
        if (alreadyIterated == false)
        {
            CM.IterateDialogue();
        }
        
    }

    public void SwitchShot()
    {
        // unloading procedure
        
        if (lastShot.isStillImage && shotIndex != 0) lastShot.stillImage.gameObject.SetActive(false);
        else if (!lastShot.isStillImage && shotIndex != 0)
        {
            foreach (GameObject npd in lastShot.listOfSprites)
            {
                npd.SetActive(false);
            }
        }

        // load new shot
        if (currentShot.isStillImage)                                           // if it is an image
        {
            currentShot.stillImage.gameObject.SetActive(true);                  // then load image
        }
        else                                                                    // if it is camera position
        {
            cinematicCamera.transform.position = currentShot.cameraPosition;    // and set camera's position and rotation.
            cinematicCamera.transform.eulerAngles = currentShot.cameraRotation;
            foreach (GameObject npd in currentShot.listOfSprites)
            {
                npd.SetActive(true);
            }
        }

        if (shotIndex == listOfShots.Count)
        {
            mainCamera.gameObject.SetActive(true);
            cinematicCamera.gameObject.SetActive(false);
        }
        //CinemaManager.IterateDialogue();
    }

    public IEnumerator FadeCoroutine()          //fade to black, switch image/camera position, fade out from black
    {
        
        DialogueButton.SetActive(false);
        blackoutFadeOut.FadeImageOutOverTime(lastShot.fadeTime);

		yield return new WaitForSeconds(1.0f);
        IterateDialogue();
        alreadyIterated = true;
        SwitchShot();
        
        if (shotIndex == listOfShots.Count-1 && dialogueIndex == currentShot.indexLastLine)
        {
            cinematicCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            CM.DeactivateSequence();
        }

		blackoutFadeIn.FadeImageInOverTime(currentShot.fadeTime);
		//SwitchShot();
		DialogueButton.SetActive(true);
	}

    public IEnumerator EndingFadeCoroutine()
    {
        DialogueButton.SetActive(false);
        blackoutFadeOut.FadeImageOutOverTime(lastShot.fadeTime);
        yield return new WaitForSeconds(1.0f);
        blackoutFadeIn.FadeImageInOverTime(currentShot.fadeTime);
        DialogueButton.SetActive(true);

        cinematicCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        CM.DeactivateSequence();
    }
}
