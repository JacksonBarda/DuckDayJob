using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading;
using System;
using Enums;

public class DayNightTransition : Interactable
{
    [SerializeField]
    private FadeIn dayNightIn;
    [SerializeField]
    private FadeOut dayNightOut;
    [SerializeField]
    private FadeIn fadeIn;
    [SerializeField]
    private FadeOut fadeOut;
    [SerializeField]
    private bool dayToNight;
    [SerializeField]
    private bool disableBeginningFade = false;
    [SerializeField]
    private bool disableEndingFade = false;
    [SerializeField]
    private UIManager UIManager;
    [SerializeField]
    private TaskManager taskManager;
    [SerializeField]
    private RectTransform pivot;
    [SerializeField] 
    private GameObject contButton;
    [SerializeField]
    private Transform endLocation;
    [SerializeField]
    private Locations endRoom;
    [SerializeField]
    private FollowPlayer playerCamera;
    private bool actionCalled = false;
    private Quaternion pivotStartRot = new Quaternion();



    private void Update()
    {
        if (actionCalled)
        {
            float rotatePivot;
            if (dayToNight)
            {
                rotatePivot = 360f;
            }
            else
            {
                rotatePivot = 180f;
            }
            // Calculate the difference between current rotation and target rotation
            float angleDifference = rotatePivot - pivot.eulerAngles.z;

            // Normalize the angle to ensure it's within -180 to 180 range
            if (angleDifference > 180f)
                angleDifference -= 360f;
            else if (angleDifference < -180f)
                angleDifference += 360f;

            // Define a rotation speed
            float rotationSpeed = 45f;

            // Rotate towards the target rotation
            float rotationAmount = Mathf.Min(Mathf.Abs(angleDifference), rotationSpeed * Time.deltaTime);
            pivot.Rotate(0f, 0f, Mathf.Sign(angleDifference) * rotationAmount);

            // Check if the rotation has reached the target
            if (Mathf.Abs(angleDifference) < 0.1f)
            {
                actionCalled = false;
                contButton.SetActive(true);

            }
        }
        
    }


    public override void Interact()
    {
		isCompleted = false;
		counted = false;
        base.Interact();
        pivotStartRot = pivot.rotation;
        //player.puzzleMode = true;
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
        AudioManager.StopSounds();

        if (!disableBeginningFade)
        {
            fadeOut.FadeImageOutOverTime(1.0f, this);
        }
        else
        {
            Action();
        }
        
        counted = false;

        if (dayToNight) dayNightIn.gameObject.transform.eulerAngles = new Vector3(0,0,180);
        else dayNightIn.gameObject.transform.eulerAngles = new Vector3(0, 0, 360);
    }

    public override void Action()
    {
        dayNightOut.FadeImageOutOverTime(1.0f, null);
        actionCalled = true;
        player.gameObject.transform.position = endLocation.position;
        if (endLocation.localEulerAngles.y % 360 > 180)
        {
            player.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
        }
        else player.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;

        playerCamera.SetBumps(endRoom);
        UIManager.setLocation(endRoom);

		if (objectToActivate != null)
		{
			foreach (GameObject thingToActivate in objectToActivate)
			{
				if (thingToActivate != null)
					thingToActivate.gameObject.SetActive(!thingToActivate.gameObject.activeSelf);
			}
		}

		UIManager.InteractionPopup.SetActive(true);


	}

    public override void Complete()
    {
        base.Complete();
        pivot.rotation = pivotStartRot;

        if (dayToNight)
        {
            UIManager.SetNightTime();
            taskManager.isNight = true;

        }
        else
        {
            UIManager.SetMorningTime();
            taskManager.isNight = false;
        }
        AudioManager.PlayRoomSounds(endRoom);
    }

    public void OnContPressed()
    {
		contButton.SetActive(false);

        if (!dayToNight) AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[3], SoundType.InteractableSFX, "ISFX_ElevatorDayBegin");

        dayNightIn.FadeImageInOverTime(1.3f, this, true);
        if (!disableEndingFade)
        {
            fadeIn.FadeImageInOverTime(1.3f, this, false);
        }
    }
}
