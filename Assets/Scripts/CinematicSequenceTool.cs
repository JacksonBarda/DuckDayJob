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
    //[Tooltip("Number of lines in scene minus 1")]
    //private int lengthOfDialogue;
    [Tooltip("Starts at zero when sequence begins, increases by 1 with each dialogue line")]
    public int dialogueIndex;
    //[SerializeField]
    //private FadeIn fadeIn;
    //[SerializeField]
    //private FadeOut fadeOut;

    [SerializeField]
    private List<Shot> listOfShots;
    [SerializeField]
    private int shotIndex;



    // Start is called before the first frame update
    void Start()
    {

    }

    public override void Interact()
    {
        dialogue.Interact();
        mainCamera.gameObject.SetActive(false);
        cinematicCamera.gameObject.SetActive(true);
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

    public void NextLine()                                                          // this code will run once each time a new dialogue line is displayed
    {
        Shot currentShot = listOfShots[shotIndex];
        Shot lastShot;
        dialogueIndex = dialogue.index;
        try
        {
            lastShot = listOfShots[shotIndex - 1];                                  
        }
        catch (ArgumentOutOfRangeException)
        {
            lastShot = currentShot;
        }



        if (dialogueIndex == currentShot.indexFirstLine)                            // if it is time for this shot to load
        {
            Debug.Log("CineSeqTool: first line index match");
            if (shotIndex != 0 && lastShot.isStillImage)                            // if this is not the first shot in the list
            {
                lastShot.stillImage.color = new Color(255, 255, 255, 0);            // then unload previous shot
                if (lastShot.fadeOut) lastShot.FadeOut();
            }

            if (currentShot.isStillImage)                                           // if it is an image
            {
                currentShot.stillImage.color = new Color(255, 255, 255, 255);       // then load image
            }
            else                                                                    // if it is camera position
            {
                Debug.Log("CineSeqTool: new camera position");
                cinematicCamera.transform.position = currentShot.cameraPosition;    // and set camera's position and rotation.
                cinematicCamera.transform.eulerAngles = currentShot.cameraRotation;
            }

            if (currentShot.fadeIn)                                                 // if current shot is configured to fade in
            {
                currentShot.FadeIn();                                               // do it
            }
        }


        else if (dialogueIndex == currentShot.indexLastLine)                        // if it is time for this shot to unload
        {
            Debug.Log("CineSeqTool: last line index match");
            if (shotIndex < listOfShots.Count-1)                                      // if this is not the last shot, next shot will be assigned to currentShot
            {
                Debug.Log("Shot index:" + shotIndex);
                Debug.Log("list length:" + listOfShots.Count);
                shotIndex++;
            }
                                      
                                                                                    // this shot unloads after one more click

        }
    }







    //public void PlayShot(Shot shot)
    //{
    //    int duration = shot.indexCutOut - shot.indexCutIn;

    //    for (int x = 0; x < duration; x++)      //  <---------------------- NEEDS WORK --------------------------->
    //    {                                       //this code will run once each time a new dialogue line is displayed
            
    //        if (dialogueIndex == shot.indexCutIn)
    //        {
    //            if (shot.fadeIn)
    //            {
    //                //fade-in code
    //            }
    //        }
    //        if (dialogueIndex == shot.indexCutOut)
    //        {
    //            if (shot.fadeOut)
    //            {
    //                //fade-out code
    //            }

    //            //code to deactivate current shot, activate next shot
    //            break;
    //        }
    //        //await this.CheckIndexForEvent(shot);
    //    }
    //}
}
