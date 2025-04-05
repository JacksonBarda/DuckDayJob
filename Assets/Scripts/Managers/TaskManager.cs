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
    public bool isNight;

    [SerializeField]
    private GameManager GM;

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


    // ----------- CHEATS --------------- //

    public void CheatSkipToDay(int dayInput)
    {
        for (int i = 0; i < dayInput-1; i++)    // days
        {
            for (int j = 0; j < 6; j++)         //parts
            {
                foreach (Interactable task in tasksByDay[i].GetInteractables((PartIdentifier)j)) //get all tasks in Day "i" in Part "j"
                {
                    CheatCompleteTask(task);
                }
            }
            
        }
    }

    public void CheatCompleteTask(Interactable task)
    {
        if (task.activatePostPuzzle && task.objectToActivate != null)
        {
            foreach (GameObject obj in task.objectToActivate)
            {
                obj.gameObject.SetActive(!obj.gameObject.activeSelf);
            }
        }
        if (task.GetComponent<CinematicSequenceTool>() != null)
        {
            Debug.Log("TaskManager: Checking shots for interactables...");
            foreach (Shot cineShot in task.GetComponent<CinematicSequenceTool>().getListOfShots())
            {
                if (cineShot.sceneChangeInteractable != null)
                {
                    cineShot.sceneChangeInteractable.Interact();
                    Debug.Log("TaskManager: Skipping shot interactable - " + cineShot.sceneChangeInteractable);
                }
            }
        }
        task.Complete();
        task.gameObject.SetActive(false);
    }

    public PartIdentifier GetCurrentPart()
    {
        return currentPt;
    }

    // --------------------------------- //

    private void Awake()
	{
		if (TMInstance == null)
		{
			TMInstance = this; // Assign the instance of TaskManager to the static field
		}
		else
		{
			Destroy(gameObject); // Prevent duplicate instances if you don't want them
		}
	}
	private void Start()
    {

        health = 3;
        InitializeTasks();
        onTaskComplete += TaskCompleted;
        onTaskFailed += TaskFailed;
        if (tasksByDay[day - 1].GetInteractables(currentPt)[count] != null && tasksByDay[day - 1].GetInteractables(currentPt)[count].forcePlay)
        {
            Debug.Log("TaskManager: Force play " + tasksByDay[day - 1].GetInteractables(currentPt)[count] + "---------------------------------------");
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


        foreach (Interactable task in tasksByDay[day - 1].GetInteractables(currentPt))          // for every task in the current part
        {
            if ((task.isCompleted || task.isOptional) && !task.counted)                             // if the task is completed or optional and hasnt been counted yet
            {
                count++;                                                                                // update variable count
                task.counted = true;
				UIManager.Instance.UpdateTime(UnityEngine.Random.Range(0.1f, 0.5f));
			}
        }
        if (count >= tasksByDay[day - 1].GetInteractables(currentPt).Count)                     // if all tasks of the current part are done
        {
            foreach (Interactable task in tasksByDay[day - 1].GetInteractables(currentPt))          // for each task in the current part
            {
                if(task.stayActive != true)
                {
                    task.gameObject.SetActive(false);                                               // hide task, unless stayActive is true
                }

            }
			UIManager.Instance.ClearTaskList();
			UIManager.Instance.UpdateTime(UnityEngine.Random.Range(0.4f, 0.8f));
			currentPt++;

            // =========================== CURRENT PART UPDATED ===============================================================================================================

            while (countCheck)
            {
                if(tasksByDay[day - 1].GetInteractables(currentPt).Count == 0)                      // if part is empty
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
            if (task.isVisibleOnStart && task.gameObject.activeSelf == false)
            {
                task.gameObject.SetActive(true);
            }
			String textToShow = task.name;
			if (task.taskName != null &&  task.taskName.Length > 0)
            {
				textToShow = task.taskName;
				UIManager.Instance.SetTaskListText(textToShow, count, task.gameObject, task.isCompleted);
			}

            

			count++;
		}
        count = holdCount;

	}

    private void ChangeDay()
    {
        int listLoc = 0;
        foreach(GameObject dayObject in duckSpritesForDays[day].spriteToMove)
        {
            
            dayObject.transform.position = duckSpritesForDays[day].dayLocation[listLoc].transform.position;
            //Debug.Log("TaskManager.cs: Moved duck " + dayObject + "to " + duckSpritesForDays[day].dayLocation[listLoc]);
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
        currentPt = GetPart(ptCount);
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

        GM.OnSaveGameState();
    }
    public Interactable findCurrentInteractable()
    {
        return tasksByDay[day - 1].GetInteractables(currentPt)[count];

	}
	public void AddInteractableToDay(Interactable newInteractable, int dayCount, int pt)
	{

		if (tasksByDay.Count >= dayCount) // Adjusting for zero-based indexing
		{
			DayTask dayTask = tasksByDay[dayCount - 1];

			// Add the interactable to the correct part list based on the pt value
			switch (pt)
			{
				case 1:
					dayTask.pt1.Add(newInteractable);
					break;
				case 2:
					dayTask.pt2.Add(newInteractable);
					break;
				case 3:
					dayTask.pt3.Add(newInteractable);
					break;
				case 4:
					dayTask.pt4.Add(newInteractable);
					break;
				case 5:
					dayTask.pt5.Add(newInteractable);
					break;
				case 6:
					dayTask.pt6.Add(newInteractable);
					break;
				default:
					Debug.LogWarning("Invalid part identifier: " + pt);
					return;
			}

			Debug.Log("New Interactable added to part " + pt + " of day " + dayCount);
		}
		else
		{
			Debug.LogWarning("Invalid day count: " + dayCount);
		}
	}
	private PartIdentifier GetPart(int _ptCount)
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

    public List<Interactable> getCompletedTasksOfPart()
    {
        List<Interactable> currentTaskList = tasksByDay[day - 1].GetInteractables(GetCurrentPart());
        List<Interactable> completedTasks = new List<Interactable>();
        foreach (Interactable task in currentTaskList)
        {
            if (task.isCompleted) completedTasks.Add(task);
        }

        return completedTasks;
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

