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
    public Transform endLocation;
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
    protected bool isLocked;
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
            fade.FadeImageOverTime(0.7f, this);
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
        }
    }
    public override void Complete()
    {
        
    }

}
