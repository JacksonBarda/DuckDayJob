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
    public GameObject correctDialogue;
    public GameObject incorrectDialogue;
    private bool dialoguesActive = false;
    private string enteredNumber = "";
    private int playerCoins = 20;
    public List<GameObject> itemCosts;
    public string[] itemArray;


    public override void Interact()
    {
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
        AudioManager.Instance.PlayMusic("VendingAmbience");
        PlayerMove.puzzleMode = true;
    }

    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override void Complete()
    {

        Debug.Log("Complete");
    }

    private void Start()
    {
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
        AudioManager.Instance.PlaySFX("SFX_VendingButton");
    }

    private void Enter()
    {
        //Debug.Log("Entered number: " + enteredNumber);
        int itemIndex = int.Parse(enteredNumber.Substring(0, 1)) - 1;

        if (enteredNumber.Length > 2 || itemIndex < 0 || itemIndex > 3) //invalid number
        {
            StartCoroutine(DisplayMessage("ERROR"));
        }
        else //valid number
        {
            //Debug.Log("Enter() itemIndex: " + itemIndex);
            PurchaseItem(itemIndex);
        }
        
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
		AudioManager.Instance.PlayMusic("Lobby");
		PlayerMove.puzzleMode = false;

		if (lastBoughtItem != "" || lastBoughtItem != null)
        {
            dialoguesActive = true;
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
        //foreach (string item in itemArray)
        //{
        //    int thing = 0;
        //    Debug.Log(thing + ": " + item);
        //    thing++;
        //}
        //Debug.Log("done");
    }
    private void PurchaseItem(int itemIndex)
    {


        StartCoroutine(DisplayMessage("SUCCESS"));
        //playerCoins -= 5;
        UpdateDisplay();
        //itemCosts[itemIndex].SetActive(false);
        //Debug.Log(itemIndex);
        Debug.Log("Purchased item: " + itemArray[itemIndex]);
        lastBoughtItem = itemArray[itemIndex];

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

        if (lastBoughtItem == correctItem) //if correct item bought, hide wrong item dialogue
        {
            //incorrectDialogue.GetComponent<DialogueTool>().Complete();
            incorrectDialogue.SetActive(false);

            correctDialogue.SetActive(true);
        }
        else if (lastBoughtItem != correctItem)
        {
            //correctDialogue.GetComponent<DialogueTool>().Complete();
            correctDialogue.SetActive(false);

            incorrectDialogue.SetActive(true);
        }
    }
}
