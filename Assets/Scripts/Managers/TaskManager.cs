using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TaskManager;
[System.Serializable]
public class TaskManager : MonoBehaviour
{

	[SerializeField]
	public List<DailyGameObjects> duckSpritesForDays = new List<DailyGameObjects>();
    public int count = 0;
    public List<DayTask> tasksByDay = new List<DayTask>();
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

    

    public GameObject deathScreen;
    private int health;
    public int maxDays = 7;

    //public int dayCount = 0;
    public int ptCount = 0;

    public int day = 1;



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
        if (tasksByDay[day - 1].GetInteractables(currentPt)[count] != null && tasksByDay[day - 1].GetInteractables(currentPt)[count].forcePlay)
        {
            tasksByDay[day - 1].GetInteractables(currentPt)[count].Interact();
        }
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

        foreach (Interactable task in tasksByDay[day - 1].GetInteractables(currentPt))
        {
            if (task.isVisibleOnStart)
            {
                task.gameObject.SetActive(true);
            }

        }

    }

    private void TaskFailed(Interactable _task)
    {
        PlayerMove.puzzleMode = false;
        if(_task.puzzleUI !=null)
            _task.puzzleUI.SetActive(false);
        if (_task.mainUI != null)
            _task.mainUI.SetActive(true);
        //health -= 1;
        if(health == 0)
        {
            OnDeath();
        }
        //uiManager.UpdateTime(1);
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
				UIManager.Instance.UpdateTime(UnityEngine.Random.Range(0.1f, 0.5f));
			}
        }
        if (count >= tasksByDay[day - 1].GetInteractables(currentPt).Count)
        {
            foreach (Interactable task in tasksByDay[day - 1].GetInteractables(currentPt))
            {
                if(task.stayActive != true)
                {
                    task.gameObject.SetActive(false);
                }

            }
			UIManager.Instance.ClearTaskList();
			UIManager.Instance.UpdateTime(UnityEngine.Random.Range(0.4f, 0.8f));
			currentPt++;

			while (countCheck)
            {
                if(tasksByDay[day - 1].GetInteractables(currentPt).Count == 0)
                {

                    currentPt++;
                    if((int)currentPt >= 5)
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
            foreach(GameObject GO in tasksByDay[day - 1].GetGameObjects(currentPt))
            {

                GO.SetActive(!GO.activeSelf);

            }
            count = 0;
            SaveGame();

        }

		UIManager.Instance.SetProgressBar((int)currentPt);
		PlayerMove.puzzleMode = false;
        if (_task.puzzleUI != null)
            _task.puzzleUI.SetActive(false);
        if (_task.mainUI != null)
            _task.mainUI.SetActive(true);
        if (!_task.repeatable)
        {
            _task.gameObject.SetActive(false);
        }
        // if (tasksByDay[day - 1].GetInteractables(currentPt)[count] != null && tasksByDay[day - 1].GetInteractables(currentPt)[count].forcePlay)
        //{
        //    tasksByDay[day - 1].GetInteractables(currentPt)[count].Interact();
        //} 


        int holdCount = count;
        count = 0;
		foreach (Interactable task in tasksByDay[day - 1].GetInteractables(currentPt))
        {
			String textToShow = task.name;
			if (task.taskName != null &&  task.taskName.Length > 0)
            {
				textToShow = task.taskName;
			}

            
            UIManager.Instance.SetTaskListText(textToShow, count, task.gameObject, task.isCompleted);
			count++;
		}
        count = holdCount;

	}

    private void ChangeDay()
    {
        int listLoc = 0;
        foreach(GameObject dayObject in duckSpritesForDays[day - 1].spriteToMove)
        {
            
            dayObject.transform.position = duckSpritesForDays[day - 1].dayLocation[listLoc].transform.position;
            listLoc++;
		}
        currentPt = 0;
        day++;
        SaveGame();
        LoadDay();  
    }

    private void LoadDay()
    {
        day = PlayerPrefs.GetInt("dayCount", day);
        ptCount = PlayerPrefs.GetInt("ptCount", ptCount);
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
    public Interactable findCurrentInteractable()
    {
        return tasksByDay[day - 1].GetInteractables(currentPt)[count];

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
public struct DailyGameObjects
{
    public List<GameObject> spriteToMove;
    public List<Transform> dayLocation;

}
[System.Serializable]
public struct DayTask
{
    [Tooltip ("Put the actual day - 1 EX: day1 = 0 for the days attribute")]

    public int day;
	public List<GameObject> gameObjectToShowHidePT1;
	public List<Interactable> pt1;
	public List<GameObject> gameObjectToShowHidePT2;
	public List<Interactable> pt2;
	public List<GameObject> gameObjectToShowHidePT3;
	public List<Interactable> pt3;
	public List<GameObject> gameObjectToShowHidePT4;
	public List<Interactable> pt4;
	public List<GameObject> gameObjectToShowHidePT5;
	public List<Interactable> pt5;
	public List<GameObject> gameObjectToShowHidePT6;
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
	public List<GameObject> GetGameObjects(PartIdentifier point)
	{
		switch (point)
		{
			case PartIdentifier.Pt1:
				return gameObjectToShowHidePT1;
			case PartIdentifier.Pt2:
				return gameObjectToShowHidePT2;
			case PartIdentifier.Pt3:
				return gameObjectToShowHidePT3;
			case PartIdentifier.Pt4:
				return gameObjectToShowHidePT4;
			case PartIdentifier.Pt5:
				return gameObjectToShowHidePT5;
			case PartIdentifier.Pt6:
				return gameObjectToShowHidePT6;
			default:
				return new List<GameObject>();
		}
	}
}

