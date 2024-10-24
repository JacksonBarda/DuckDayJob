using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Enums;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using Unity.IO.LowLevel.Unsafe;
//using static UnityEditor.Progress;

public class ReadDialogueData : MonoBehaviour
{
    [SerializeField]
    private GameObject UIManager;
    
    [SerializeField]
    private TextAsset DialogData;

    [Serializable]
    public struct DialogStruct
    {
        public string scene;
        public int day;
       // public int order_priority;
        public DayEnum dayOrNight;
        public string name;
        public int profileNumber;
        public string dialogue;
        public bool normalUI;
        public Alignment align;
        public FontSelectStyle fontStyle;
        public bool options;
        public int optionNumber;
        public bool talkAgain;

        public DialogStruct(string sceneString, int dayNumber, int order, DayEnum dayNight, string duck, 
            int profile, string dialog, bool uistyle, Alignment alignment, FontSelectStyle style, bool opt, int optNum, bool again)
        {
            scene = sceneString;
            day = dayNumber;
            //order_priority = order;
            dayOrNight = dayNight;
            name = duck;
            profileNumber = profile;
            dialogue = dialog;
            normalUI = uistyle;
            align = alignment;
            fontStyle = style;
            options = opt;  
            optionNumber = optNum;
            talkAgain = again;
           
        }

    }

    public List<DialogStruct> DialogList = new List<DialogStruct>();

    public List<Sprite> ProfileImages = new List<Sprite>();

    public GameObject DialogTool;

    //public int priority;

    public List<GameObject> DialogueToolsList = new List<GameObject>(); 

    // Start is called before the first frame update
    void Awake()
    {
        getData();

        // set dialogue list for each tool
        foreach (GameObject dTool in DialogueToolsList)
        {
            dTool.GetComponent<DialogueTool>().setList();
        }

        //DialogTool = DialogueToolsList[0];
       // DialogTool.GetComponent<DialogueTool>().Interact();

        // set which dialogue tools are active
        //priority = 1;
        //setDialogueTools();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void getData()
    {
        DialogList.Clear();
        string[] firstLinesInFile = DialogData.text.Split("\n");

        // remove first row because those are the headings
        int count = 0;
        List<string> LinesInFile = new List<string>();

        foreach (string row in firstLinesInFile)
        {
            if (count != 0)
            {
                LinesInFile.Add(row);
            }
            count++;
        }


        // sets the row into a proper struct to add to the dialogue lists
        foreach (string line in LinesInFile)
        {
            if (string.IsNullOrEmpty(line))
            {
                Debug.Log("Empty");

            }
            else
            {
                string[] getLine = line.Split("\t");  
                string scene = getLine[0];
                /*
                Debug.Log(line);
                int day = int.Parse(getLine[1]);
                int order = int.Parse(getLine[2]);
                DayEnum dayOrNight = (DayEnum)System.Enum.Parse(typeof(DayEnum), getLine[3]);
                string name = getLine[4];
                int profile = int.Parse(getLine[5]);
                string dialog = getLine[6];
                Debug.Log(getLine[7]);
                bool normalUI = bool.Parse(getLine[7]);
                Alignment align = (Alignment)System.Enum.Parse(typeof(Alignment), getLine[8]);
                FontSelectStyle style = (FontSelectStyle)System.Enum.Parse(typeof(FontSelectStyle), getLine[9]);
                bool options = bool.Parse(getLine[10]);
                int optNum = int.Parse(getLine[11]);
                bool talkAgain = bool.Parse(getLine[12]);
                */

                if (scene != "")
                {
                    DialogStruct dialogRow = new DialogStruct(getLine[0], int.Parse(getLine[1]), int.Parse(getLine[2]), (DayEnum)System.Enum.Parse(typeof(DayEnum), getLine[3]), getLine[4],
                    int.Parse(getLine[5]), getLine[6], bool.Parse(getLine[7]), (Alignment)System.Enum.Parse(typeof(Alignment), getLine[8]),
                    (FontSelectStyle)System.Enum.Parse(typeof(FontSelectStyle), getLine[9]), bool.Parse(getLine[10]), int.Parse(getLine[11]), bool.Parse(getLine[12]));

                    DialogList.Add(dialogRow);
                }
                
            }
        }
    }

    public void nextLine()
    {
        Debug.Log("Clicky");
       // if (DialogTool.GetComponent<DialogueTool>().hadOption == false){
            DialogTool.GetComponent<DialogueTool>().index++;
            DialogTool.GetComponent<DialogueTool>().setDialogueUI();
       // }
        
    }

    public void setDialogueTools()
    {
        foreach (GameObject dTool in DialogueToolsList)
        {
            /*
            if (dTool.GetComponent<DialogueTool>().DialogueList[0].order_priority == priority 
                && dTool.GetComponent<DialogueTool>().DialogueList[0].day == UIManager.GetComponent<UIManager>().dayNumber 
                && dTool.GetComponent<DialogueTool>().DialogueList[0].dayOrNight == UIManager.GetComponent<UIManager>().dayOrNight)
            {
                dTool.SetActive(true);
                //if (dTool.GetComponent<DialogueTool>().DialogueIndicator != null)
                  //  dTool.GetComponent<DialogueTool>().DialogueIndicator.SetActive(true);
            }
            else
            {
                dTool.SetActive(false);
            }
            */
        }
    }
}
