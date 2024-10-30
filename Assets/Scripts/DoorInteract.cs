using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Enums;

public class DoorInteract : Interactable
{

    public Transform endLocation;
    public Transform Player;
    public PlayerMove playerMove;
    public Rigidbody rigid;
    public UIManager manager;
    public Locations endRoom;
    public FollowPlayer followPlayer;
    public bool isLocked;
    public DialogueTool lockedDialogue;

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
