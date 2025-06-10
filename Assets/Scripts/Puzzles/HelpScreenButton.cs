using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreenButton : MonoBehaviour
{
    [SerializeField]
    private GameObject helpScreenOverlay;

    // Start is called before the first frame update
    void Awake()
    {
        helpScreenOverlay.SetActive(false);
    }

    public void ToggleHelpScreen()
    {
        helpScreenOverlay.SetActive(!helpScreenOverlay.activeSelf);
    }
}
