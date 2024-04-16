using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DucketteDeskPuzzle : Interactable
{
    [SerializeField]
    private InputField password;

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
        if(password.text.ToLower() != correctPassword.ToLower())
        {

        }

    }

    public override void Complete()
    {
        base.Complete();

    }
}
