using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JanitorDisablePuzzle : Interactable
{
    [SerializeField]
    private Transform transformTeleportPlayer;
    [SerializeField]
    private Transform cameraTransfrom;
    [SerializeField]
    private DialogueTool optionDialogue;
    [SerializeField]
    private GameObject disableJanitorPopup;
    [SerializeField]
    private Slider progressBar;
    [SerializeField]
    private SpriteRenderer janitorSprite;
    [SerializeField]
    private int choice;


    private float decay;
    private float boost = 10f;
    private bool inProgress = false;

    private void Awake()
    {
        disableJanitorPopup.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        choice = optionDialogue.getOptionAnswer();
        progressBar.value = 20f;
        progressBar.minValue = 0f;
        progressBar.maxValue = 100f;

        switch (choice)
        {
            case 1:
                decay = 3; //best
                break;
            case 2:
                decay = 4; //second best
                break;
            case 3:
                decay = 5; //second worst
                break;
            case 4:
                decay = 8; //worst
                break;
        }

        disableJanitorPopup.SetActive(true);
    }

    public override void Interact()
    {
        inProgress = true;
        player.transform.position = transformTeleportPlayer.position;
    }

    public override void Complete()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (progressBar.value > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) && inProgress) progressBar.value += boost;

            progressBar.value -= Time.deltaTime * decay;
        }

        disableJanitorPopup.transform.LookAt(cameraTransfrom, Vector3.up);
        disableJanitorPopup.transform.eulerAngles = new Vector3(disableJanitorPopup.transform.eulerAngles.x, disableJanitorPopup.transform.eulerAngles.y + 180f, disableJanitorPopup.transform.eulerAngles.z);
    }
}
