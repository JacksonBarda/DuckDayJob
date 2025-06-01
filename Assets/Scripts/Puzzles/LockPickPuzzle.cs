using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class LockPickPuzzle : Interactable
{
    [SerializeField]
    private List<GameObject> listRedPins;
    [SerializeField]
    private List<GameObject> listBluePins;
    [SerializeField]
    private List<GameObject> listSprings;
    [SerializeField]
    private GameObject pick;
    private Vector3 pickPosition;
    private float pickMinY = 314.18f;
    private float pickMaxY = 426.64f;
    [SerializeField]
    private GameObject shearLine;
    [SerializeField]
    private GameObject needle;
    [SerializeField]
    private int currentChamber;
    [SerializeField]
    private float offset;
    
    private float[] pcx = { 443.19f, 584.21f, 725.24f, 868.05f, 1005.5f };  // pick chamber x positions, rect anchor positions
    //private float[] pcy = { -56.2f, -47.6f, -61.9f, -52.7f, -47.2f };   // pick chamber y positions
    
    private bool pickMoving;
    [SerializeField]
    private float tension;
    [SerializeField]
    private float decay;
    [SerializeField]
    private float decaylimit;
    [SerializeField]
    private float boost;
    private bool canPick;
    private bool reset = true;


    public override void Interact()
    {
        base.Interact();
        
    }

    public override void Complete()
    {
        base.Complete();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        currentChamber = 0;
        pickMoving = true;
        tension = 50;
        canPick = true;
        puzzleUI.SetActive(true);

        foreach (GameObject bp in listBluePins)
        {
            bp.GetComponent<Rigidbody2D>().simulated = true;
        }
        pick.transform.position = new Vector2(pcx[0], pickMinY);

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        HandlePickMovement();
        HandleTension();
        HandleChamberMovement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttemptPick(currentChamber);
        }

        if (Input.GetMouseButtonDown(0)) // left click
        {
            tension -= boost;
        }

        if (Input.GetMouseButtonDown(1)) // right click
        {
            tension += boost;
        }
    }

    private void HandlePickMovement()
    {
        if (pickMoving)
        {
            pickPosition = Input.mousePosition;
            pickPosition.x -= 590;

            pickPosition.x = pcx[currentChamber];
            pickPosition.y = Mathf.Clamp(pickPosition.y, pickMinY, pickMaxY); // pick minimum and maximum Y positions

            pick.transform.position = pickPosition;
        }
    }

    private void HandleTension()
    {
        float angle = Mathf.Lerp(80, -80, tension / 100);
        decay = Mathf.Clamp(Mathf.Lerp(-decaylimit, decaylimit, tension / 100), -10, 10);

        if (tension >= 8)
        {
            tension += Time.deltaTime * decay * 10;

            if (reset == false) reset = true;
        }

        tension = Mathf.Clamp(tension, 0, 100);

        needle.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0,0,angle);

        if (tension >= 34 && tension <= 66)     // if tension is in green zone
        {
            canPick = true;
        }
        else                // if tension is outside green zone
        {
            canPick = false;

            if ((tension < 8 || tension > 93) && reset == true)         // if tension is in red zone
            {
                currentChamber = 0;
                tension = 0;
                reset = false;
                AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[3], SoundType.InteractableSFX, "ISFX_LockTensionFail");

                if (currentChamber != 0)
                {
                    StartCoroutine(MovePick(0));
                }
                foreach (GameObject bp in listBluePins)
                {
                    bp.GetComponent<Rigidbody2D>().simulated = true;
                }
            }
        }
    }

    private void HandleChamberMovement()
    {
        Chamber chamber = GetChamber(currentChamber);
        Vector2 bpAnchorPos = chamber.bp.GetComponent<RectTransform>().anchoredPosition;
        Vector2 newPosition = bpAnchorPos;

        if (chamber.rp.GetComponent<Collider2D>().IsTouching(pick.GetComponent<Collider2D>()))
        {
            Debug.Log("touching");

            newPosition.y = pick.GetComponent<RectTransform>().anchoredPosition.y + chamber.offset;
        }

        newPosition.y = Mathf.Clamp(newPosition.y, 29, 55);
        chamber.bp.GetComponent<RectTransform>().anchoredPosition = newPosition;
    }

    private void AttemptPick(int i)   // called when spacebar pressed; checks if pins match with shear line
    {
        //check collisions
        if (canPick)
        {
            Chamber chamber = GetChamber(i);

            if (shearLine.GetComponent<Collider2D>().IsTouching(chamber.rp.GetComponent<Collider2D>())
                && shearLine.GetComponent<Collider2D>().IsTouching(chamber.bp.GetComponent<Collider2D>()))
            {
                // chamber pick successful
                Debug.Log("LPP.Attemptpick(" + i + "): successful");
                CompleteChamber(i);
            }
            else
            {
                // unsuccessful
                Debug.Log("LPP.Attemptpick(" + i + "): not successful");
            }
        }
    }

    private void CompleteChamber(int i)
    {
        Debug.Log("LPP.CompleteChamber() ----------------------------------------------------------");
        Chamber chamber = GetChamber(i);

        chamber.bp.GetComponent<Rigidbody2D>().simulated = false;
        AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[3], SoundType.InteractableSFX, "ISFX_LockPinClick");

        currentChamber++;
        if(currentChamber > 4)  // if final chamber is complete
        {
            AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[3], SoundType.InteractableSFX, "ISFX_LockPicked");
            Complete();
        }
        else
        {
            StartCoroutine(MovePick(currentChamber));
            System.Random rnd = new System.Random();
            tension += rnd.Next(-10, 10);
            Debug.Log("LPP.CompleteChamber(" + i + "): MovePick(" + currentChamber + ")");
        }

    }

    private IEnumerator MovePick(int i)
    {
        Debug.Log("LPP.MovePick(" + i + ")");

        // disconnect pick from mouse input, move pick downwards, move to new position, reconnect to mouse input
        pickMoving = false;
        float timeElapsed = 0;
        Vector2 targetPosition = new Vector2(pick.transform.position.x, pickMinY);

        Debug.Log("LPP.MovePick(" + i + "): moving down...");
        while (true)    //downward movement
        {
            timeElapsed += Time.deltaTime;
            //pick.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(pick.GetComponent<RectTransform>().anchoredPosition, targetPosition, timeElapsed * 0.2f);
            pick.transform.position = Vector2.Lerp(pick.transform.position, targetPosition, timeElapsed * 0.2f);

            if (((Vector2)pick.transform.position - targetPosition).magnitude < new Vector2(0.1f, 0.1f).magnitude)
            {
                Debug.Log("LPP.MovePick(" + i + "): downwards movement done");
                break;
            }
            yield return null;
        }

        timeElapsed = 0;
        targetPosition = new Vector2(pcx[i], pickMinY);

        Debug.Log("LPP.MovePick(" + i + "): moving hoizontally...");
        while (true)    //lateral movement
        {
            timeElapsed += Time.deltaTime;
            //pick.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(pick.GetComponent<RectTransform>().anchoredPosition, targetPosition, timeElapsed * 0.2f);
            pick.transform.position = Vector2.Lerp(pick.transform.position, targetPosition, timeElapsed * 0.2f);

            if (((Vector2)pick.transform.position - targetPosition).magnitude < new Vector2(0.1f, 0.1f).magnitude)
            {
                Debug.Log("LPP.MovePick(" + i + "): movement complete");
                pickMoving = true;

                yield break;
            }
            yield return null;
        }
    }

    private Chamber GetChamber(int i)
    {
        Chamber chamber = new Chamber();

        chamber.rp = listRedPins[i];
        chamber.bp = listBluePins[i];
        chamber.s = listSprings[i];
        chamber.offset = -12.5f + (chamber.rp.GetComponent<RectTransform>().sizeDelta.y * chamber.rp.GetComponent<RectTransform>().localScale.y * chamber.bp.GetComponent<RectTransform>().localScale.y) + (chamber.bp.GetComponent<RectTransform>().sizeDelta.y * chamber.bp.GetComponent<RectTransform>().localScale.y) / 2;
        //chamber.offset = offset + (chamber.rp.GetComponent<RectTransform>().sizeDelta.y * chamber.rp.GetComponent<RectTransform>().localScale.y * chamber.bp.GetComponent<RectTransform>().localScale.y) + (chamber.bp.GetComponent<RectTransform>().sizeDelta.y * chamber.bp.GetComponent<RectTransform>().localScale.y) / 2;

        return chamber;
    }

    private struct Chamber
    {
        public GameObject rp;
        public GameObject bp;
        public GameObject s;
        public float offset;
    }
}