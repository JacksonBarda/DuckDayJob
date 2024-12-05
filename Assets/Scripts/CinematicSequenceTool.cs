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
    private List<Shot> listOfShots;
    [SerializeField]
    private int shotIndex;
    private Shot currentShot;
    private Shot lastShot;


    // Start is called before the first frame update
    void Start()
    {

    }

    public override void Interact()
    {
        dialogue.Interact();
        mainCamera.gameObject.SetActive(false);
        cinematicCamera.gameObject.SetActive(true);
        NextLine();
    }

    //public override void Action()
    //{

    //}
    //public override void Complete()
    //{

    //}

    //public void NextShot()          //switch to next shot
    //{

    //}

    public void NextLine()                                              // this code will run once each time a new dialogue line is displayed
    {
        Debug.Log("-------------------------------------------------------------------------------------");
        currentShot = listOfShots[shotIndex];
        dialogueIndex = dialogue.index;
        Debug.Log("dialogueIndex: " + dialogue.index);
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


        Debug.Log("dialogueIndex:" + dialogueIndex + "currentShot.indexFirstLine:" + currentShot.indexFirstLine);
        if (dialogueIndex == currentShot.indexFirstLine)                            // if it is time for this shot to load
        {
            Debug.Log("shotIndex: " + shotIndex);
            if (shotIndex == 0)                                                         // if this is the first shot of sequence
            {
                Debug.Log("first shot of sequence");                                // immediately go to black
                blackoutFadeOut.InstantFadeOut();
                blackoutFadeIn.FadeImageInOverTime(currentShot.fadeTime);
            }
            else if (shotIndex != 0)                                                    // if this is not the first shot                          
            {
                Debug.Log("lastShot.fadeTransition: " + lastShot.fadeTransition);
                if (lastShot.fadeTransition)
                {
                    FadeCoroutine();
                }
                SwitchShot();
            }
        }

        else if (dialogueIndex == currentShot.indexLastLine)                        // if it is time for this shot to unload
        {
            //Debug.Log("CineSeqTool: last line index match");
            if (shotIndex < listOfShots.Count-1)                                    // if this is not the last shot, next shot will be assigned to currentShot
            {                                                                       // this shot unloads after one more click
                shotIndex++;
            }
            else
            {
                FadeCoroutine();
            }
        }
    }

    public void SwitchShot()
    {
        // unloading procedure
        
        if (lastShot.isStillImage && shotIndex != 0) lastShot.stillImage.gameObject.SetActive(false);

        // load new shot
        if (currentShot.isStillImage)                                           // if it is an image
        {
            currentShot.stillImage.gameObject.SetActive(true);                  // then load image
        }
        else                                                                    // if it is camera position
        {
            cinematicCamera.transform.position = currentShot.cameraPosition;    // and set camera's position and rotation.
            cinematicCamera.transform.eulerAngles = currentShot.cameraRotation;
        }

        if (shotIndex == listOfShots.Count - 1)
        {
            mainCamera.gameObject.SetActive(true);
            cinematicCamera.gameObject.SetActive(false);
        }
    }

    public IEnumerator FadeCoroutine()          //fade to black, switch image/camera position, fade out from black
    {
        blackoutFadeOut.FadeImageOutOverTime(lastShot.fadeTime);
        yield return new WaitForSeconds(1);
        blackoutFadeIn.FadeImageInOverTime(currentShot.fadeTime);
        //SwitchShot();
    }

}
