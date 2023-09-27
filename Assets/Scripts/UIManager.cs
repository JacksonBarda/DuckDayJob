using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Enums;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text location;
    [SerializeField]
    private TMP_Text day;
    [SerializeField]
    private TMP_Text time;
    [SerializeField]
    private GameObject MainUI;
    [SerializeField]
    private GameObject DialogueUI;

    private int dayNumber = 1;
    private int hour = 9;
    private string meridiem = "a.m.";
    private Locations startingLocation = Locations.LOBBY;

    // Start is called before the first frame update
    void Start()
    {
        setLocation(startingLocation);
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

    public void setLocation(Locations currentLocation)
    {
        string room = "";

        switch (currentLocation)
        {
            case Locations.LOBBY:
                room = "LOBBY";
                break;

            case Locations.CLOSET:
                room = "CLOSET";
                break;

            case Locations.OFFICE:
                room = "OFFICE";
                break;

            case Locations.BREAKROOM:
                room = "BREAKROOM";
                break;

            case Locations.MEETINGROOM:
                room = "MEETING ROOM";
                break;

            case Locations.BATHROOM:
                room = "BATHROOM";
                break;

            case Locations.MAZE:
                room = "MAZE";
                break;

            case Locations.MANUFACTURING:
                room = "MANUFACTURING ROOM";
                break;

            case Locations.KILLINGFLOOR:
                room = "KILLING ROOM";
                break;

            case Locations.BOSSROOM:
                room = "BOSS'S OFFICE";
                break;

            default:
                room = "NONE";
                break;
            
        }

        UpdateLocation(room);
    }

}
