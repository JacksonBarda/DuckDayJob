using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VentInteract : DoorInteract
{
    [Header("Vent Custom Variables")]
    [SerializeField]
    private Image vignette;
    [SerializeField]
    private CinemaManager cinemaManager;
    [SerializeField]
    private bool isVent2 = false;
    private bool alreadyInteracted = false;

    private void Start()
    {
        vignette.color = new Color(1f, 1f, 1f, 0);
    }

    public override void Interact()
    {
        if (alreadyInteracted == false)
        {
            StartCoroutine(VignetteLoading());

            if (isVent2)
            {
                cinemaManager.ActivateSequence();
                alreadyInteracted = true;
            }
            else
            {
                fadeOut.FadeImageOutOverTime(timeToFade, this);
            }
        }
    }
    public override void Action()
    {
        if (alreadyInteracted == false)
        {
            Player.transform.position = endLocation.position;
            followPlayer.SetBumps(endRoom);
            manager.setLocation(endRoom);
        }
    }
    public override void Complete()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (isVent2 == true)
        {
            Interact();
        }
    }

    private IEnumerator VignetteLoading()
    {
        yield return new WaitForSeconds(1.0f);
        

        if (player.mazeMode == false)
        {
            StartCoroutine(fadeIn.FadeInCoroutine(1.0f, this, false));
            vignette.color = new Color(1f, 1f, 1f, 1f);
            playerMove.Maze(true);
            rigid.useGravity = false;
        }
        else
        {
            //StartCoroutine(fadeOut.FadeImageOutOverTime(1.0f));
            vignette.color = new Color(0, 0, 0, 0);
            playerMove.Maze(false);
            rigid.useGravity = true;
        }
    }

}
