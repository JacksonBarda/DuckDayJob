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
    private bool autoInteract = false;
    [SerializeField]
    private bool isVent2 = false;

    private void Start()
    {
        vignette.color = new Color(255, 255, 255, 0);
    }

    public override void Interact()
    {
        fadeOut.FadeImageOverTime(timeToFade, this);

        StartCoroutine(VignetteLoading());

        if (isVent2)
        {
            //load killing room sequence      
        }

    }
    public override void Action()
    {

        Player.transform.position = endLocation.position;
        followPlayer.SetBumps(endRoom);
        manager.setLocation(endRoom);

        //StartCoroutine(Wait(1.0f));
    }
    public override void Complete()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("collision");
        {
            Interact();
        }
    }

    private IEnumerator VignetteLoading()
    {
        yield return new WaitForSeconds(0.7f);

        if (player.mazeMode == false)
        {
            vignette.color = new Color(255, 255, 255, 1);
            playerMove.Maze(true);
            Debug.Log("Maze(true)");
            rigid.useGravity = false;
        }
        else
        {
            vignette.color = new Color(255, 255, 255, 0);
            playerMove.Maze(false);
            Debug.Log("Maze(false)");
            rigid.useGravity = true;
        }
    }

}
