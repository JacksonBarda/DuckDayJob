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
    public bool isCompleted;
    public bool hasFailed;
    public bool isVisibleOnStart = true;
    public bool stayActive = false;
    public bool activatePostPuzzle = false;
    public List<GameObject> puzzleToActivate;
    public PlayerMove player;

    public GameObject mainUI;
    public bool counted = false;
    [Header("Custom puzzle Variable")]
    public GameObject puzzleUI;

    private void Start()
    {
        if (forcePlay)
        {
            this.Interact();
        }
    }
    public virtual void Interact()
    {
        PlayerMove.puzzleMode = true;
        UIManager.InteractionPopup.SetActive(false);
    }
    public virtual void Action() { }
    public virtual void Complete()
    {

		if (activatePostPuzzle)
        {
            foreach (GameObject thingToActivate in puzzleToActivate)
            {
                if (thingToActivate != null)
                    thingToActivate.gameObject.SetActive(!thingToActivate.gameObject.activeSelf);
            }
        }
        isCompleted = true;
		UIManager.InteractionPopup.SetActive(true);
		TaskManager.onTaskComplete(this);
    }
    public virtual void Failed()
    {
        TaskManager.onTaskFailed(this);
    }

}
