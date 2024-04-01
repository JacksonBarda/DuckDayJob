using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeDuck : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    private Vector3 screenBounds;

    private PrototypePuzzle prototypePuzzle;

    // Start is called before the first frame update
    void Start()
    {
        prototypePuzzle = GameObject.Find("/Puzzles/PrototypePuzzle/InteractPrototype").GetComponent<PrototypePuzzle>();
        speed = (prototypePuzzle.scrollSpeed + .1f) * 1000;

        rb = this.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(-speed, 0);
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < screenBounds.x) {
            Destroy(this.gameObject);
        }
    }
}
