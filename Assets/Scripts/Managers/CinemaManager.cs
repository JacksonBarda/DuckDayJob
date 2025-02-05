using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemaManager : MonoBehaviour
{
    [SerializeField]
    private CinematicSequenceTool currentSequence;
    [SerializeField]
    public bool cinemaMode = false;
    [SerializeField]
    private List<CinematicSequenceTool> listOfSequences;
    [SerializeField]
    private int sequenceIndex;
    public ReadDialogueData DialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        sequenceIndex = 0;
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void ActivateSequence(CinematicSequenceTool sequence)
    {
        cinemaMode = true;
        currentSequence = sequence;
    }

    public void DeactivateSequence()
    {
        cinemaMode = false;
        sequenceIndex++;
    }

    public void OnNextDialogueLine()
    {
        Debug.Log("clicky ------------------");
        if (currentSequence != null && cinemaMode == true)
        {
            currentSequence.NextLine();
        }
    }

    public void IterateDialogue()
    {
        Debug.Log("iterated -----------------");
        DialogueManager.nextLine(true);
    }
}
