using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Day[] dayNum;
    [SerializeField]
    TaskManager TM;
    [SerializeField]
    UIManager uiManager;

    public static bool loadSave;

    // Start is called before the first frame update
    void Start()
    {
        if (loadSave)
        {
            OnLoadGameState();
            loadSave = false;
        }
    }

    public void OnSaveGameState()   
    {
        Debug.Log("GameManager: OnSaveState()");
        
        GameData saveState = new GameData();

        saveState.playerName = PlayerPrefs.GetString("playerName");
        Debug.Log("GameManager.OnSaveState(): PlayerPrefs playername: " + PlayerPrefs.GetString("playerName"));
        saveState.day = TM.day;
        saveState.dayPart = TM.ptCount;
        saveState.completedTasksInPart = TM.getCompletedTasksOfPart();

        string json = JsonUtility.ToJson(saveState);

        File.WriteAllText(Application.dataPath + "saveFile.json", json); //+ gameSaveSlot + ".json", json);

        StartCoroutine(uiManager.NotifySaveProgress());
        //StartCoroutine()
    }

    public void OnLoadGameState() //change to take save file for input
    {
        //Debug.LogError("GameManager: OnLoadGameState()");

        GameData save;
        save = GetSave();

        PlayerPrefs.SetString("playerName", save.playerName);
        PlayerPrefs.Save();

        TM.CheatSkipToDay(save.day);                //  set day
        
        Debug.Log("save.day = " + save.day);

        for (int i = 0; i < save.dayPart; i++)      //  complete all tasks until dayPart
        {
            int j = 0;
            Debug.Log("Part " + TM.GetCurrentPart());
            foreach (Interactable task in TM.tasksByDay[save.day-1].GetInteractables((TaskManager.PartIdentifier)i))
            {
                Debug.Log("Completing task #" + j + " in part " + TM.GetCurrentPart());
                task.gameObject.SetActive(true);
                TM.CheatCompleteTask(task);
                j++;
            }
        }
        if (save.completedTasksInPart != null && save.completedTasksInPart.Count != 0)
        {
            foreach (Interactable task in save.completedTasksInPart)    //  complete tasks in list
            {
                task.gameObject.SetActive(true);
                TM.CheatCompleteTask(task);
            }
        }
    }

    public GameData GetSave()
    {
        string filePath;
        string json;
        GameData loadedGameData;

        filePath = Application.dataPath + "saveFile.json";
        json = File.ReadAllText(filePath);
        loadedGameData = JsonUtility.FromJson<GameData>(json);

        return loadedGameData;
    }

    public void OnExitToMenu()
    {
        Debug.Log("GameManager: Load scene - StartScreen");
        LevelManager.Instance.LoadScene("StartScreen");
    }

    public class GameData
    {
        public string playerName;
        public int day;
        public int dayPart;
        public List<Interactable> completedTasksInPart;
    }
}