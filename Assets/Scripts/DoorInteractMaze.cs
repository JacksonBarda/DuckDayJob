using Enums;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DoorInteractMaze : Interactable
{
    [SerializeField]
    private FadeIn fadeIn;
    [SerializeField]
    private FadeOut fadeOut;
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
        fadeOut.FadeImageOutOverTime(0.5f, this);


    }
    public override void Action()
    {
        
        playerMove.Maze(true);
        rigid.useGravity = false;
        Player.transform.position = endLocation.position;
        manager.setLocation(endRoom);
        StartCoroutine(fadeIn.FadeInCoroutine(0.5f, this, false));
    }
    public override void Complete()
    {

    }

}
