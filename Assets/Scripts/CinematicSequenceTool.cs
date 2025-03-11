using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CinematicSequenceTool : Interactable
{
    
    private Camera camMain;
    private Camera camCinematic;

    [SerializeField]
    private DialogueTool dialogue;
    [SerializeField]
    private FadeIn blackoutFadeIn;
    [SerializeField]
    private FadeOut blackoutFadeOut;
    [Tooltip("Starts at zero when sequence begins, increases by 1 with each dialogue line")]
    public int dialogueIndex;
    [SerializeField]
    [Tooltip("Disable if another fade transition precedes this sequence, ex. if sequence is activated after door interaction")]
    private bool disableBeginningFade = false;
    [SerializeField]
    private bool disableEndingFade = false;
    [SerializeField]
    private CinemaManager CM;

    [SerializeField]
    private List<Shot> listOfShots;
    [SerializeField]
    private int shotIndex = 0;
    [SerializeField]
    private GameObject DialogueButton;
    private Shot currentShot;
    private Shot lastShot;
    [SerializeField]
    private bool alreadyIterated = false;


    //// Start is called before the first frame update
    //new void Start()
    //{
    //    base.Start();
    //}

    public override void Interact()
    {
        StartCoroutine(InitializeCoroutine());
        //camMain = CM.mainCamera;
        //camCinematic = CM.cinematicCamera;
        //CM.ActivateSequence(this);
        //dialogue.Interact();
        //camMain.gameObject.SetActive(false);
        //camCinematic.gameObject.SetActive(true);
        //dialogueIndex = 0;
        //NextLine();
        
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

        // if 
        if (!(dialogueIndex == currentShot.indexFirstLine && lastShot.fadeTransition && shotIndex != 0) && dialogueIndex != 0) IterateDialogue();

        //Debug.Log("dialogueIndex:" + dialogueIndex + "; currentShot.indexFirstLine:" + currentShot.indexFirstLine);
        if (dialogueIndex == currentShot.indexFirstLine)                            // if it is time for this shot to load
        {
            if (shotIndex == 0)                                                         // if this is the first shot of sequence
            {
                Debug.Log("CST: First shot <<<<<<<<<<<<<<<<<<<<<<<<");
                //Debug.Log("CST: 1   +-+-+-+-+-+-+-+-+-+-+-+-");
                //blackoutFadeOut.InstantFadeOut();
                //blackoutFadeOut.FadeImageOutOverTime(currentShot.fadeTime);
                SwitchShot();
            }
            else if (shotIndex != 0)                                                    // if this is not the first shot                          
            {
                //Debug.Log("lastShot.fadeTransition: " + lastShot.fadeTransition);
                if (lastShot.fadeTransition)
                {
                    //Debug.Log("CST: 2   +-+-+-+-+-+-+-+-+-+-+-+-");
                    StartCoroutine(FadeCoroutine());
                }
                else
                {
                    //Debug.Log("CST: 3   +-+-+-+-+-+-+-+-+-+-+-+-");
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
                //Debug.Log("CST: 4   +-+-+-+-+-+-+-+-+-+-+-+-");
            }
            else
            {
                //Debug.Log("CST: 5   +-+-+-+-+-+-+-+-+-+-+-+-");
                Debug.Log("LAST LINE OF SEQUENCE <<<<<<<<<<<<<<<<<<<<<<<");
                //StartCoroutine(EndingFadeCoroutine());
            }
        }

        dialogueIndex++;

        if (dialogueIndex > listOfShots[listOfShots.Count - 1].indexLastLine+1)
        {
            StartCoroutine(EndingCoroutine());
            //Debug.Log("CST: EndingFadeCoroutine()");
        }
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
        //Debug.Log("CST: SwitchShot() from " + lastShot + " to " + currentShot);
        // unloading procedure

        if (lastShot.isStillImage && shotIndex != 0)
        {
            //Debug.Log("CST: 6   +-+-+-+-+-+-+-+-+-+-+-+-");
            lastShot.stillImage.gameObject.SetActive(false);
        }
        else if (!lastShot.isStillImage && shotIndex != 0)
        {
            //Debug.Log("CST: 7   +-+-+-+-+-+-+-+-+-+-+-+-");
            foreach (GameObject npd in lastShot.listOfSprites)
            {
                npd.SetActive(false);
            }
        }

        // load new shot
        if (currentShot.isStillImage)                                           // if it is an image
        {
            //Debug.Log("CST: 8   +-+-+-+-+-+-+-+-+-+-+-+-");
            //Debug.Log("CST: Load still image");
            currentShot.stillImage.gameObject.SetActive(true);                  // then load image
        }
        else                                                                    // if it is camera position
        {
            //Debug.Log("CST: 9   +-+-+-+-+-+-+-+-+-+-+-+-");
            camCinematic.transform.position = currentShot.cameraPosition;    // and set camera's position and rotation.
            camCinematic.transform.eulerAngles = currentShot.cameraRotation;
            foreach (GameObject npd in currentShot.listOfSprites)
            {
                npd.SetActive(true);
            }
        }

        if (currentShot.sceneChangeInteractable != null) currentShot.sceneChangeInteractable.Interact();

        if (shotIndex == listOfShots.Count)
        {
            camMain.gameObject.SetActive(true);
            camCinematic.gameObject.SetActive(false);
        }
        //CinemaManager.IterateDialogue();
    }

    public List<Shot> getListOfShots()
    {
        return listOfShots;
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
            camCinematic.gameObject.SetActive(false);
            camMain.gameObject.SetActive(true);
            CM.DeactivateSequence();
        }

		blackoutFadeIn.FadeImageInOverTime(currentShot.fadeTime);
		//SwitchShot();
		DialogueButton.SetActive(true);
	}

    public IEnumerator InitializeCoroutine()
    {
        if (!disableBeginningFade)
        {
            blackoutFadeOut.FadeImageOutOverTime(1f);
            yield return new WaitForSeconds(1.0f);
            blackoutFadeIn.FadeImageInOverTime(1f);
        }
        camMain = CM.mainCamera;
        camCinematic = CM.cinematicCamera;
        CM.ActivateSequence(this);
        dialogue.Interact();

        camMain.gameObject.SetActive(false);
        camCinematic.gameObject.SetActive(true);
        player.gameObject.SetActive(false);
        dialogueIndex = 0;
        NextLine();
    }

    public IEnumerator EndingCoroutine()
    {
        if (!disableEndingFade)
        {
            blackoutFadeOut.FadeImageOutOverTime(1f);
            yield return new WaitForSeconds(1.0f);
            blackoutFadeIn.FadeImageInOverTime(1f);
        }

        DialogueButton.SetActive(false);
        DialogueButton.SetActive(true);

        camCinematic.gameObject.SetActive(false);
        camMain.gameObject.SetActive(true);
        player.gameObject.SetActive(true);
        CM.DeactivateSequence();
        base.Complete();
    }
}
