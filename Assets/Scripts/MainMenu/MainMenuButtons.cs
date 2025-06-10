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

        string filePath = Application.dataPath + "saveFile.json";
        if (File.Exists(filePath))
        {
            MainMenu.gameObject.transform.GetChild(1).GetComponent<Button>().interactable = true;
        }
        else
        {
            MainMenu.gameObject.transform.GetChild(1).GetComponent<Button>().interactable = false;
        }
    }

    public void OnNewGameClicked()
    {
        MainMenu.SetActive(false);
        NameMenu.SetActive(true);

        AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[1], SoundType.Music, "ISFX_ButtonPress");
    }

    public void OnLoadSaveClicked()
    {
        AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[1], SoundType.Music, "ISFX_ButtonPress");
        MainMenu.SetActive(false);

        string filePath = Application.dataPath + "saveFile.json";
        if (File.Exists(filePath))
        {
            GameManager.loadSave = true;
        }

        LevelManager.Instance.LoadScene("MainScene");
    }

    public void OnBackClicked()
    {
        AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[1], SoundType.Music, "ISFX_ButtonPress");
        NameMenu.SetActive(false);
        MainMenu.SetActive(true);
    }


    public void OnLoadNewGame()
    {
        AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[1], SoundType.Music, "ISFX_ButtonPress");
        PlayerPrefs.SetString("playerName", playerInput.text);
        PlayerPrefs.Save();
        LevelManager.Instance.LoadScene("MainScene");
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

