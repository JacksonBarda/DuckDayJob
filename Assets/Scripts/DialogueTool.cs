using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static ReadDialogueData;
using UnityEditor;

public class DialogueTool : Interactable
{
    // Notes from Jackson:
    // get game objects from the puzzles (like the box collider)
    // need tag interactable, but just copy boxes and keep everything and replace it with the sprite of the character
    [SerializeField]
    private GameObject MainDisplay;
    [SerializeField]
    private GameObject DialogueDisplay;
    [SerializeField]
    private Image Profile;
    [SerializeField]
    private TMP_Text Name;
    [SerializeField]
    private TMP_Text Dialogue;
    [SerializeField]
    private GameObject DialogueManager;
    [SerializeField]
    private string scene;
    

    private List<DialogStruct> DialogueList = new List<DialogStruct>();

    private List<DialogStruct> refDialogueList = new List<DialogStruct>();

    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        refDialogueList = DialogueManager.GetComponent<ReadDialogueData>().DialogList;
        foreach (DialogStruct DialogueRow in refDialogueList)
        {
            if (DialogueRow.scene == scene)
            {
                DialogueList.Add(DialogueRow);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Action()
    {
        // happens when you fade out and in
        // don't need it but keep it
        throw new System.NotImplementedException();

    }

    public override void Interact()
    {
        // as you press w, it does this
        MainDisplay.SetActive(false);
        DialogueDisplay.SetActive(true);

        // disable user input

        // set first line
        setDialogueUI();
    }

    protected override void Finished()
    {
        // call it anytime inside the function and have your stuff that finished the interact, like close all the dialogue and bring back the main UI
        // below is placeholder if there is no code. If there is code, can delete
        throw new System.NotImplementedException();
    }

    
    public void setProfile()
    {
        Profile.sprite = DialogueManager.GetComponent<ReadDialogueData>().ProfileImages[DialogueList[index].profileNumber - 1];
    }
    public void setName()
    {
        Name.text = DialogueList[index].name;
    }
    public void setLine()
    {
        Dialogue.text = DialogueList[index].dialogue;
    }

    public void setDialogueUI()
    {
        setProfile();
        setName();
        setLine();
    }

}
