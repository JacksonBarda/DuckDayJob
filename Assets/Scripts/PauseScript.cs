using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField]
    private GameObject UI_MainMenu;
    [SerializeField]
    private GameObject UI_AudioMenu;
    [SerializeField]
    private GameObject UI_CharacterMenu;

    public Button BTN_Pause;
    public Button BTN_CharacterMenu;
    public Button BTN_AudioOption;
    public Button BTN_ReturnAudio;
    public Button BTN_ReturnCharacters;

    private bool OpenPause = false;


    // Start is called before the first frame update
    void Start()
    {
        BTN_Pause.onClick.AddListener(OpenMenu);
        BTN_AudioOption.onClick.AddListener(OpenAudio);
        BTN_CharacterMenu.onClick.AddListener(OpenCharacters);
        BTN_ReturnAudio.onClick.AddListener(MenuFromAudio);
        BTN_ReturnCharacters.onClick.AddListener(MenuFromCharacters);
    }

    public void OpenMenu()
    {
        if (OpenPause == true)
        {
            UI_MainMenu.SetActive(false);
            UI_AudioMenu.SetActive(false);
            UI_CharacterMenu.SetActive(false);
            OpenPause = false;
        }
        else
        {
            UI_MainMenu.SetActive(true);
            OpenPause = true;
        }
    }
    public void OpenAudio()
    {
        UI_AudioMenu.SetActive(true);
        UI_MainMenu.SetActive(false);
    }

    public void OpenCharacters()
    {

        UI_CharacterMenu.SetActive(true);
        UI_MainMenu.SetActive(false);
    }

    public void MenuFromAudio()
    {
        UI_AudioMenu.SetActive(false);
        UI_MainMenu.SetActive(true);
    }
    public void MenuFromCharacters()
    {
        UI_CharacterMenu.SetActive(false);
        UI_MainMenu.SetActive(true);
    }
}
