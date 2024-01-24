using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public FadeController fade;
    public PlayerMove player;
    public string taskName;
    public bool isCompleted;
    public bool hasFailed;
    public GameObject puzzleUI;
    public GameObject mainUI;

    public virtual void Interact() { }
    public virtual void Action() { }
    public virtual void Complete()
    {
        TaskManager.onTaskComplete(this);
    }
    public virtual void Failed()
    {
        TaskManager.onTaskFailed(this);
    }

}
