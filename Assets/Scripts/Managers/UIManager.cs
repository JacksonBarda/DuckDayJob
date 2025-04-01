using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Enums;
using System.Runtime.CompilerServices;
using System.Threading;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance { get; private set; }
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
	private GameObject interactionPopuphold;
    public static GameObject InteractionPopup;
    [SerializeField]
    private TMP_Text interactableText;
    [SerializeField]
    private Transform cameraTransfrom;
    [SerializeField]
    private List<GameObject> taskList;
    [SerializeField]
    private List<GameObject> progressDucks;
	[SerializeField]
	private GameObject SaveProgressNotifText;

    public int dayNumber = 1;
    public DayEnum dayOrNight = DayEnum.Day;
    private int hour = 9;
    private string meridiem = "a.m.";
    private Locations startingLocation = Locations.LOBBY;
    private string interactableName;

    // Start is called before the first frame update
    void Start()
    {
		InteractionPopup = interactionPopuphold;
		// Check if there is already an instance of UIManager
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject); // Destroy this if it's a duplicate
			return;
		}

		Instance = this; // Assign the instance to this object

		// Optionally, you can use this line to preserve the UIManager across scene changes
		DontDestroyOnLoad(gameObject);
		setLocation(startingLocation);
        day.text = "Day " + dayNumber;
        time.text = hour + ":00 " + meridiem;

		SaveProgressNotifText.SetActive(true);
		SaveProgressNotifText.GetComponent<Text>().color = new Color(1,1,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        SetInteractPopupText();

    }
    public void SetTaskListText(String newTask, int count, GameObject interact, bool completed)
    {

		if (string.IsNullOrEmpty(newTask))
		{
			Debug.LogError("newTask is null or empty.");
			return;
		}

		// Validate task index
		if (count < 0 || count >= taskList.Count)
		{
			Debug.LogError($"Invalid index {count}. taskList has {taskList.Count} items.");
			return;
		}

		GameObject task = taskList[count];

		// Validate task GameObject
		if (task == null)
		{
			Debug.LogError($"Task at index {count} is null. Check taskList initialization.");
			return;
		}

		// Validate TextMeshPro component
		var textMeshPro = task.GetComponent<TextMeshProUGUI>();
		if (textMeshPro == null)
		{
			Debug.LogError($"Task GameObject at index {count} does not have a TextMeshPro component.");
			return;
		}

		// Update task text logic
		if (interact.activeSelf)
		{			

            if (completed)
            {
				textMeshPro.text = "<s>" + textMeshPro.text + "</s>";
            }
            else
            {
				textMeshPro.SetText(newTask);
			}

		}
		else if (newTask == textMeshPro.text)
		{
			textMeshPro.text = "<s>" + textMeshPro.text + "</s>";
		}
	}
    public void ClearTaskList()
    {
        foreach (var task in taskList)
        {
            var textMeshPro = task.GetComponent<TextMeshProUGUI>();
			textMeshPro.SetText("");
		}
	}
    public void SetProgressBar(int _count)
    {
        foreach(GameObject GO in progressDucks)
        {
            GO.SetActive(false);
        }
        progressDucks[_count].SetActive(true);

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
    int minuteCount;
	public void UpdateTime(float addHours)
	{

		// Convert the added time to minutes
		int totalMinutesToAdd = Mathf.RoundToInt(addHours * 60);

		// Calculate the new time in minutes
		int currentMinutes = (hour % 12) * 60 + (meridiem == "p.m." ? 720 : 0); // Convert hour and meridiem to total minutes
		currentMinutes += totalMinutesToAdd + minuteCount;

		// Wrap around if the time exceeds 24 hours (1440 minutes)
		currentMinutes %= 1440;

		// Calculate the new hour and minute values
		int newHour = currentMinutes / 60;
		int newMinute = currentMinutes % 60;
        minuteCount = newMinute;
		// Determine the meridiem (AM/PM)
		if (newHour >= 12)
		{
			meridiem = "p.m.";
		}
		else
		{
			meridiem = "a.m.";
		}

		// Convert hour to 12-hour format
		hour = newHour % 12;
		if (hour == 0) hour = 12; // Midnight or Noon should display as 12

		// Update the displayed time
		time.text = $"{hour}:{newMinute:00} {meridiem}";
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
            case Locations.VENTILATION:
                room = "VENTILATION";
                break;

            default:
                room = "NONE";
                break;
            
        }

        UpdateLocation(room);
    }

    public void SetInteractPopupText()
    {
        bool playerName = false;


			if (PlayerMove.GetInteractable() != null && !PlayerMove.puzzleMode)
			{
				//Debug.Log("UIManager: GetInteractable(): " + PlayerMove.GetInteractable());
				if (PlayerMove.GetInteractable().customPopupName != null && PlayerMove.GetInteractable().customPopupName != "")
				{
					interactableName = PlayerMove.GetInteractable().customPopupName;
					SetInteractionPopupLoc();
				}
				else
				{
					//Debug.Log("The Interactable: " + interactableName);

					switch (PlayerMove.GetInteractable().name)
					{
						case string x when x.Contains("Duckette"):
							interactableName = "Duckette";
							SetInteractionPopupLoc();
							break;
						case string x when x.Contains("MainDuck"):
							interactableName = PlayerPrefs.GetString("playerName") != null ? PlayerPrefs.GetString("playerName") : "MainDuck";
							playerName = true;
							//SetInteractionPopupLoc();
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
						case string x when x.Contains("Door") || x.Contains("Vent"):
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
						case string x when x.Contains("InteractVM"):
							interactableName = "Vending Machine";
							SetInteractionPopupLoc();
							break;
						case string x when x.Contains("DucketteHack"):
							interactableName = "Duckette's Computer";
							SetInteractionPopupLoc();
							break;
						case string x when x.Contains("EmailDecrypt"):
							interactableName = "Elon Duck's Computer";
							SetInteractionPopupLoc();
							break;
						default:
							interactableName = "Interact";
							break;
					}




				}
				if (!playerName)
				{
					InteractionPopup.SetActive(true);
					InteractionPopup.transform.LookAt(cameraTransfrom, Vector3.up);
					InteractionPopup.transform.eulerAngles = new Vector3(InteractionPopup.transform.eulerAngles.x, InteractionPopup.transform.eulerAngles.y + 180f, InteractionPopup.transform.eulerAngles.z);

					interactableText.text = interactableName;
				}

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

	public IEnumerator NotifySaveProgress()
    {
		Debug.Log("UIManager: NotifySaveProgress()");
		SaveProgressNotifText.GetComponent<Text>().color = new Color(1, 1, 1, 1);

		yield return new WaitForSeconds(3f);

		while (SaveProgressNotifText.GetComponent<Text>().color.a > 0)
        {
			Color currentColor = SaveProgressNotifText.GetComponent<Text>().color;
			currentColor.a -= .3f * Time.deltaTime;
			SaveProgressNotifText.GetComponent<Text>().color = currentColor;
			//Debug.Log("UIManager: currentColor = " + currentColor);
			yield return null;
		}
	}
}
