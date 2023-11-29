using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField]
    private List<List<Interactable>> tasksByDay = new List<List<Interactable>>();
    [SerializeField]
    private List<Interactable> interactablesDay1;
    [SerializeField]
    private List<Interactable> interactablesDay2;
    [SerializeField]
    private List<Interactable> interactablesDay3;
    [SerializeField]
    private List<Interactable> interactablesDay4;
    [SerializeField]
    private List<Interactable> interactablesDay5;
    [SerializeField]
    private List<Interactable> interactablesDay6;
    [SerializeField]
    private List<Interactable> interactablesDay7;



    
    public int maxDays = 7;

    public int dayCount = 0;
    private int count = 0;
    private int day = 1;
    private List<Interactable> currentDay;


    public static TaskManager TMInstance;

    private void Start()
    {
        InitializeTasks();
    }


    private void InitializeTasks()
    {

        tasksByDay.Add(interactablesDay1);
        tasksByDay.Add(interactablesDay2);
        tasksByDay.Add(interactablesDay3);
        tasksByDay.Add(interactablesDay4);
        tasksByDay.Add(interactablesDay5);
        tasksByDay.Add(interactablesDay6);
        tasksByDay.Add(interactablesDay7);
        currentDay = tasksByDay[dayCount];


    }

    private void TaskFailed()
    {
        //Potentially change day here

    }

    private void TaskCompleted(Interactable _task)
    {
        count++;
        ChangeDay();
    }
    private void ChangeDay()
    {
        if(count > currentDay.Count)
        {
            dayCount++;
            currentDay = tasksByDay[dayCount];
            
            SaveGame();
            LoadNextDay();
        }
        //
        //Change day time here
    }

    private void LoadNextDay()
    {
        //Reload scene with next day counter 
    }

    private void SaveGame()
    {
        //PlayerPrefs save the 
    }
}

