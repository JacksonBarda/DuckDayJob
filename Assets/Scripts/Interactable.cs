using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool activatePostPuzzle = false;
    public Interactable puzzleToActivate;
    public FadeController fade;
    public PlayerMove player;
    public string taskName;
    public bool isCompleted;
    public bool hasFailed;
    public bool isVisibleOnStart = true;
    public GameObject puzzleUI;
    public GameObject mainUI;

    public virtual void Interact() { }
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
