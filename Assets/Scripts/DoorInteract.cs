using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DoorInteract : Interactable
{

    [SerializeField]
    private Transform endLocation;
    [SerializeField]
    private Transform Player;

    public override void Interact()
    {
        fade.FadeImageOverTime(0.5f, this);

    }
    public override void Action()
    {
        Player.transform.position = endLocation.position;
    }
    protected override void Finished()
    {

    }

}
