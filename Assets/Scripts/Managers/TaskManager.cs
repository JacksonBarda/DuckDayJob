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
    public int currentDay = 0;
    public int maxDays = 7;
    public int minTasksPerDay = 3;
    public int maxTasksPerDay = 5;

    private void Start()
    {
        InitializeTasks();
    }

    private void Update()
    {
        CheckCompletedTasks();
        ChangeWorld();
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

        // Add tasks to each day's list
        // You can populate tasksByDay[i] with instances of Interactable subclasses
    }

    private void CheckCompletedTasks()
    {
        List<Interactable> currentDayTasks = tasksByDay[currentDay];
        foreach (Interactable task in currentDayTasks)
        {
            if (!task.isCompleted && task.hasFailed)
            {
                ChangeWorldOnTaskFailure(task);
            }
        }
    }

    private void ChangeWorld()
    {
        // Change the world based on the number of completed tasks
        // You can add your own logic here to modify the world as needed
        int completedTasks = CountCompletedTasks();
        if (completedTasks == 1)
        {

        }
        else if (completedTasks == 2)
        {

        }
        // Add more conditions for other completed tasks
    }

    private int CountCompletedTasks()
    {
        int completedTasks = 0;
        List<Interactable> currentDayTasks = tasksByDay[currentDay];
        foreach (Interactable task in currentDayTasks)
        {
            if (task.isCompleted)
            {
                completedTasks++;
            }
        }
        return completedTasks;
    }

    private void ChangeWorldOnTaskFailure(Interactable task)
    {
        task.hasFailed = false;

    }
}

