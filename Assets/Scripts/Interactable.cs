using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public FadeController fade;
    public PlayerMove player;
    public string taskName;
    public bool isCompleted;
    public bool hasFailed;
    
    public abstract void Interact();
    public abstract void Action();
    public abstract void Finished();

}
