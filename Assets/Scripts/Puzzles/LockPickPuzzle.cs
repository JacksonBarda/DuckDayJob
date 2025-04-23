using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class LockPickPuzzle : Interactable
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private List<GameObject> listRedPins;
    [SerializeField]
    private List<GameObject> listBluePins;
    [SerializeField]
    private List<GameObject> listSprings;
    [SerializeField]
    private GameObject pick;
    [SerializeField]
    private GameObject shearLine;
    [SerializeField]
    private GameObject needle;
    [SerializeField]
    private int currentChamber;
    
    private float[] pcx = { -216.12f, -156.8f, -99f, -40f, 18.7f };  // pick chamber x positions
    //private float[] pcy = { -56.2f, -47.6f, -61.9f, -52.7f, -47.2f };   // pick chamber y positions
    
    private Vector2 mousePosition;
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
    void Start()
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
        pick.GetComponent<RectTransform>().anchoredPosition = new Vector2(pcx[0], -91.1f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("test");
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
            mousePosition.x = pcx[currentChamber];
            mousePosition.y = Mathf.Clamp(Input.mousePosition.y, 168, 226);

            Vector2 anchorPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(puzzleUI.GetComponent<RectTransform>(), mousePosition, cam, out anchorPos);

            anchorPos.x = pcx[currentChamber];

            pick.GetComponent<RectTransform>().anchoredPosition = anchorPos;
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

        if (tension >= 34 && tension <= 66)
        {
            canPick = true;
        }
        else
        {
            canPick = false;

            if ((tension < 8 || tension > 93) && reset == true)
            {
                currentChamber = 0;
                tension = 0;
                reset = false;

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

        //for (int i = 0; i < 4; i++)
        //{
        //    GameObject bp = listBluePins[i];
        //    Vector2 tempPos = bp.GetComponent<RectTransform>().anchoredPosition;
        //    tempPos.y = Mathf.Clamp(bp.GetComponent<RectTransform>().anchoredPosition.y, 29, 55);
        //    bp.GetComponent<RectTransform>().anchoredPosition = tempPos;
        //}

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

        currentChamber++;
        if(currentChamber > 4)
        {
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
        Vector2 targetPosition = new Vector2(pick.GetComponent<RectTransform>().anchoredPosition.x, -91.1f);

        Debug.Log("LPP.MovePick(" + i + "): moving down...");
        while (true)    //downward movement
        {
            timeElapsed += Time.deltaTime;
            pick.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(pick.GetComponent<RectTransform>().anchoredPosition, targetPosition, timeElapsed * 0.2f);

            if ((pick.GetComponent<RectTransform>().anchoredPosition - targetPosition).magnitude < new Vector2(0.1f, 0.1f).magnitude)
            {
                Debug.Log("LPP.MovePick(" + i + "): downwards movement done");
                break;
            }
            yield return null;
        }

        timeElapsed = 0;
        targetPosition = new Vector2(pcx[i], -91.1f);

        Debug.Log("LPP.MovePick(" + i + "): moving hoizontally...");
        while (true)    //lateral movement
        {
            timeElapsed += Time.deltaTime;
            pick.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(pick.GetComponent<RectTransform>().anchoredPosition, targetPosition, timeElapsed * 0.2f);

            if ((pick.GetComponent<RectTransform>().anchoredPosition - targetPosition).magnitude < new Vector2(0.1f, 0.1f).magnitude)
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
        //chamber.offset = 37.8f + (chamber.rp.GetComponent<RectTransform>().sizeDelta.y * chamber.rp.GetComponent<RectTransform>().localScale.y)/2;
        chamber.offset = 37.8f + (chamber.rp.GetComponent<RectTransform>().sizeDelta.y * chamber.rp.GetComponent<RectTransform>().localScale.y * chamber.bp.GetComponent<RectTransform>().localScale.y) + (chamber.bp.GetComponent<RectTransform>().sizeDelta.y * chamber.bp.GetComponent<RectTransform>().localScale.y) / 2;

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
