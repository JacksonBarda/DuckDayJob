using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestVendingMachine : Interactable
{
    [SerializeField]
    private GameObject puzzleUI;
    [SerializeField]
    private GameObject mainUI;

    public Text displayText;

    public Text playerCoinsText;
    public Button[] numberButtons;
    public Button enterButton;
    public Button deleteButton;
    public Button exitButton;

    private string enteredNumber = "";
    private int playerCoins = 20;
    public List<GameObject> itemCosts;

    public override void Interact()
    {
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
    }

    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override void Finished()
    {
        puzzleUI.SetActive(false);
        mainUI.SetActive(true);
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
    }

    private void AddNumber(string number)
    {
        enteredNumber += number;
        UpdateDisplay();
    }

    private void Enter()
    {

        int itemIndex = int.Parse(enteredNumber) - 1;
        enteredNumber = null;
        enteredNumber = "";
        UpdateDisplay();
        PurchaseItem(itemIndex);
    }
    private void VMexit()
    {
        enteredNumber = null;
        enteredNumber = "";
        UpdateDisplay();
        puzzleUI.SetActive(false);
        mainUI.SetActive(true);
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
            itemCosts[itemIndex].SetActive(false);
            Debug.Log("Purchased item: " + itemIndex);
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
