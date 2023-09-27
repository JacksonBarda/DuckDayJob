using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform Player;
    [SerializeField]
    private Camera mainCamera;
    // Start is called before the first frame update
    void Awake()
    {

        this.gameObject.transform.position = new Vector3(Player.position.x, Player.position.y + 1.25f, -10.0f);
        mainCamera.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = new Vector3(Player.position.x, Player.position.y+1.25f, -10.0f);

    }
}
