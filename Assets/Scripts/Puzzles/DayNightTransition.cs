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
    private UIManager UIManager;
    [SerializeField]
    private FadeIn moonIn;
    [SerializeField]
    private FadeIn sunIn;
    [SerializeField]
    private FadeIn coverIn;
    [SerializeField]
    private FadeIn backgroundIn;
    [SerializeField]
    private FadeOut moonOut;
    [SerializeField]
    private FadeOut sunOut;
    [SerializeField]
    private FadeOut coverOut;
    [SerializeField]
    private FadeOut backgroundOut;
    [SerializeField]
    private RectTransform pivot;
    [SerializeField] 
    private GameObject contButton;
    [SerializeField]
    private Transform endLocation;
    [SerializeField]
    private Locations endRoom;
    [SerializeField]
    private FollowPlayer camera;
    private bool actionCalled = false;




    private void Update()
    {
        if (actionCalled)
        {
            float rotatePivot = pivot.rotation.z + 180;
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
        base.Interact();
        //player.puzzleMode = true;
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
        moonOut.FadeImageOverTime(1.0f, this);
        sunOut.FadeImageOverTime(1.0f, this);
        coverOut.FadeImageOverTime(1.0f, this);
        backgroundOut.FadeImageOverTime(1.0f, this);
        counted = false;
    }

    public override void Action()
    {
        actionCalled = true;
        player.gameObject.transform.position = endLocation.position;
        camera.SetBumps(endRoom);
        UIManager.setLocation(endRoom);

    }

    public override void Complete()
    {
        base.Complete();
        contButton.SetActive(false);
        if (190f > pivot.rotation.z && pivot.rotation.z > 170f)
        {
            UIManager.SetNightTime();

        }
        else
        {
            UIManager.SetMorningTime();
        }

    }
    public void OnContPressed()
    {
        StartCoroutine(moonIn.FadeInCoroutine(1.0f, this));
        StartCoroutine(sunIn.FadeInCoroutine(1.0f, this));
        StartCoroutine(coverIn.FadeInCoroutine(1.0f, this));
        StartCoroutine(backgroundIn.FadeInCoroutine(1.0f, this));
    }
}
