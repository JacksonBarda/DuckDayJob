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


    public GameObject deathScreen;
    private int health;
    public int maxDays = 7;

    public int dayCount = 0;
    private int count = 0;
    private int day = 1;
    private List<Interactable> currentDay;

    public delegate void OnTaskComplete(Interactable _task);
    public static OnTaskComplete onTaskComplete;

    public delegate void OnTaskFailed(Interactable _task);
    public static OnTaskComplete onTaskFailed;

    public static TaskManager TMInstance;

    private void Start()
    {
        health = 3;
        InitializeTasks();
        onTaskComplete += TaskCompleted;
        onTaskComplete += TaskFailed;
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

    private void TaskFailed(Interactable _task)
    {
        _task.player.puzzleMode = false;
        _task.puzzleUI.SetActive(false);
        _task.mainUI.SetActive(true);
        health -= 1;
        if(health == 0)
        {
            OnDeath();
        }
        SaveGame();
    }

    private void OnDeath()
    {
        deathScreen.SetActive(true);
    }

    private void TaskCompleted(Interactable _task)
    {
        //Change day time here
        _task.player.puzzleMode = false;
        _task.puzzleUI.SetActive(false);
        _task.mainUI.SetActive(true);
        _task.gameObject.SetActive(false);
        count++;

        ChangeDay();
        SaveGame();
    }
    private void ChangeDay()
    {
        if(count > currentDay.Count)
        {
            dayCount++;
            currentDay = tasksByDay[dayCount];
            
            SaveGame();
            LoadDay();
        }
        //
        
    }

    private void LoadDay()
    {
        deathScreen.SetActive(false);
        currentDay = tasksByDay[dayCount];

    }

    private void SaveGame()
    {
        PlayerPrefs.SetInt("dayCount", dayCount);
        PlayerPrefs.SetInt("Count", count);
        PlayerPrefs.SetInt("Health", health);
        PlayerPrefs.Save();
    }
}

