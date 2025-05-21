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
    public ReadDialogueData DialogueManager;
    public Camera mainCamera;
    public Camera cinematicCamera;
    public List<GameObject> listOfNormalSprites;
    private List<bool> spritesOriginalStates = new List<bool>();

    // Start is called before the first frame update
    void Start()
    {
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

        foreach (GameObject sprite in listOfNormalSprites)
        {
            spritesOriginalStates.Add(sprite.activeSelf);
            sprite.SetActive(false);
        }
    }

    public void DeactivateSequence()
    {
        cinemaMode = false;
        mainCamera.gameObject.SetActive(true);
        cinematicCamera.gameObject.SetActive(false);

        int i = 0;
        foreach (GameObject sprite in listOfNormalSprites)
        {
            sprite.SetActive(spritesOriginalStates[i]);
            i++;
        }
        spritesOriginalStates.Clear();
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
