using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemaManager : MonoBehaviour
{
    [SerializeField]
    private CinematicSequenceTool currentSequence;
    [SerializeField]
    private bool cinemaMode = false;
    [SerializeField]
    private List<CinematicSequenceTool> listOfSequences;
    [SerializeField]
    private int sequenceIndex;

    // Start is called before the first frame update
    void Start()
    {
        sequenceIndex = 0;
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void ActivateSequence()
    {
        cinemaMode = true;
        currentSequence = listOfSequences[sequenceIndex];
        currentSequence.Interact();
    }

    public void DeactivateSequence()
    {
        cinemaMode = false;
        sequenceIndex++;
    }

    public void OnNextDialogueLine()
    {
        if (currentSequence != null && cinemaMode == true)
        {
            currentSequence.NextLine();
        }
    }
}