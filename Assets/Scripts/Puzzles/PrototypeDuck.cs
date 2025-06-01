using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeDuck : MonoBehaviour
{
    public float speed;

    private Rigidbody rb;
    private Vector3 screenBounds;
    private bool isAlive;
    private PrototypePuzzle prototypePuzzle;

    // Start is called before the first frame update
    void Start()
    {
        prototypePuzzle = GameObject.Find("/Puzzles/PrototypePuzzle/InteractPrototype").GetComponent<PrototypePuzzle>();
        speed = (prototypePuzzle.scrollSpeed + .4f) * 1000;

        rb = this.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(-speed, 0);
        rb.useGravity = false;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive && prototypePuzzle.active){
            transform.Rotate(new Vector3(0, 0, 400) * Time.deltaTime); //rotate if dead
        }
        if (transform.position.x < screenBounds.x) {
            Destroy(this.gameObject);
            prototypePuzzle.ListOfDucks.Remove(gameObject);
            Debug.Log("Ducks in list: " + prototypePuzzle.ListOfDucks.Count);
        }
        if (!prototypePuzzle.active){
            rb.velocity = new Vector3(0, 0, 0);

        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && isAlive){   //prevent collision on dead duck
            AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[3], SoundType.InteractableSFX, "ISFX_PrototypeDuckDeath");
            rb.velocity += new Vector3(0, -speed);
            Debug.Log("hit duck");
            prototypePuzzle.ducksHit++;
            isAlive = false;
            prototypePuzzle.ListOfDucks.Remove(gameObject);
            Debug.Log("Ducks in list: " + prototypePuzzle.ListOfDucks.Count);
        }
    }
}
