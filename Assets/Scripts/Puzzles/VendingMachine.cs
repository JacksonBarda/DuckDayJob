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

    private string enteredNumber = "";
    private int playerCoins = 20;
    public List<GameObject> itemCosts;
    public VMItem[] itemArray = new VMItem[3];

    public override void Interact()
    {
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
        AudioManager.Instance.PlayMusic("VendingAmbience");
    }

    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override void Complete()
    {

        //Location of Puzzle
        AudioManager.Instance.PlayMusic("Lobby");
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

        //create item array
        itemArray[0] = new VMItem("Duck Crunch");
        itemArray[1] = new VMItem("Peepsi");
        itemArray[2] = new VMItem("Energy Gooster");
        itemArray[3] = new VMItem("Dr. Ducker");

        foreach (VMItem item in itemArray)
        {
            Debug.Log(item.itemName);
        }
    }

    private void AddNumber(string number)
    {
        enteredNumber += number;
        UpdateDisplay();
        AudioManager.Instance.PlaySFX("SFX_VendingButton");
    }

    private void Enter()
    {
        Debug.Log(enteredNumber);
        if (enteredNumber.Length > 2) //invalid number
        {
            StartCoroutine(DisplayMessage("ERROR"));
        }
        else //valid number
        {
            int itemIndex = int.Parse(enteredNumber.Substring(0,1)) - 1;
            StartCoroutine(DisplayMessage("SUCCESS"));
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
    }
    private void VMremove()
    {
        enteredNumber = null;
        enteredNumber = "";
        UpdateDisplay();
    }
    private void PurchaseItem(int itemIndex)
    {

        if (itemIndex >= 0 && playerCoins >= 5)
        {
            playerCoins -= 5;
            UpdateDisplay();
            //itemCosts[itemIndex].SetActive(false);
            Debug.Log("Purchased item: " + itemArray[itemIndex].itemName);
        }
        else
        {
            Debug.Log("Not enough coins or invalid item index: " + itemIndex);
        }
    }

    private void UpdateDisplay()
    {
        displayText.text = enteredNumber;
        playerCoinsText.text = "" + playerCoins;
    }
}
