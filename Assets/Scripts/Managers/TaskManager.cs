using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TaskManager;

public class TaskManager : MonoBehaviour
{
    public int count = 0;
    private List<DayTask> tasksByDay = new List<DayTask>();
    [SerializeField]
    public DayTask Day1;
    [SerializeField]
    public DayTask Day2;
    [SerializeField]
    public DayTask Day3;
    [SerializeField]
    public DayTask Day4;
    [SerializeField]
    public DayTask Day5;
    [SerializeField]
    public DayTask Day6;
    [SerializeField]
    public DayTask Day7;

    [SerializeField]
    private List<GameObject> duckSpritesForDays = new List<GameObject>();

    public GameObject deathScreen;
    private int health;
    public int maxDays = 7;

    //public int dayCount = 0;
    public int ptCount = 0;

    private int day = 1;


    public UIManager uiManager;

    public delegate void OnTaskComplete(Interactable _task);
    public static OnTaskComplete onTaskComplete;

    public delegate void OnTaskFailed(Interactable _task);
    public static OnTaskComplete onTaskFailed;

    public static TaskManager TMInstance;
    public enum PartIdentifier
    {
        Pt1,
        Pt2,
        Pt3,
        Pt4,
        Pt5,
        Pt6
    }
    private PartIdentifier currentPt = PartIdentifier.Pt1;

    private void Start()
    {

        health = 3;
        InitializeTasks();
        onTaskComplete += TaskCompleted;
        onTaskFailed += TaskFailed;
    }


    private void InitializeTasks()
    {

        tasksByDay.Add(Day1);
        tasksByDay.Add(Day2);
        tasksByDay.Add(Day3);
        tasksByDay.Add(Day4);
        tasksByDay.Add(Day5);
        tasksByDay.Add(Day6);
        tasksByDay.Add(Day7);
        ptCount = 0;
        day = 1;


    }

    private void TaskFailed(Interactable _task)
    {
        PlayerMove.puzzleMode = false;
        if(_task.puzzleUI !=null)
            _task.puzzleUI.SetActive(false);
        if (_task.mainUI != null)
            _task.mainUI.SetActive(true);
        health -= 1;
        if(health == 0)
        {
            OnDeath();
        }
        uiManager.UpdateTime(1);
        SaveGame();
        
    }

    private void OnDeath()
    {
        deathScreen.SetActive(true);
    }

    private void TaskCompleted(Interactable _task)
    {
        bool countCheck = true;


        foreach (Interactable task in tasksByDay[day - 1].GetInteractables(currentPt))
        {
            if (task.isCompleted && !task.counted)
            {
                count++;
                task.counted = true;
            }
        }
        if (count >= tasksByDay[day - 1].GetInteractables(currentPt).Count)
        {
            foreach (Interactable task in tasksByDay[day - 1].GetInteractables(currentPt))
            {
                task.gameObject.SetActive(false);
            }
            currentPt++;
            while(countCheck)
            {
                if(tasksByDay[day - 1].GetInteractables(currentPt).Count == 0)
                {
                    currentPt++;
                    if((int)currentPt >= 4)
                    {
                        ChangeDay();
                        currentPt = PartIdentifier.Pt1;
                    }
                }
                else
                {
                    countCheck = false;
                }
            }
            foreach (Interactable task in tasksByDay[day - 1].GetInteractables(currentPt))
            {
                if (task.isVisibleOnStart)
                {
                    task.gameObject.SetActive(true);
                }

            }
            count = 0;
            SaveGame();

        }
        else
        {
                
        }

        uiManager.UpdateTime(1);
        //Change day time here
        PlayerMove.puzzleMode = false;
        if (_task.puzzleUI != null)
            _task.puzzleUI.SetActive(false);
        if (_task.mainUI != null)
            _task.mainUI.SetActive(true);
        if (!_task.repeatable)
        {
            _task.gameObject.SetActive(false);
        }
        if (tasksByDay[day - 1].GetInteractables(currentPt)[count] != null && tasksByDay[day - 1].GetInteractables(currentPt)[count].forcePlay)
        {
            tasksByDay[day - 1].GetInteractables(currentPt)[count].Interact();
        }
             
    }

    private void ChangeDay()
    {
        if (duckSpritesForDays.Count >= day)
        {
            duckSpritesForDays[day-1].gameObject.SetActive(false);
        }
        day++;
        if (duckSpritesForDays.Count > day)
        {
            duckSpritesForDays[day - 1].gameObject.SetActive(true);
        }
        currentPt = 0;
            
        SaveGame();
        LoadDay();  
    }

    private void LoadDay()
    {
        day = PlayerPrefs.GetInt("dayCount", day);
        ptCount =PlayerPrefs.GetInt("ptCount", ptCount);
        health = PlayerPrefs.GetInt("Health", health);
        deathScreen.SetActive(false);
        currentPt = GetCurrentPt(ptCount);
        foreach (Interactable task in tasksByDay[day - 1].GetInteractables(currentPt))
        {
            if (task.isVisibleOnStart)
            {
                task.gameObject.SetActive(true);
            }

        }

    }

    private void SaveGame()
    {
        ptCount = (int)currentPt;
        PlayerPrefs.SetInt("dayCount", day);
        PlayerPrefs.SetInt("ptCount", ptCount);
        PlayerPrefs.SetInt("Health", health);
        PlayerPrefs.Save();
    }
    private PartIdentifier GetCurrentPt(int _ptCount)
    {
        switch(_ptCount)
        {
            case 0:
                return PartIdentifier.Pt1;
                
            case 1:
                return PartIdentifier.Pt2;
                
            case 2:
                return PartIdentifier.Pt3;
                
            case 3:
                return PartIdentifier.Pt4;
               
            case 4:
                return PartIdentifier.Pt5;

            case 5:
                return PartIdentifier.Pt6;

            default:
                return PartIdentifier.Pt1;
                
        }
    }


}

[System.Serializable]
public struct DayTask
{
    [Tooltip ("Put the actual day - 1 EX: day1 = 0 for the days attribute")]
    public int day;
    public List<Interactable> pt1;
    public List<Interactable> pt2;
    public List<Interactable> pt3;
    public List<Interactable> pt4;
    public List<Interactable> pt5;
    public List<Interactable> pt6;

    public List<Interactable> GetInteractables(PartIdentifier point)
    {
        switch (point)
        {
            case PartIdentifier.Pt1:
                return pt1;
            case PartIdentifier.Pt2:
                return pt2;
            case PartIdentifier.Pt3:
                return pt3;
            case PartIdentifier.Pt4:
                return pt4;
            case PartIdentifier.Pt5:
                return pt5;
            case PartIdentifier.Pt6:
                return pt6;
            default:
                return new List<Interactable>();
        }
    }
}

