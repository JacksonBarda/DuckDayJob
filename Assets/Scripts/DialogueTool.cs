using System.Collections;
using System.Collections.Generic;
using System;
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
    private string gameScene;
    [SerializeField]
    private string gameSceneAlt;
    public bool useAlternateScene;
    [SerializeField]
    private bool retrieval;
    [SerializeField]
    public bool recordAnswer = false;

    [SerializeField]
    public List<InteractOnLine> InteractableOnLine;

    [SerializeField]
    private GameObject optionButtonPrefab;
    [SerializeField]
    private Slider SLDR_Progress;
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private GameObject charSprite;
    private GameObject DialogueIndicatorE;
    private GameObject DialogueIndicatorQ;
    [SerializeField]
    private bool DI_UseQuestionMark;
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Sprite customizedBackgroundImage;
    [SerializeField]
    private bool playAnim;
    [SerializeField]
    private int whichLine;
    [SerializeField]
    private PlayAnimation playAnimation;

	private string duckname;
    private int quackscompleted;
    private string previousName;

    public List<DialogStruct> DialogueList = new List<DialogStruct>();

    public List<DialogStruct> talkAgainList = new List<DialogStruct>();

    [SerializeField]
    private List<List<DialogStruct>> responseList = new List<List<DialogStruct>>();

    public List<DialogStruct> refDialogueList = new List<DialogStruct>();

    public List<DialogStruct> retrievalDialogueList = new List<DialogStruct>();

    private List<GameObject> buttonList = new List<GameObject>();

    public int index = 0;

    private bool talkAgain;

    private bool inOptionDialog;
    [SerializeField]
    private int roundNum = 1;

    // Retrieval Variables
    public List<int> correctAnswers;
    [SerializeField]
    private int givenAnswer;
    private bool correct = false;
    private int holdIndex = 0;
    private DialogStruct lastDialogue;
    private int attempts = 0;
    private bool originalFlip;

    [System.Serializable]
    public struct InteractOnLine
    {
        public Interactable thingToInteract;
        public int lineToInteract;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (taskName == null) 
        {
            //taskName = gameScene;
        }
        talkAgain = false;
        inOptionDialog = false;
        if (backgroundImage != null)
        {
            backgroundImage.enabled = false;
        }

        if (charSprite != null)
        {
            DialogueIndicatorE = charSprite.transform.GetChild(0).gameObject;
            DialogueIndicatorQ = charSprite.transform.GetChild(1).gameObject;
        }
    }

    public override void Start()
    {
        Debug.Log("DT.Start");

        if (useAlternateScene == true)
        {
            gameScene = gameSceneAlt;
            DialogueList.Clear();
            talkAgainList.Clear();
            setList();
        }

        if (charSprite != null)
        {
            if (DI_UseQuestionMark)
            {
                DialogueIndicatorE.SetActive(false);
                DialogueIndicatorQ.SetActive(true);
                //Debug.Log("DT: DI_E - FALSE; DI_Q - TRUE 1");
            }
            else
            {
                DialogueIndicatorE.SetActive(true);
                DialogueIndicatorQ.SetActive(false);
                //Debug.Log("DT: DI_E - TRUE; DI_Q - FALSE 2");
            }
        }
        base.Start();
    }

    private void OnDisable()
    {
        if (charSprite != null)
        {
            DialogueIndicatorE.SetActive(false);
            DialogueIndicatorQ.SetActive(false);
            //Debug.Log("DT: DI_E - FALSE; DI_Q - FALSE 3");
        }
    }

    public void setList()
    {
        refDialogueList = DialogueManager.GetComponent<ReadDialogueData>().DialogList;
        foreach (DialogStruct DialogueRow in refDialogueList)
        {
            if (DialogueRow.scene.Contains(gameScene) == true)
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
        retrievalDialogueList = DialogueList;
    }

    public override void Action()
    {
        // happens when you fade out and in
        // don't need it but keep it
        throw new System.NotImplementedException();
        

    }

    public override void Interact()
    {
        //Debug.Log("Talked");
        index = 0;

        // as you press w, it does this
        MainDisplay.SetActive(false);
        DialogueDisplay.SetActive(true);        

        // disable user input
        PlayerMove.puzzleMode = true;

        // set first line
        DialogueManager.GetComponent<ReadDialogueData>().DialogTool = this.gameObject;
        setDialogueUI();

        try
        {
            DialogueIndicatorE.SetActive(false);
            DialogueIndicatorQ.SetActive(false);
            Debug.Log("DT: DI_E - FALSE; DI_Q - FALSE 4");
        }
        catch (NullReferenceException) { 
        }
        
        if (charSprite != null)     // make NPD look at you upon interacting
        {
            originalFlip = charSprite.GetComponent<SpriteRenderer>().flipX;
            if (player.transform.position.x < gameObject.transform.position.x) charSprite.GetComponent<SpriteRenderer>().flipX = true;
            else charSprite.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
   

    public override void Complete()
    {

        // call it anytime inside the function and have your stuff that finished the interact, like close all the dialogue and bring back the main UI
        // below is placeholder if there is no code. If there is code, can delete
        ResetTool();
        roundNum = 1;


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
            DialogueIndicatorE.SetActive(false);
            DialogueIndicatorQ.SetActive(true);
            Debug.Log("DT: DI_E - FALSE; DI_Q - TRUE 5");
            talkAgain = true;
        }
        index = 0;

        if (charSprite != null) charSprite.GetComponent<SpriteRenderer>().flipX = originalFlip;

        base.Complete();
        //Tasklist.SetText(taskName);
        //SLDR_Progress.value++;
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
                                    newButton.GetComponentInChildren<TMP_Text>().text = noQuotationMarks(optionDialog.dialogue);
                                }
                            }
                            else
                            {
                                newButton.GetComponentInChildren<TMP_Text>().text = noQuotationMarks(optionDialog.dialogue);
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
            
            if (DialogueList[index].options)
            { 
                if (attempts == 0)
                {
                    refDialogueList = DialogueList;
                }
                hadOption = true;
                nextButton.interactable = false;
                setProfile();
                setName();
                setLine();
                lastDialogue = DialogueList[index];
                if (activatePostPuzzle)
                {
                    if (roundNum >= 2)
                    {
                        DialogueList = retrievalDialogueList;
                    }
                }

                for (int i = 1; i <= 4; i++)
                {
                    GameObject newButton = Instantiate(optionButtonPrefab, Options.transform);
                    newButton.GetComponent<OptionButtonSetUp>().optionNumber = i;
                    buttonList.Add(newButton);
                    List<DialogStruct> optionList = new List<DialogStruct>();
                    foreach (DialogStruct optionDialog in DialogueList)
                    {
                        if (optionDialog.optionNumber == i && optionDialog.scene.Contains("Response") == true)
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
                                    newButton.GetComponentInChildren<TMP_Text>().text = noQuotationMarks(optionDialog.dialogue);
                                }
                            }
                            else
                            {
                                newButton.GetComponentInChildren<TMP_Text>().text = noQuotationMarks(optionDialog.dialogue);
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

            if (talkAgainList[index].profileNumber == 0)
            {
                Profile.gameObject.SetActive(false);
                Profile2.gameObject.SetActive(false);
            }
        }
        else
        {
            if (DialogueList[index].profileNumber == 0)
            {
                Profile.gameObject.SetActive(false);
                Profile2.gameObject.SetActive(false);
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
        
    }
    public void setName()
    {
        if (talkAgain)
        {
            Name.text = replaceMainDuck(talkAgainList[index].name);
            duckname = talkAgainList[index].name;

            if (duckname.Equals("None"))
            {
                Name.text = "";

            }

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

            if (duckname.Equals("None"))
            {
                Name.text = "";

            }

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

                case FontSelectStyle.Bold:
                    Dialogue.fontStyle = FontStyles.Bold;
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

                case FontSelectStyle.Bold:
                    Dialogue.fontStyle = FontStyles.Bold;
                    break;

                default:
                    Dialogue.fontStyle = FontStyles.Normal;
                    break;

            }
        }
    }

    public void setBackground()
    {
        if (DialogueList[index].normalUI == false)
        {
            Debug.Log("Show Background");
            backgroundImage.sprite = customizedBackgroundImage;
            backgroundImage.enabled = true;
        }
        else
        {
            backgroundImage.enabled = false;
        }
    }

    public void setDialogueUI()
    {
        if(playAnim)
            if(index-1 == whichLine)
            {
                playAnimation.PlayAnimInteract(this);
                SetAnimMode();
            }
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
                    if(backgroundImage != null)
                    {
                        setBackground();
                    }

                    if (quackscompleted == 0)
                    {
                        //AudioManager.Instance.PlayDialogue(duckname);
                    }
                }

            }
        }
        else
        {
            if (index > DialogueList.Count - 1)
            {
                Debug.Log("DialogueTool: End of List");
                if (retrieval && !correct)
                {
                    
                    DialogueList = retrievalDialogueList;
                    index = holdIndex;
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
                    if (backgroundImage != null)
                    {
                        setBackground();
                    }
                    if (quackscompleted == 0)
                    {
                        //AudioManager.Instance.PlayDialogue(duckname);
                    }
                }
                

            }
        }
        Debug.Log("clicky");
        //Debug.Log("DialogueList: " + DialogueList);
        //Debug.Log("responseList: " + responseList);
        //Debug.Log("refDialogueList: " + refDialogueList);
        //Debug.Log("retrievalDialogueList: " + retrievalDialogueList);
    }

    public void setSelectedOptionDialogue(GameObject selectedButton)
    {
        Options.SetActive(false);
        inOptionDialog = true;
        holdIndex = DialogueList.IndexOf(lastDialogue); // NEW MODIFICATION FOR WHEN THERE ARE MULTIPLE LINES IN A RESPONSE
        nextButton.interactable = true;
        DialogueList = responseList[selectedButton.GetComponent<OptionButtonSetUp>().optionNumber - 1];
        // need to delete option buttons
        foreach (GameObject button in buttonList)
        {
            button.SetActive(false);
            Destroy(button,0.1f);
        }
        if (activatePostPuzzle)
        {
                if (selectedButton.GetComponent<OptionButtonSetUp>().optionNumber == correctAnswers[roundNum - 1])
                    {
                roundNum++;
                correct = true;
                Debug.Log("Correct");
                attempts = 0;
            }
            else
            {
                correct = false;
                Debug.Log("Wrong");
                attempts++;
            }

        }

        if (recordAnswer)
        {
            givenAnswer = selectedButton.GetComponent<OptionButtonSetUp>().optionNumber;
            Debug.Log("Given answer: " + givenAnswer);
        }
        nextButton.interactable = false;
        nextButton.interactable = true;
        ResetTool();
    }

    public int getOptionAnswer()
    {
        return givenAnswer;
    }

    public void ResetTool()
    {
        MainDisplay.SetActive(true);
        DialogueDisplay.SetActive(false);
        PlayerMove.puzzleMode = false;
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
        newText = noQuotationMarks(newText);
        return newText;
    }

    public string noQuotationMarks(string originalText)
    {
        string newText = originalText;
        if (originalText.StartsWith("\""))
        {
            newText = originalText.Substring(1, originalText.Length - 2);
        }
        return newText;
    }
    public void SetAnimMode()
    {
        nextButton.interactable = false;
		DialogueDisplay.SetActive(false);
	}
	public void SetDisplayMode()
	{
		nextButton.interactable = true;
		DialogueDisplay.SetActive(true);

	}
}
