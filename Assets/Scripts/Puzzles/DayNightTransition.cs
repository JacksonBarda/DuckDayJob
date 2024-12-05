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
    private UIManager UIManager;
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
        counted = false;
        base.Interact();
        pivotStartRot = pivot.rotation;
        //player.puzzleMode = true;
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
        fadeOut.FadeImageOutOverTime(1.0f, this);


        counted = false;
    }

    public override void Action()
    {
        dayNightOut.FadeImageOutOverTime(1.0f, null);
        actionCalled = true;
        player.gameObject.transform.position = endLocation.position;
        playerCamera.SetBumps(endRoom);
        UIManager.setLocation(endRoom);

    }

    public override void Complete()
    {

        pivot.rotation = pivotStartRot;
        contButton.SetActive(false);
        if (dayToNight)
        {
            UIManager.SetNightTime();

        }
        else
        {
            UIManager.SetMorningTime();
        }
        base.Complete();
        Debug.Log("FadeIn Completed");
    }
    public void OnContPressed()
    {

        StartCoroutine(dayNightIn.FadeInCoroutine(1.3f, this, true));
        StartCoroutine(fadeIn.FadeInCoroutine(1.3f, this, false));

    }
}
