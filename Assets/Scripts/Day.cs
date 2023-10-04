using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

public class Day : MonoBehaviour
{
    [SerializeField]
    private Interactable[] tasks;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public int FindItemIndex(Interactable itemName)
    {
        // Loop through the array to find the index of the item
        for (int i = 0; i < tasks.Length; i++)
        {
            if (tasks[i] == itemName)
            {

                // Item found, return its index
                return i;
            }
        }

        // Item not found, return -1
        return -1;
    }

    public void RemoveItemAt(int index)
    {
        // Create a new array with one less element
        Interactable[] newArray = new Interactable[tasks.Length - 1];

        // Copy the elements before the index
        for (int i = 0; i < index; i++)
        {
            newArray[i] = tasks[i];
        }

        // Copy the elements after the index
        for (int i = index + 1; i < tasks.Length; i++)
        {
            newArray[i - 1] = tasks[i];
        }

        // Replace the original array with the new array
        tasks = newArray;
    }
}
