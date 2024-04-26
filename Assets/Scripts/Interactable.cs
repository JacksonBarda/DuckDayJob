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
    public Interactable puzzleToActivate;

    public FadeController fade;
    public PlayerMove player;

    public GameObject mainUI;
    public bool counted = false;
    [Header("Custom puzzle Variable")]
    public GameObject puzzleUI;

    public virtual void Interact()
    {
        PlayerMove.puzzleMode = true;
    }
    public virtual void Action() { }
    public virtual void Complete()
    {
        if (activatePostPuzzle)
        {
            if(puzzleToActivate != null)
                puzzleToActivate.gameObject.SetActive(true);
        }
        isCompleted = true;
        TaskManager.onTaskComplete(this);
    }
    public virtual void Failed()
    {
        TaskManager.onTaskFailed(this);
    }

}
