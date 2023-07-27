using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text location;
    [SerializeField]
    private TMP_Text day;
    [SerializeField]
    private TMP_Text time;

    private int dayNumber = 1;
    private int hour = 9;
    private string meridiem = "a.m.";

    // Start is called before the first frame update
    void Start()
    {
        location.text = "Lobby";
        day.text = "Day " + dayNumber;
        time.text = hour + ":00 " + meridiem;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLocation(string newLocation)
    {
        location.text = newLocation;
    }

    public void UpdateDay()
    {
        dayNumber++;
        day.text = "Day " + dayNumber;
    }

    public void UpdateTime(int addHours)
    {
        hour += addHours;
        if (hour > 12)
        {
            hour = hour - 12;
            if (meridiem.Equals("a.m."))
            {
                meridiem = "p.m.";
            }
            else
            {
                meridiem = "a.m.";
            }
        }
        time.text = hour + ":00 " + meridiem;
    }
}
