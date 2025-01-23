using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VentInteract : DoorInteract
{
    [SerializeField]
    private Image vignette;
    [SerializeField]
    private bool autoInteract = false;
    [SerializeField]
    private bool isVent2 = false;
    [SerializeField]
    private CinemaManager cinemaManager;
    private bool alreadyInteracted = false;
    [SerializeField]
    private DialogueTool preinteractDialogue;

    private void Start()
    {
        vignette.color = new Color(255, 255, 255, 0);
    }

	public override void Interact()
	{
		if (alreadyInteracted == false)
		{
			StartCoroutine(VignetteLoading());

			if (isVent2)
			{
				fadeOut.FadeImageOutOverTime(timeToFade, this);

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
        
        Player.transform.position = endLocation.position;
        followPlayer.SetBumps(endRoom);
        manager.setLocation(endRoom);
        
    }
    public override void Complete()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("collision");
        if (autoInteract == true)
        {
            Interact();
            Debug.Log("auto interact");
        }
    }

    private IEnumerator VignetteLoading()
    {
 
        yield return new WaitForSeconds(0.7f);
        if (isVent2 == false)
        {
			StartCoroutine(Wait(1.0f));
            if (preinteractDialogue != null) preinteractDialogue.Interact();

        }
        else
        {
			StartCoroutine(Wait2(1.0f));

        }       

    }
	public IEnumerator Wait(float delay)
	{
		vignette.color = new Color(255, 255, 255, 1);
		playerMove.Maze(true);
		Debug.Log("Maze(true)");
		rigid.useGravity = false;
		Debug.Log("Waiting to fade in");
		yield return new WaitForSeconds(delay);
		StartCoroutine(fadeIn.FadeInCoroutine(1.0f, this, false));
	}
	public IEnumerator Wait2(float delay)
	{
		vignette.color = new Color(255, 255, 255, 0);
		playerMove.Maze(false);
		Debug.Log("Maze(false)");
		rigid.useGravity = true;
		Debug.Log("Waiting to fade in");
		yield return new WaitForSeconds(delay);
		StartCoroutine(fadeIn.FadeInCoroutine(1.0f, this, false));
		cinemaManager.ActivateSequence();
	}
}
