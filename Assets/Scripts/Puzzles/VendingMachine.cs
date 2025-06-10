using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachine : Interactable
{


    public Text displayText;

    public Text playerCoinsText;
    public Button[] numberButtons;
    public Button enterButton;
    public Button deleteButton;
    public Button exitButton;

    public string lastBoughtItem = "";
    public string correctItem = "";
    public DialogueTool reactionDialogue;     //alternate scene should be incorrect dialogue
    //private bool dialoguesActive = false;
    private string enteredNumber = "";
    //private int playerCoins = 20;
    public List<GameObject> itemCosts;
    public string[] itemArray;


    public override void Interact()
    {
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
        PlayerMove.puzzleMode = true;
    }

    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override void Complete()
    {
        base.Complete();
    }

    public override void Start()
    {
        base.Start();
        UpdateDisplay();
        for (int i = 0; i < numberButtons.Length; i++)
        {
            string number = (i).ToString();
            numberButtons[i].onClick.AddListener(() => AddNumber(number));
        }
        enterButton.onClick.AddListener(Enter);
        deleteButton.onClick.AddListener(VMremove);
        exitButton.onClick.AddListener(VMexit);

        itemArray = new string[]{
            "Duck Crunch",
            "Peepsi",
            "Energy Gooster",
            "Dr. Ducker"
        };
    }

    private void AddNumber(string number)
    {
        enteredNumber += number;
        UpdateDisplay();
        AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[3], SoundType.InteractableSFX, "ISFX_VendingButton");
    }

    private void Enter()
    {
        //Debug.Log("Entered number: " + enteredNumber);
        int itemIndex = int.Parse(enteredNumber.Substring(0, 1)) - 1;

        if (enteredNumber.Length != 2 || 
            itemIndex < 0 || 
            itemIndex > 3 ||
            int.Parse(enteredNumber.Substring(1, 1)) > 3 ||
            int.Parse(enteredNumber.Substring(1, 1)) < 1)       //invalid number
        {
            StartCoroutine(DisplayMessage("ERROR"));
        }
        else //valid number
        {
            //Debug.Log("Enter() itemIndex: " + itemIndex);
            PurchaseItem(itemIndex);
        }
        AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[3], SoundType.InteractableSFX, "ISFX_VendingButton");
    }
    
    public IEnumerator DisplayMessage(string message)
    {
        enteredNumber = message;
        UpdateDisplay();
        yield return new WaitForSeconds(1f);
        enteredNumber = "";
        UpdateDisplay();
    }

    private void VMexit()
    {
        enteredNumber = null;
        enteredNumber = "";
        UpdateDisplay();
		puzzleUI.SetActive(false);
		mainUI.SetActive(true);
		PlayerMove.puzzleMode = false;
        AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[3], SoundType.InteractableSFX, "ISFX_VendingButton");

        Debug.Log("VendingMachine.VMexit(): lastBoughtItem -  " + lastBoughtItem);

		if (lastBoughtItem != "" || lastBoughtItem != null)
        {
            //dialoguesActive = true;
            CheckForCorrectItem();
            Complete();
        }
        Debug.Log("VMExit");
    }
    private void VMremove()
    {
        enteredNumber = null;
        enteredNumber = "";
        UpdateDisplay();
    }

    private void PurchaseItem(int itemIndex)
    {
        StartCoroutine(DisplayMessage("SUCCESS"));

        UpdateDisplay();

        Debug.Log("Purchased item: " + itemArray[itemIndex]);
        lastBoughtItem = itemArray[itemIndex];
        AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[3], SoundType.InteractableSFX, "ISFX_VendingDropSnack");
    }

    private void UpdateDisplay()
    {
        displayText.text = enteredNumber;
        //playerCoinsText.text = "" + playerCoins;
    }

    private void CheckForCorrectItem()      
    {
        //dialogue tools should be included in task manager and dialogue manager as normal
        //

        if (reactionDialogue.isCompleted == false) reactionDialogue.gameObject.SetActive(true);

        if (lastBoughtItem == correctItem) //if correct item bought, hide wrong item dialogue
        {
            reactionDialogue.useAlternateScene = false;
        }

        else if (lastBoughtItem != correctItem)
        {
            reactionDialogue.useAlternateScene = true;
        }
    }
}
