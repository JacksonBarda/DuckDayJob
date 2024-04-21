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
    [SerializeField]
    private PlayerMove PlayerMove;
    [SerializeField]
    private GameObject InteractionPopup;
    [SerializeField]
    private TMP_Text interactableText;

    public int dayNumber = 1;
    public DayEnum dayOrNight = DayEnum.Day;
    private int hour = 9;
    private string meridiem = "a.m.";
    private Locations startingLocation = Locations.LOBBY;
    private string interactableName;

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
        SetInteractPopupText();

    }

    public void UpdateLocation(string newLocation)
    {
        location.text = newLocation;
    }

    // when update day, set priority in dialgoeu manager back to 0
    // also need a task code that can trigger priority increase as well
    public void UpdateDay()
    {
        dayNumber++;
        day.text = "Day " + dayNumber;
    }
    public void SetNightTime()
    {
        hour = 5;
        meridiem = "p.m.";
    }
    public void SetMorningTime()
    {
        hour = 8;
        meridiem = "a.m.";
        UpdateDay();
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

    private void SetInteractPopupText()
    {
        if (PlayerMove.GetInteractable() != null)
        {
            //Debug.Log("The Interactable: " + interactableName);

            switch (PlayerMove.GetInteractable().name)
            {
                case string x when x.Contains("Duckette"):
                    interactableName = "Duckette";
                    break;
                case string x when x.Contains("Eggwin"):
                    interactableName = "Eggwin";
                    break;
                case string x when x.Contains("Donald"):
                    interactableName = "Donald";
                    break;
                case string x when x.Contains("Quackson"):
                    interactableName = "Quackson";
                    break;
                case string x when x.Contains("ElonDuck"):
                    interactableName = "Elon Duck";
                    break;
                case string x when x.Contains("Janitor"):
                    interactableName = "Janitor";
                    break;
                case string x when x.Contains("NPC"):
                    interactableName = "NPC";
                    break;
                case string x when x.Contains("Door"):
                    interactableName = "Door";
                    break;
                case string x when x.Contains("BathroomStalls"):
                    interactableName = "Empty Stall";
                    break;
                case string x when x.Contains("InteractCad"):
                    interactableName = "Computer";
                    break;
                case string x when x.Contains("InteractVendingMachine"):
                    interactableName = "Vending Machine";
                    break;
                case string x when x.Contains("DeskDragger"):
                    interactableName = "Duckette's Computer";
                    break;
                case string x when x.Contains("EmailDecrypt"):
                    interactableName = "Elon Duck's Computer";
                    break;
                default: interactableName = "Interact";
                    break;
            }
            InteractionPopup.SetActive(true);
            interactableText.text = interactableName;
        }
        else
        {
            InteractionPopup.SetActive(false);
        }
    }
}
