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
    private void Start()
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
                    if (thingToActivate.name.Contains("Door"))   //if GameObject is a door, toggle isLocked bool
                    {
                        thingToActivate.GetComponent<DoorInteract>().isLocked = !thingToActivate.GetComponent<DoorInteract>().isLocked;
                        Debug.Log("Interactable.cs: Setting " + thingToActivate + " to " + (thingToActivate.GetComponent<DoorInteract>().isLocked ? "locked" : "unlocked"));
                    }
                    else
                    {
                        thingToActivate.gameObject.SetActive(!thingToActivate.gameObject.activeSelf);
                        Debug.Log("Interactable.cs: Setting " + thingToActivate + " to " + (thingToActivate.gameObject.activeSelf ? "active" : "inactive"));
                    }
                
                } 
            }
        }
        isCompleted = true;
		//UIManager.InteractionPopup.SetActive(true);
		TaskManager.onTaskComplete(this);
        Debug.Log(this + ": Complete");
    }
    public virtual void Failed()
    {
        TaskManager.onTaskFailed(this);
    }

}
