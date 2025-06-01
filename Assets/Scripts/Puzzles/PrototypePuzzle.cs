using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading;
using System;

public class PrototypePuzzle : Interactable
{
    [SerializeField]
    private Canvas canvas;
    private RectTransform canvasRect;
    private Vector3 adjustedmousePosition;
    private Vector3 screenBounds;
    private float timePassed = 0;
    private float timeUntilNextSpawn = 2;
    private float maxSpawnWaitTime = 2;
    private bool recordScore = true;
    [SerializeField]
    private GameObject ducksParent;

    public GameObject background;
    public GameObject car;
    public List<Sprite> carSprites;
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
        AudioManager.StopSounds();
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
        popupUI.SetActive(false);
        PlayerMove.puzzleMode = true;
        active = true;                      //is the puzzle active (things moving); necessary because update function runs before puzzle is interacted with
        recordScore = true;                 //is the puzzle keeping track of score; is true until just before duckpocalypse

        AudioManager.PlaySoundContinuous(AudioManager.Instance.sourceList[0], SoundType.InteractableSFX, "ISFX_PrototypeCar");
        StartCoroutine(DuckSpawner());
    }

    public override void Action()
    {
        throw new NotImplementedException();
    }

    public override void Complete()
    {
        base.Complete();
    }

    void Awake()
    {
        
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        if (active){

            //Debug.Log("mouseinput: " + Input.mousePosition);

            scrollSpeed = .3f + timePassed / 65;        //control scroll speed; initial speed is .3f
            if ((int)timePassed % 2 == 0) car.GetComponent<Image>().sprite = carSprites[1];
            else car.GetComponent<Image>().sprite = carSprites[0];

            Vector3 carPos = Input.mousePosition;

            carPos.x = 300;//Mathf.Clamp(carPos.x, 190f, 1795f);
            carPos.y = Mathf.Clamp(carPos.y, 80f, 900f);
            car.transform.position = carPos;




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
        AudioManager.StopSounds();

        yield return new WaitForSeconds(1);

        popupUI.SetActive(true);
        displayText.text = "TIME SURVIVED: " + timePassed +
                           "\nDUCKS HIT: " + ducksHit +
                           "\nSCORE: " + score;
    }
}
