using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading;
using System;

public class PrototypePuzzle : Interactable
{
    private Vector3 mousePosition;
    private Vector3 screenBounds;
    private float timePassed = 0;
    private float timeUntilNextSpawn = 2;
    private float maxSpawnWaitTime = 2;
    private bool recordScore = true;
    [SerializeField]
    private GameObject ducksParent;

    public GameObject background;
    public Canvas canvasObject;
    public GameObject car;
    public GameObject duck;
    public Text textScore;
    public GameObject popupUI;
    public Text displayText;
    public float scrollSpeed = .3f;
    public int score;
    public int ducksHit;

    [HideInInspector]
    public List<GameObject> ListOfDucks;
    [HideInInspector]
    public bool active = false;

    // Start is called before the first frame update

    public override void Interact()
    {
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
        popupUI.SetActive(false);
        //AudioManager.Instance.PlayMusic("VendingAmbience");
        PlayerMove.puzzleMode = true;
        active = true;                      //is the puzzle active (things moving); necessary because update function runs before puzzle is interacted with
        recordScore = true;                 //is the puzzle keeping track of score; is true until just before duckpocalypse

        StartCoroutine(DuckSpawner());
    }

    public override void Action()
    {
        throw new NotImplementedException();
    }

    public override void Complete()
    {
        base.Complete();

        //Location of Puzzle
        AudioManager.Instance.PlayMusic("ManufacturingRoom");
    }

    void Awake()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        if (active){

            scrollSpeed = .3f + timePassed / 65;        //control scroll speed; initial speed is .3f

            mousePosition.x = 400;
            mousePosition.y = Math.Clamp(Input.mousePosition.y, 50f, 550f);
            mousePosition.z = 4;
            car.transform.position = mousePosition;     //car follow mouse Y position

            Debug.Log("PP: Car y-pos: " + car.transform.position.y);

            if (recordScore){
                timePassed += Time.deltaTime;
                score = (int)timePassed - (ducksHit * 5);
                textScore.text = "Score: " + score.ToString();
            }
            
        }
    }

    void SpawnDuck()
    {
        GameObject duckClone = Instantiate(duck);
        ListOfDucks.Add(duckClone);
        duckClone.transform.position = new Vector3(500, UnityEngine.Random.Range(-200, 200));
        duckClone.transform.SetParent(ducksParent.transform, false);

        if (ListOfDucks.Count >= 10){
            StartCoroutine(Duckpocalypse());
        }
    }

    IEnumerator DuckSpawner()
    {
        while (active){
            yield return new WaitForSeconds(timeUntilNextSpawn);
            SpawnDuck();
            timeUntilNextSpawn = UnityEngine.Random.Range(.1f, maxSpawnWaitTime);
            maxSpawnWaitTime -= .05f; //originally .01
        }
    }

    IEnumerator Duckpocalypse()
    {
        yield return new WaitForSeconds(3);
        recordScore = false;
        active = false;
        StopCoroutine(DuckSpawner());
        scrollSpeed = 0;

        yield return new WaitForSeconds(1);
        popupUI.SetActive(true);
        displayText.text = "TIME SURVIVED: " + timePassed +
                           "\nDUCKS HIT: " + ducksHit +
                           "\nSCORE: " + score;
    }
}
