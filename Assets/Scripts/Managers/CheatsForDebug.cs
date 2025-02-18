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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CheatMenu.SetActive(!CheatMenu.activeSelf);
        }
    }
    
    public void SkipToDay()
    {
        int day = int.Parse(STD_Input.text);
        Debug.Log("CheatsForDebug: Skipping to day " + day + "...");
        TaskManager.CheatSkipToDay(day);
    }

    public void Teleport()
    {
        Debug.Log("CheatsForDebug: Teleporting player...");
        PlayerMove.CheatMoveToCustomLocation();
    }

    public void SkipNextTask()
    {
        List<Interactable> taskList = TaskManager.tasksByDay[TaskManager.day - 1].GetInteractables(TaskManager.GetCurrentPart());
        foreach (Interactable task in taskList)
        {
            if (task.isCompleted)
            {
                continue;
            }
            else
            {
                Debug.Log("CheatsForDebug: Skipping " + task + "...");
                task.Complete();
                break;
            }
        }
        
    }
}
