using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading;
using System;

public class CADPuzzle : Interactable
{
    [SerializeField]
    private List<Image> fragments;
    [SerializeField]
    private List<Image> outlines;
    public float rotationSpeed = 75f;
    public float snapThreshold = 0.1f;
    public float snapThresholdRot = 5.0f;
    public float rotDistance;

    [SerializeField]
    private Slider SLDR_Progress;

    [HideInInspector]
    public Image selectedFragment;
    [HideInInspector]

    private int count = 0;

    private void Update()
    {
        HandleInput();


    }

    private void HandleInput()
    {
        if (selectedFragment != null)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                selectedFragment.rectTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                selectedFragment.rectTransform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
            }
        }
    }

    public void CheckSnap(Image fragment)
    {

        int index = fragments.IndexOf(fragment);
        float distance = Vector3.Distance(fragment.rectTransform.position, outlines[index].rectTransform.position);
        rotDistance = Mathf.Abs((fragment.rectTransform.rotation.eulerAngles.z - outlines[index].rectTransform.rotation.eulerAngles.z));
        if (distance <= snapThreshold && (360f - snapThresholdRot) <= rotDistance || rotDistance <= snapThresholdRot)
        {
            fragment.rectTransform.position = outlines[index].rectTransform.position;
            fragment.rectTransform.rotation = outlines[index].rectTransform.rotation;
            count++;
            selectedFragment.gameObject.GetComponent<FragmentDragger>().FragmentPlaced();
            AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[3], SoundType.InteractableSFX, "ISFX_CADPiecePlaced");
        }
        else
        {
            fragment.rectTransform.position = fragment.GetComponent<FragmentDragger>().originalPosition;
        }
        if (count >= fragments.Count)
        {
            Complete();
        }
    }

    public override void Interact()
    {
        base.Interact();
        PlayerMove.puzzleMode = true;
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
    }

    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override void Complete()
    {
        base.Complete();

        count = 0;
        foreach (Image fragment in fragments)
        {
            fragment.rectTransform.position = fragment.GetComponent<FragmentDragger>().originalPosition;
        }
        SLDR_Progress.value++;
    }
}
