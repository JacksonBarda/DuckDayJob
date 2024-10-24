using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Enums;

public class DoorInteract : Interactable
{
    [SerializeField]
    private FadeIn fadeIn;
    [SerializeField]
    private FadeOut fadeOut;
    [SerializeField]
    private float timeToFade = 0.5f;
    [SerializeField]
    public Transform endLocation;
    [SerializeField]
    private Transform Player;
    [SerializeField]
    private PlayerMove playerMove;
    [SerializeField]
    private Rigidbody rigid;
    [SerializeField]
    private UIManager manager;
    [SerializeField]
    public Locations endRoom;
    [SerializeField]
    private FollowPlayer followPlayer;
    [SerializeField]
    private bool isLocked;
    [SerializeField]
    private DialogueTool lockedDialogue;

    public override void Interact()
    {
        if (isLocked)
        {
            lockedDialogue.Interact();
        }
        else
        {
            fadeOut.FadeImageOverTime(timeToFade, this);
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
        yield return new WaitForSeconds(delay);
		StartCoroutine(fadeIn.FadeInCoroutine(1.0f, this, false));
	}

}
