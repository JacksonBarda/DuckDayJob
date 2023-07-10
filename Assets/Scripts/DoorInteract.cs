using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : Interactable
{
    [SerializeField]
    private Transform endLocation;
    [SerializeField]
    private Transform Player;

    

    public override void Interact()
    {

        Player.transform.position = endLocation.position;

        Finished();
    }
    protected override void Finished()
    {

        Debug.Log("Telported");
    }

}
