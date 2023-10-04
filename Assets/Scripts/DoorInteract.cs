using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Enums;

public class DoorInteract : Interactable
{

    [SerializeField]
    private Transform endLocation;
    [SerializeField]
    private Transform Player;
    [SerializeField]
    private PlayerMove playerMove;
    [SerializeField]
    private Rigidbody rigid;
    [SerializeField]
    private UIManager manager;
    [SerializeField]
    private Locations endRoom;

    public override void Interact()
    {
        fade.FadeImageOverTime(0.7f, this);

    }
    public override void Action()
    {
        playerMove.Maze(false);
        rigid.useGravity = true;
        Player.transform.position = endLocation.position;
        manager.setLocation(endRoom);
    }
    public override void Finished()
    {

    }

}
