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
    [SerializeField]
    private Transform cameraTransfrom;

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
                    SetInteractionPopupLoc();
                    break;
                case string x when x.Contains("MainDuck"):
                    interactableName = PlayerPrefs.GetString("playerName") != null ? PlayerPrefs.GetString("playerName"): "MainDuck";
                    SetInteractionPopupLoc();
                    break;
                case string x when x.Contains("Eggwin"):
                    interactableName = "Eggwin";
                    SetInteractionPopupLoc();
                    break;
                case string x when x.Contains("Donald"):
                    interactableName = "Donald";
                    SetInteractionPopupLoc();
                    break;
                case string x when x.Contains("Quackson"):
                    interactableName = "Quackson";
                    SetInteractionPopupLoc();
                    break;
                case string x when x.Contains("ElonDuck"):
                    interactableName = "Elon Duck";
                    SetInteractionPopupLoc();
                    break;
                case string x when x.Contains("Janitor"):
                    interactableName = "Janitor";
                    SetInteractionPopupLoc();
                    break;
                case string x when x.Contains("NPC"):
                    interactableName = "NPC";
                    SetInteractionPopupLoc();
                    break;
                case string x when x.Contains("Door"):
                    DoorInteract door = (DoorInteract)PlayerMove.GetInteractable();
                    interactableName = door.endRoom.ToString();
                    SetInteractionPopupLocForDoor();
                    break;
                case string x when x.Contains("BathroomStalls"):
                    interactableName = "Empty Stall";
                    SetInteractionPopupLoc();
                    break;
                case string x when x.Contains("InteractCad"):
                    interactableName = "Computer";
                    SetInteractionPopupLoc();
                    break;
                case string x when x.Contains("InteractVendingMachine"):
                    interactableName = "Vending Machine";
                    SetInteractionPopupLoc();
                    break;
                case string x when x.Contains("DeskDragger"):
                    interactableName = "Duckette's Computer";
                    SetInteractionPopupLoc();
                    break;
                case string x when x.Contains("EmailDecrypt"):
                    interactableName = "Elon Duck's Computer";
                    SetInteractionPopupLoc();
                    break;
                default: interactableName = "Interact";
                    break;
            }


            InteractionPopup.transform.LookAt(cameraTransfrom, Vector3.up);
            InteractionPopup.transform.eulerAngles = new Vector3(InteractionPopup.transform.eulerAngles.x, InteractionPopup.transform.eulerAngles.y +180f , InteractionPopup.transform.eulerAngles.z);
            InteractionPopup.SetActive(true);
            interactableText.text = interactableName;

        }
        else
        {
            InteractionPopup.SetActive(false);
        }
    }
    private void SetInteractionPopupLoc()
    {
        InteractionPopup.transform.position = new Vector3(PlayerMove.GetInteractable().transform.position.x,
                                                    Mathf.Clamp(PlayerMove.GetInteractable().transform.position.y, 2f, 5f) + 1f,
                                                    Mathf.Clamp(PlayerMove.GetInteractable().transform.position.z, -5f, -1f));
    }
    private void SetInteractionPopupLocForDoor()
    {
        InteractionPopup.transform.position = new Vector3(PlayerMove.GetInteractable().transform.position.x,
                                                    PlayerMove.GetInteractable().transform.position.y,
                                                    PlayerMove.GetInteractable().transform.position.z);
    }
}
