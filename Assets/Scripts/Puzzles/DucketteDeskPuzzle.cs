using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DucketteDeskPuzzle : Interactable
{
    [SerializeField]
    private TMP_InputField password;
    [SerializeField]
    private GameObject DeskUI;
    [SerializeField]
    private GameObject ComputerUI;
    [SerializeField]
    private GameObject tryAgainText;

    private string correctPassword = "F3ath3rF1irt";

    public override void Interact()
    {
        base.Interact();
        //player.puzzleMode = true;
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
        counted = false;
    }

    public override void Action()
    {


    }

    public override void Complete()
    {
        base.Complete();

    }
    public void OnToComputerPressed()
    {
        DeskUI.SetActive(false);
        ComputerUI.SetActive(true);
    }
    public void OnReturnPressed()
    {
        DeskUI.SetActive(true);
        ComputerUI.SetActive(false);
    }
    public void OnEnterPressed()
    {
        if (password.text.ToLower() == correctPassword.ToLower())
        {
            tryAgainText.SetActive(false);
            Complete();
        }
        else
        {
            tryAgainText.SetActive(true);
        }
    }
}
