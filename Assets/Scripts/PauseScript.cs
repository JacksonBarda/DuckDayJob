using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField]
    private GameManager GM;
    [SerializeField]
    private GameObject UI_MainPauseMenu;
    [SerializeField]
    private GameObject UI_AudioMenu;

    public Button BTN_Pause;
    public Button BTN_Resume;
    public Button BTN_AudioOption;
    public Button BTN_ReturnAudio;
    public Button BTN_ExitGame;

    private bool OpenPause = false;

    // Start is called before the first frame update
    void Start()
    {
        BTN_Pause.onClick.AddListener(OpenMenu);
        BTN_Resume.onClick.AddListener(OpenMenu);
        BTN_AudioOption.onClick.AddListener(OpenAudio);
        BTN_ReturnAudio.onClick.AddListener(MenuFromAudio);
        BTN_ExitGame.onClick.AddListener(ExitGame);

        UI_MainPauseMenu.SetActive(false);
        UI_AudioMenu.SetActive(false);
    }

    public void OpenMenu()
    {
        if (OpenPause == true)
        {
            UI_MainPauseMenu.SetActive(false);
            UI_AudioMenu.SetActive(false);
            OpenPause = false;
            PlayerMove.puzzleMode = false;
        }
        else
        {
            UI_MainPauseMenu.SetActive(true);
            OpenPause = true;
            PlayerMove.puzzleMode = true;
        }
    }
    public void OpenAudio()
    {
        UI_AudioMenu.SetActive(true);
        UI_MainPauseMenu.SetActive(false);
    }

    public void MenuFromAudio()
    {
        UI_AudioMenu.SetActive(false);
        UI_MainPauseMenu.SetActive(true);
    }
    public void ExitGame()
    {
        GM.OnSaveGameState();
        GM.OnExitToMenu();

        //#if UNITY_EDITOR
        //    // If running in the Unity editor
        //    if (UnityEditor.EditorApplication.isPlaying)
        //    {
        //        // If in play mode, exit play mode
        //        UnityEditor.EditorApplication.ExitPlaymode();
        //    }
        //#else
        //    // If not running in the Unity editor, quit the application
        //    Application.Quit();
        //#endif  
    }
}
