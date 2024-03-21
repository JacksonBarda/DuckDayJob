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
    public string[] itemArray;


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
        //Debug.Log("PurchaseItem() itemIndex: " + itemIndex);
        if (itemIndex >= 0 && playerCoins >= 5)
        {
            StartCoroutine(DisplayMessage("SUCCESS"));
            playerCoins -= 5;
            UpdateDisplay();
            //itemCosts[itemIndex].SetActive(false);
            //Debug.Log(itemIndex);
            Debug.Log("Purchased item: " + itemArray[itemIndex]);
        }
        else
        {
            StartCoroutine(DisplayMessage("NO FUNDS"));
            //Debug.Log("Not enough coins or invalid item index: " + itemIndex);
        }
    }

    private void UpdateDisplay()
    {
        displayText.text = enteredNumber;
        playerCoinsText.text = "" + playerCoins;
    }
}
