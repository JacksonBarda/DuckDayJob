using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading;
using System;

public class PrototypePuzzle : Interactable
{
    public float scrollSpeed;

    private Vector3 mousePosition;
    private Vector3 screenBounds;
    private float timePassed = 0;
    private float timeUntilNextSpawn = 2;
    private float maxSpawnWaitTime = 2;
    private bool active = false;

    [SerializeField]
    public GameObject background;
    [SerializeField]
    public Canvas canvasObject;
    [SerializeField]
    public GameObject car;
    [SerializeField]
    public GameObject duck;

    // Start is called before the first frame update

    public override void Interact()
    {
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
        //AudioManager.Instance.PlayMusic("VendingAmbience");
        player.puzzleMode = true;
        active = true;          //is the puzzle active; necessary because update function runs before puzzle is interacted with

        StartCoroutine(DuckSpawner());
        Debug.Log("StartCoroutine");
    }

    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override void Complete()
    {

        //Location of Puzzle
        AudioManager.Instance.PlayMusic("ManufacturingRoom");
        active = false;
        StopCoroutine(DuckSpawner());
    }

    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        if (active){

            timePassed += Time.deltaTime;

            scrollSpeed = .3f + timePassed / 65;        //control scroll speed; initial speed is .3f

            mousePosition.x = 250;
            mousePosition.y = Input.mousePosition.y;
            mousePosition.z = 4;
            car.transform.position = mousePosition;     //car follow mouse Y position

        }
    }

    void SpawnDuck()
    {
        GameObject duckClone = Instantiate(duck);
        duckClone.transform.position = new Vector3(500, UnityEngine.Random.Range(-200, 200));
        duckClone.transform.SetParent(canvasObject.transform, false);
    }

    IEnumerator DuckSpawner()
    {
        while (true){
            yield return new WaitForSeconds(timeUntilNextSpawn);
            SpawnDuck();
            timeUntilNextSpawn = UnityEngine.Random.Range(.1f, maxSpawnWaitTime);
                maxSpawnWaitTime -= .01f;
            
        }
        
    }

    void SpawnDuckpocalypse()
    {
        int counter = 0;
        while (counter < 100)
        {
            SpawnDuck();
            counter++;
        }
    }
}
