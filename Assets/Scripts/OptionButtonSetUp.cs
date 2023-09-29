using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static ReadDialogueData;

public class OptionButtonSetUp : MonoBehaviour
{
    public int optionNumber;
    public List<DialogStruct> OptionResponseList = new List<DialogStruct>();
    public GameObject originalDialogueTool;

    public void selectOption()
    {
        originalDialogueTool.GetComponent<DialogueTool>().setSelectedOptionDialogue(this.gameObject);
        originalDialogueTool.GetComponent<DialogueTool>().Interact();
    }

}
