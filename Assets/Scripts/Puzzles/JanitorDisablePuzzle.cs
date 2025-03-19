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

    [SerializeField]
    private float decay;
    private float boost = 10f;
    private bool inProgress = false;
    private bool interacted = false;

    private void Awake()
    {
        disableJanitorPopup.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        progressBar.value = 20f;
        progressBar.minValue = 0f;
        progressBar.maxValue = 100f;

        inProgress = true;
    }

    public override void Interact()
    {
        interacted = true;
        PlayerMove.puzzleMode = true;
        mainUI.SetActive(false);

        player.transform.position = transformTeleportPlayer.position;
        choice = optionDialogue.getOptionAnswer();

        switch (choice)
        {
            case 1:
                decay = 10; //best
                break;
            case 2:
                decay = 15; //second best
                break;
            case 3:
                decay = 20; //second worst
                break;
            case 4:
                decay = 25; //worst
                break;
            default:
                decay = 30;
                Debug.LogError("JanitorDisablePuzzle: something went wrong here");
                break;
        }

        //spacebarAnimator.SetBool("isActive", true);
        disableJanitorPopup.SetActive(true);
    }

    public override void Complete()
    {
        base.Complete();
    }

    // Update is called once per frame
    void Update()
    {
        if (progressBar.value > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) && interacted) progressBar.value += boost;

            if (inProgress) progressBar.value -= Time.deltaTime * decay;
        }

        if (progressBar.value >= 98)
        {
            disableJanitorPopup.SetActive(false);
            Complete();
        }
        //disableJanitorPopup.transform.LookAt(cameraTransfrom, Vector3.up);
        //disableJanitorPopup.transform.eulerAngles = new Vector3(disableJanitorPopup.transform.eulerAngles.x, disableJanitorPopup.transform.eulerAngles.y + 180f, disableJanitorPopup.transform.eulerAngles.z);

    }
}
