using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Interactable Default Variable")]
    public bool repeatable = false;

    public bool forcePlay = false;
    public string taskName;
    public string customPopupName;
    public bool isCompleted;
    public bool hasFailed;
    public bool isVisibleOnStart = true;
    public bool stayActive = false;
    public bool isOptional = false;
    public bool activatePostPuzzle = false;
    public List<GameObject> objectToActivate;
    public PlayerMove player;

    public GameObject mainUI;
    public bool counted = false;
    [Header("Custom puzzle Variable")]
    public GameObject puzzleUI;

    // Start will be called before first frame ONLY if GameObject is enabled; if disabled, will be called upon enabling
    public virtual void Start()
    {
        if (forcePlay)
        {
            this.Interact();
            Debug.Log(this + " Force Play +++++++++++++++++++");
        }
        //Debug.Log(this + ".Start() <<<<<<<<<<<<<<<<<<<<<<<<<<<");
    }
    public virtual void Interact()
    {
        PlayerMove.puzzleMode = true;
        UIManager.InteractionPopup.SetActive(false);
    }
    public virtual void Action() { }
    public virtual void Complete()
    {
		if (objectToActivate != null)
        {
            foreach (GameObject thingToActivate in objectToActivate)
            {
                if (thingToActivate != null && !counted)
                {
                    if (thingToActivate.GetComponent<DoorInteract>() != null)   //if GameObject is a door, toggle isLocked bool
                    {
                        thingToActivate.GetComponent<DoorInteract>().ToggleDoor();
                        Debug.Log("Interactable.cs: Setting " + thingToActivate + " to " + (thingToActivate.GetComponent<DoorInteract>().GetIsLocked() ? "locked" : "unlocked"));
                    }
                    else
                    {
                        thingToActivate.gameObject.SetActive(!thingToActivate.gameObject.activeSelf);
                        Debug.Log("Interactable.cs: Setting " + thingToActivate + " to " + (thingToActivate.gameObject.activeSelf ? "active" : "inactive") + "-----------------------------------");
                    }
                
                } 
            }
        }
        isCompleted = true;
        //UIManager.InteractionPopup.SetActive(true);
        try 
        {
            TaskManager.onTaskComplete(this);
            if (this.GetComponent<DialogueTool>() == null && 
                this.GetComponent<MoveNPD>() == null && 
                this.GetComponent<CinematicSequenceTool>() == null &&
                this.GetComponent<DayNightTransition>() == null)
            {
                AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[3], SoundType.InteractableSFX, "ISFX_TaskComplete");
            }
        }
		catch (System.NullReferenceException)
        {

        }
        Debug.Log(this + ": Complete");
    }
    public virtual void Failed()
    {
        TaskManager.onTaskFailed(this);
    }

}
