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
    private TMP_Text Tasklist;
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
    [SerializeField]
    private Slider SLDR_Progress;
    //[SerializeField]
    //private GameObject nextButton;

    private string duckname;

    // Dialogue counter
    private int quackscompleted;
    private string previousName;

    public List<DialogStruct> DialogueList = new List<DialogStruct>();

    public List<DialogStruct> talkAgainList = new List<DialogStruct>();

    private List<List<DialogStruct>> responseList = new List<List<DialogStruct>>();

    private List<DialogStruct> refDialogueList = new List<DialogStruct>();

    private List<DialogStruct> retrievalDialogueList = new List<DialogStruct>();

    private List<GameObject> buttonList = new List<GameObject>();

    public int index = 0;

    private bool talkAgain;

    private bool inOptionDialog;
    [SerializeField]
    private int roundNum = 1;

    // Retrieval Variables
    public int Round1Answer;
    public int Round2Answer;
    private bool correct = false;
    private int holdIndex = 0;
    private int attempts = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(taskName == null) 
        {
            taskName = scene;
        }

        talkAgain = false;
        inOptionDialog = false;
        //nextButton.SetActive(true);
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
        //nextButton.SetActive(true);
        // as you press w, it does this
        MainDisplay.SetActive(false);
        DialogueDisplay.SetActive(true);

        // disable user input
        PlayerMove.puzzleMode = true;

        // set first line
        DialogueManager.GetComponent<ReadDialogueData>().DialogTool = this.gameObject;
        setDialogueUI();

    }
   

    public override void Complete()
    {
        base.Complete();
        // call it anytime inside the function and have your stuff that finished the interact, like close all the dialogue and bring back the main UI
        // below is placeholder if there is no code. If there is code, can delete
        ResetTool();
        roundNum = 1;
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
        //Tasklist.SetText(taskName);
        SLDR_Progress.value++;
    }

   

    public bool setOptions()
    {
        bool hadOption = false;
        responseList.Clear();
        buttonList.Clear();
        
        if (talkAgain)
        {
            Options.SetActive(true);

            if (talkAgainList[index].options)
            {
                refDialogueList = talkAgainList;
                hadOption = true;
                //nextButton.SetActive(false);
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
                            //optionDialog.scene.Contains("Round" + roundNum) == true //THERE IS ALWAYS 2 ROUNDS
                        {
                            if(activatePostPuzzle)
                            {
                                if (optionDialog.scene.Contains("Round" + roundNum) == true)
                                {
                                    optionList.Add(optionDialog);
                                }
                            }else
                            {
                                optionList.Add(optionDialog);
                            }
                            
                        }
                        if (optionDialog.optionNumber == i && optionDialog.scene.Contains("Option") == true)
                        {
                            if (activatePostPuzzle)
                            {
                                if (optionDialog.scene.Contains("Round" + roundNum) == true)
                                {
                                    newButton.GetComponentInChildren<TMP_Text>().text = optionDialog.dialogue;
                                }
                            }
                            else
                            {
                                newButton.GetComponentInChildren<TMP_Text>().text = optionDialog.dialogue;
                            }
                            
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
            if (activatePostPuzzle)
            {
                if (roundNum == 2)
                {
                    retrievalDialogueList = refDialogueList;
                }
            }
            if (DialogueList[index].options)
            {
                //holdIndex = index; //IndexOf or find where it is in the original dialoge list
                Debug.Log(holdIndex);
                if (attempts == 0)
                {
                    refDialogueList = DialogueList;
                }
                hadOption = true;
                //nextButton.SetActive(false);
                setProfile();
                setName();
                setLine();
                if (activatePostPuzzle)
                {
                    if (roundNum == 2)
                    {
                        DialogueList = retrievalDialogueList;
                    }
                }

                for (int i = 1; i <= 4; i++)
                {
                    //DialogueList[index].optionNumber <-- NEED TO CHANGE LATER FOR RETRIEVAL BECAUSE ONLY SHOWING ONE
                    GameObject newButton = Instantiate(optionButtonPrefab, Options.transform);
                    newButton.GetComponent<OptionButtonSetUp>().optionNumber = i;
                    buttonList.Add(newButton);
                    List<DialogStruct> optionList = new List<DialogStruct>();
                    foreach (DialogStruct optionDialog in DialogueList)
                    {
                        if (optionDialog.optionNumber == i && optionDialog.scene.Contains("Response") == true)
                        //optionDialog.scene.Contains("Round" + roundNum) == true //THERE IS ALWAYS 2 ROUNDS
                        {
                            
                            if (activatePostPuzzle)
                            {
                                if (optionDialog.scene.Contains("Round" + roundNum) == true)
                                {
                                    optionList.Add(optionDialog);
                                    
                                }
                            }
                            else
                            {
                                optionList.Add(optionDialog);
                            }

                        }
                        if (optionDialog.optionNumber == i && optionDialog.scene.Contains("Option") == true)
                        {
                            if (activatePostPuzzle)
                            {
                                if (optionDialog.scene.Contains("Round" + roundNum) == true)
                                {
                                    newButton.GetComponentInChildren<TMP_Text>().text = optionDialog.dialogue;
                                }
                            }
                            else
                            {
                                newButton.GetComponentInChildren<TMP_Text>().text = optionDialog.dialogue;
                            }

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
            Name.text = replaceMainDuck(talkAgainList[index].name);
            duckname = talkAgainList[index].name;
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
            Name.text = replaceMainDuck(DialogueList[index].name);
            duckname = DialogueList[index].name;
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
        if (duckname.Equals(previousName))
        {
            quackscompleted++;
        }
        else
        {
            quackscompleted = 0;
            previousName = duckname;
        }

    }
    public void setLine()
    {
        if (talkAgain)
        {
            Dialogue.text = replaceMainDuck(talkAgainList[index].dialogue);
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
            Dialogue.text = replaceMainDuck(DialogueList[index].dialogue);
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
                Complete();
            }
            else
            {
                if (!setOptions())
                {
                    setProfile();
                    setName();
                    setLine();
                    if (quackscompleted == 0)
                    {
                        AudioManager.Instance.PlayDialogue(duckname);
                    }
                }

            }
        }
        else
        {
            if (index > DialogueList.Count - 1)
            {
                Debug.Log("End of List");
                if (activatePostPuzzle && !correct)
                {
                    
                    DialogueList = refDialogueList;
                    if (roundNum == 1)
                    {
                        index = holdIndex;
                    }
                    else
                    {
                        index = 0;
                    }
                    setDialogueUI();
                }
                else
                {
                    Complete();
                }
                
            }
            else
            {
                if (!setOptions())
                {
                    setProfile();
                    setName();
                    setLine();
                    if (quackscompleted == 0)
                    {
                        AudioManager.Instance.PlayDialogue(duckname);
                    }
                }
                

            }
        }

    }

    public void setSelectedOptionDialogue(GameObject selectedButton)
    {
        Options.SetActive(false);
        inOptionDialog = true;
        holdIndex = DialogueList.IndexOf(refDialogueList[0]); //<---------- WE NEED TO HOLD THE INDEX OF THE RESPONSE
        //nextButton.SetActive(true);
        DialogueList = responseList[selectedButton.GetComponent<OptionButtonSetUp>().optionNumber - 1];
        // need to delete option buttons
        foreach (GameObject button in buttonList)
        {
            Destroy(button);
        }
        if (activatePostPuzzle)
        {
            
            
            //roundNum++;
            if (roundNum == 1)
            {
                if (selectedButton.GetComponent<OptionButtonSetUp>().optionNumber == Round1Answer)
                {
                    roundNum++;
                    correct = true;
                    Debug.Log("Correct");
                    attempts = 0;
                }
                else
                {
                    correct = false;
                    attempts++;
                }
            }
            if (roundNum == 2)
            {
                if (selectedButton.GetComponent<OptionButtonSetUp>().optionNumber == Round2Answer)
                {
                    roundNum++;
                    correct = true;
                    Debug.Log("Correct");
                    attempts = 0;
                }
                else
                {
                    //DialogueList = retrievalDialogueList;
                    correct = false;
                    attempts++;
                }
            }
            /*
            if (roundNum == 1)
            {
                if (selectedButton.GetComponent<OptionButtonSetUp>().optionNumber == Round1Answer)
                {
                    roundNum++;
                    correct = true;
                }
                else
                {
                    correct = false;
                }
            }
            else
            {
                if (selectedButton.GetComponent<OptionButtonSetUp>().optionNumber == Round2Answer)
                {
                    roundNum++;
                    correct = true;
                }
                else
                {
                    correct = false;
                    // DON'T HAVE INDEX--, INSTEAD HAVE ANOTHER VARIABLE FOR 
                }
            }
            */
        }

        ResetTool();
    }

    public void ResetTool()
    {
        MainDisplay.SetActive(true);
        DialogueDisplay.SetActive(false);
        PlayerMove.puzzleMode = false;
        //nextButton.SetActive(true);
        if (!inOptionDialog)
        {
            this.gameObject.SetActive(false);
            this.gameObject.SetActive(true);
        }
        responseList.Clear();
        buttonList.Clear();

    }

    // Return function that returns the text with player name whenever Main Duck is mentioned
    public string replaceMainDuck(string originalText)
    {
        string name = PlayerPrefs.GetString("playerName");
        string newText = originalText.Replace("Main Duck", name);
        return newText;
    }

}
