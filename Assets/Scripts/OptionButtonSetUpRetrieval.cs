using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static ReadDialogueRetrieval;

public class OptionButtonSetUpRetrieval : MonoBehaviour
{
    public int optionNumber;
    public List<DialogStructRetrieval> OptionResponseList = new List<DialogStructRetrieval>();
    public GameObject originalRetrievaPuzzle;

    public void selectOption()
    {
        originalRetrievaPuzzle.GetComponent<RetrievalPuzzle>().setSelectedOptionDialogue(this.gameObject);
        originalRetrievaPuzzle.GetComponent<RetrievalPuzzle>().Interact();
    }

}
