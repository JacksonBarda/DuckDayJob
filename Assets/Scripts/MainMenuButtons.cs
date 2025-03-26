using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField]
    private GameObject MainMenu;
    [SerializeField]
    private GameObject SettingButton;
    [SerializeField]
    private GameObject NameMenu;
    [SerializeField]
    private TMP_InputField playerInput;
    [SerializeField]
    private int saveSelected;

    private void Start()
    {
        PlayerPrefs.SetString("playerName", null);
        NameMenu.SetActive(false);
        MainMenu.SetActive(true);

        if (SceneManager.GetSceneByName("MainScene").isLoaded) SceneManager.UnloadSceneAsync("MainScene");
    }

    public void OnNewGameClicked()
    {
        MainMenu.SetActive(false);
        NameMenu.SetActive(true);
    }

    public void OnLoadSaveClicked()
    {
        MainMenu.SetActive(false);

        string filePath = "Saves/saveFile.json";
        if (File.Exists(filePath))
        {
            GameManager.loadSave = true;
        }

        SceneManager.LoadScene("MainScene");
    }

    public void OnBackClicked()
    {
        NameMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    
    public void OnSettingsClicked()
    {

    }

    public void OnLoadNewGame()
    {
        PlayerPrefs.SetString("playerName", playerInput.text);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainScene");
    }

    public void OnQuitClicked()
    {
        #if UNITY_EDITOR
        // If running in the Unity editor
        if (UnityEditor.EditorApplication.isPlaying)
        {
            // If in play mode, exit play mode
            UnityEditor.EditorApplication.ExitPlaymode();
        }
        #else
            // If not running in the Unity editor, quit the application
            Application.Quit();
        #endif
    }
}

