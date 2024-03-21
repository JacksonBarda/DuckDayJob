using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMItem : MonoBehaviour
{
    public string itemName { get; set; }
    //public int itemCost = 0;

    public VMItem(string name)//, int cost)
    {
        itemName = name;
        //itemCost = cost;
    }
}
