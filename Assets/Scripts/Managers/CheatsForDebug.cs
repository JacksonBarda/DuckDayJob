using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CheatsForDebug : MonoBehaviour
{
    [SerializeField]
    private GameObject CheatMenu;
    public TaskManager TaskManager;
    public PlayerMove PlayerMove;

    [SerializeField]
    private TMP_InputField STD_Input;
    // Start is called before the first frame update
    void Start()
    {
        CheatMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CheatMenu.SetActive(!CheatMenu.activeSelf);
        }
    }
    
    public void SkipToDay()
    {
        TaskManager.CheatSkipToDay(int.Parse(STD_Input.text));
    }

    public void Teleport()
    {
        PlayerMove.CheatMoveToCustomLocation();
    }
}
