using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Enums;

public class DoorInteract : Interactable
{
    [SerializeField]
    protected FadeIn fadeIn;
    [SerializeField]
    protected FadeOut fadeOut;
    [SerializeField]
    protected float timeToFade = 0.5f;
    [SerializeField]
    protected Transform endLocation;
    [SerializeField]
    protected Transform Player;
    [SerializeField]
    protected PlayerMove playerMove;
    [SerializeField]
    protected Rigidbody rigid;
    [SerializeField]
    protected UIManager manager;
    [SerializeField]
    public Locations endRoom;
    [SerializeField]
    protected FollowPlayer followPlayer;
    [SerializeField]
    private bool isLocked;
    [SerializeField]
    protected DialogueTool lockedDialogue;

    public override void Interact()
    {
        if (isLocked)
        {
            lockedDialogue.Interact();
        }
        else
        {
            fadeOut.FadeImageOutOverTime(timeToFade, this);
        }
    }
    public override void Action()
    {
        if (!isLocked)
        {
            playerMove.Maze(false);
            rigid.useGravity = true;
            Player.transform.position = endLocation.position;
            manager.setLocation(endRoom);
            followPlayer.SetBumps(endRoom);

            StartCoroutine(Wait(1.0f));
        }
    }
    public override void Complete()
    {

    }
    public IEnumerator Wait(float delay)
    {
        UnityEngine.Debug.Log("Waiting to fade in");
        yield return new WaitForSeconds(delay);
        fadeIn.FadeImageInOverTime(1.0f, this, false);
    }

    public void LockDoor()
    {
        isLocked = true;
    }
    public void UnlockDoor()
    {
        isLocked = false;
    }
    public void ToggleDoor()
    {
        isLocked = !isLocked;
    }
    public bool GetIsLocked()
    {
        return isLocked;
    }

}
