using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public FadeController fade;
    public PlayerMove player;

    public abstract void Interact();
    public abstract void Action();
    protected abstract void Finished();

}
