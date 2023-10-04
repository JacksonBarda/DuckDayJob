using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading;

public class CADPuzzle : Interactable
{
    public List<Image> fragments;
    public List<Image> outlines;
    public float rotationSpeed = 100f;
    public float snapThreshold = 0.1f;

    [SerializeField]
    private GameObject puzzleUI;
    [SerializeField]
    private GameObject mainUI;

    [HideInInspector]
    public Image selectedFragment;

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
        if (distance <= snapThreshold)
        {
            fragment.rectTransform.position = outlines[index].rectTransform.position;
            fragment.rectTransform.rotation = outlines[index].rectTransform.rotation;
            count++;
        }
        else
        {
            fragment.rectTransform.position = fragment.GetComponent<FragmentDragger>().originalPosition;
        }
        if(count >= fragments.Count)
        {
            Finished();
        }
    }

    public override void Interact()
    {
        player.puzzleMode = true;
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
    }

    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override void Finished()
    {
        player.puzzleMode = false;
        puzzleUI.SetActive(false);
        mainUI.SetActive(true);
        count = 0;
        foreach(Image fragment in fragments)
        {
            fragment.rectTransform.position = fragment.GetComponent<FragmentDragger>().originalPosition;
        }
      
    }
}
