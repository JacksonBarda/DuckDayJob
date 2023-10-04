using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static ReadDialogueData;
using UnityEditor;
using Enums;
using Unity.VisualScripting;

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
    private Image Profile2;
    [SerializeField]
    private TMP_Text Name;
    [SerializeField]
    private TMP_Text Dialogue;
    [SerializeField]
    private GameObject DialogueManager;
    [SerializeField]
    private GameObject Options;
    [SerializeField]
    private string scene;
    [SerializeField]
    private bool increasePriority;
    [SerializeField]
    private GameObject optionButtonPrefab;


    public List<DialogStruct> DialogueList = new List<DialogStruct>();

    public List<DialogStruct> talkAgainList = new List<DialogStruct>();

    private List<List<DialogStruct>> responseList = new List<List<DialogStruct>>();

    private List<DialogStruct> refDialogueList = new List<DialogStruct>();

    private List<GameObject> buttonList = new List<GameObject>();

    public int index = 0;

    private bool talkAgain;

    private bool inOptionDialog;

    // Start is called before the first frame update
    void Start()
    {
        talkAgain = false;
        inOptionDialog = false;
        /*
        refDialogueList = DialogueManager.GetComponent<ReadDialogueData>().DialogList;
        foreach (DialogStruct DialogueRow in refDialogueList)
        {
            if (DialogueRow.scene == scene)
            {
                DialogueList.Add(DialogueRow);
            }
        }
        */
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setList()
    {
        refDialogueList = DialogueManager.GetComponent<ReadDialogueData>().DialogList;
        foreach (DialogStruct DialogueRow in refDialogueList)
        {
            if (DialogueRow.scene.Contains(scene) == true)
            {
                if (DialogueRow.talkAgain == false)
                {
                    DialogueList.Add(DialogueRow);
                }
                else
                {
                    talkAgainList.Add(DialogueRow);
                }
                
            }
        }

    }

    public override void Action()
    {
        // happens when you fade out and in
        // don't need it but keep it
        throw new System.NotImplementedException();

    }

    public override void Interact()
    {
        Debug.Log("Talked");
        index = 0;
        // as you press w, it does this
        MainDisplay.SetActive(false);
        DialogueDisplay.SetActive(true);

        // disable user input
        player.puzzleMode = true;

        // set first line
        DialogueManager.GetComponent<ReadDialogueData>().DialogTool = this.gameObject;
        setDialogueUI();
    }

    public override void Finished()
    {
        // call it anytime inside the function and have your stuff that finished the interact, like close all the dialogue and bring back the main UI
        // below is placeholder if there is no code. If there is code, can delete
        ResetTool();

        if (increasePriority)
        {
            DialogueManager.GetComponent<ReadDialogueData>().priority++;
            DialogueManager.GetComponent<ReadDialogueData>().setDialogueTools();
        }

        if (inOptionDialog)
        {
            if (talkAgain)
            {
                talkAgainList = refDialogueList;
            }
            else
            {
                DialogueList = refDialogueList;
            }
            inOptionDialog = false;
        }

        // only sets talk again if there is a talk again dialogue. otherwise, if talk with the duck again, will play the same dialogue
        if (talkAgainList.Count != 0)
        {
            talkAgain = true;
        }
        index = 0;

    }

    public void ResetTool()
    {
        MainDisplay.SetActive(true);
        DialogueDisplay.SetActive(false);
        player.puzzleMode = false;
        if (!inOptionDialog)
        {
            this.gameObject.SetActive(false);
            this.gameObject.SetActive(true);
        }
        responseList.Clear();
        buttonList.Clear();


    }

    public bool setOptions()
    {
        bool hadOption = false;
        responseList.Clear();
        if (talkAgain)
        {
            Options.SetActive(true);

            if (talkAgainList[index].options)
            {
                refDialogueList = talkAgainList;
                hadOption = true;
                setProfile();
                setName();
                setLine();
                for (int i = 1; i <= talkAgainList[index].optionNumber; i++)
                {
                    GameObject newButton = Instantiate(optionButtonPrefab, Options.transform);
                    newButton.GetComponent<OptionButtonSetUp>().optionNumber = i;
                    buttonList.Add(newButton);
                    List<DialogStruct> optionList = new List<DialogStruct>();
                    foreach (DialogStruct optionDialog in talkAgainList)
                    {
                        if (optionDialog.optionNumber == i && optionDialog.scene.Contains("Response") == true)
                        {
                            optionList.Add(optionDialog);
                        }
                        if (optionDialog.optionNumber == i && optionDialog.scene.Contains("Option") == true)
                        {
                            newButton.GetComponentInChildren<TMP_Text>().text = optionDialog.dialogue;
                        }
                    }
                    newButton.GetComponent<OptionButtonSetUp>().OptionResponseList = optionList;
                    newButton.GetComponent<OptionButtonSetUp>().originalDialogueTool = this.gameObject;
                    responseList.Add(optionList);
                }
            }

        }
        else
        {
            Options.SetActive(true);

            if (DialogueList[index].options)
            {
                refDialogueList = DialogueList;
                hadOption = true;
                setProfile();
                setName();
                setLine();
                for (int i = 1; i <= DialogueList[index].optionNumber; i++)
                {
                    GameObject newButton = Instantiate(optionButtonPrefab, Options.transform);
                    newButton.GetComponent<OptionButtonSetUp>().optionNumber = i;
                    buttonList.Add(newButton);
                    List<DialogStruct> optionList = new List<DialogStruct>();
                    foreach (DialogStruct optionDialog in DialogueList)
                    {
                        if (optionDialog.optionNumber == i && optionDialog.scene.Contains("Response") == true)
                        {
                            optionList.Add(optionDialog);
                        }
                        if (optionDialog.optionNumber == i && optionDialog.scene.Contains("Option") == true)
                        {
                            newButton.GetComponentInChildren<TMP_Text>().text = optionDialog.dialogue;
                        }
                    }
                    newButton.GetComponent<OptionButtonSetUp>().OptionResponseList = optionList;
                    newButton.GetComponent<OptionButtonSetUp>().originalDialogueTool = this.gameObject;
                    responseList.Add(optionList);
                }
            }

        }
        return hadOption;
    }
    public void setProfile()
    {
        if (talkAgain)
        {
            switch (talkAgainList[index].align)
            {
                case Alignment.Left:
                    Profile.gameObject.SetActive(true);
                    Profile2.gameObject.SetActive(false);
                    Profile.sprite = DialogueManager.GetComponent<ReadDialogueData>().ProfileImages[talkAgainList[index].profileNumber - 1];
                    break;

                case Alignment.Right:
                    Profile.gameObject.SetActive(false);
                    Profile2.gameObject.SetActive(true);
                    Profile2.sprite = DialogueManager.GetComponent<ReadDialogueData>().ProfileImages[talkAgainList[index].profileNumber - 1];
                    break;

                default:
                    Profile.gameObject.SetActive(true);
                    Profile2.gameObject.SetActive(false);
                    Profile.sprite = DialogueManager.GetComponent<ReadDialogueData>().ProfileImages[talkAgainList[index].profileNumber - 1];
                    break;
            }
        }
        else
        {
            switch (DialogueList[index].align)
            {
                case Alignment.Left:
                    Profile.gameObject.SetActive(true);
                    Profile2.gameObject.SetActive(false);
                    Profile.sprite = DialogueManager.GetComponent<ReadDialogueData>().ProfileImages[DialogueList[index].profileNumber - 1];
                    break;

                case Alignment.Right:
                    Profile.gameObject.SetActive(false);
                    Profile2.gameObject.SetActive(true);
                    Profile2.sprite = DialogueManager.GetComponent<ReadDialogueData>().ProfileImages[DialogueList[index].profileNumber - 1];
                    break;

                default:
                    Profile.gameObject.SetActive(true);
                    Profile2.gameObject.SetActive(false);
                    Profile.sprite = DialogueManager.GetComponent<ReadDialogueData>().ProfileImages[DialogueList[index].profileNumber - 1];
                    break;
            }
        }
        
    }
    public void setName()
    {
        if (talkAgain)
        {
            Name.text = talkAgainList[index].name;
            switch (talkAgainList[index].align)
            {
                case Alignment.Left:
                    Name.alignment = TextAlignmentOptions.Left;
                    break;

                case Alignment.Right:
                    Name.alignment = TextAlignmentOptions.Right;
                    break;

                default:
                    Name.alignment = TextAlignmentOptions.Left;
                    break;
            }
        }
        else
        {
            Name.text = DialogueList[index].name;
            switch (DialogueList[index].align)
            {
                case Alignment.Left:
                    Name.alignment = TextAlignmentOptions.Left;
                    break;

                case Alignment.Right:
                    Name.alignment = TextAlignmentOptions.Right;
                    break;

                default:
                    Name.alignment = TextAlignmentOptions.Left;
                    break;
            }
        }
        
    }
    public void setLine()
    {
        if (talkAgain)
        {
            Dialogue.text = talkAgainList[index].dialogue;
            switch (talkAgainList[index].fontStyle)
            {
                case FontSelectStyle.Normal:
                    Dialogue.fontStyle = FontStyles.Normal;
                    break;

                case FontSelectStyle.Italics:
                    Dialogue.fontStyle = FontStyles.Italic;
                    break;

                default:
                    Dialogue.fontStyle = FontStyles.Normal;
                    break;

            }
        }
        else
        {
            Dialogue.text = DialogueList[index].dialogue;
            switch (DialogueList[index].fontStyle)
            {
                case FontSelectStyle.Normal:
                    Dialogue.fontStyle = FontStyles.Normal;
                    break;

                case FontSelectStyle.Italics:
                    Dialogue.fontStyle = FontStyles.Italic;
                    break;

                default:
                    Dialogue.fontStyle = FontStyles.Normal;
                    break;

            }
        }
        

    }

    public void setDialogueUI()
    {
        if (talkAgain)
        {
            if (index > talkAgainList.Count - 1)
            {
                Finished();
            }
            else
            {
                if (!setOptions())
                {
                    setProfile();
                    setName();
                    setLine();
                }

            }
        }
        else
        {
            if (index > DialogueList.Count - 1)
            {
                Finished();
            }
            else
            {
                if (!setOptions())
                {
                    setProfile();
                    setName();
                    setLine();
                }

            }
        }
        
    }

    public void setSelectedOptionDialogue(GameObject selectedButton)
    {
        Options.SetActive(false);
        inOptionDialog = true;
        DialogueList = responseList[selectedButton.GetComponent<OptionButtonSetUp>().optionNumber - 1];
        // need to delete option buttons
        foreach (GameObject button in buttonList)
        {
            Destroy(button);
        }
        ResetTool();
    }

}
