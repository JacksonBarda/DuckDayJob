using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private void Start()
    {
        PlayerPrefs.SetString("playerName", null);
    }
    public void OnStartClicked()
    {

        string _playerName = PlayerPrefs.GetString("playerName");
        Debug.Log(_playerName);
        if (_playerName != "")
        {
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            MainMenu.SetActive(false);
            NameMenu.SetActive(true);
        }
        
    }
    public void OnSettingsClicked()
    {

    }
    public void OnBackClicked()
    {

    }
    public void OnEnterNameClicked()
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

