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
    public Camera mainCamera;
    public Camera cinematicCamera;

    // Start is called before the first frame update
    void Start()
    {
        sequenceIndex = 0;
        cinematicCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        foreach (CinematicSequenceTool sequence in listOfSequences)
        {
            sequence.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void ActivateSequence(CinematicSequenceTool sequence)
    {
        cinemaMode = true;
        currentSequence = sequence;
        mainCamera.gameObject.SetActive(false);
        cinematicCamera.gameObject.SetActive(true);
    }

    public void DeactivateSequence()
    {
        cinemaMode = false;
        sequenceIndex++;
        mainCamera.gameObject.SetActive(true);
        cinematicCamera.gameObject.SetActive(false);
    }

    public void OnNextDialogueLine()
    {
        if (currentSequence != null && cinemaMode == true)
        {
            Debug.Log("clicky ------------------");
            currentSequence.NextLine();
        }
    }

    public void IterateDialogue()
    {
        Debug.Log("iterated -----------------");
        DialogueManager.nextLine(true);
    }
}
