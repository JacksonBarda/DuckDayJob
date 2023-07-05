using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private float moveValRight;
    private float moveValLeft;
    private float moveValUp;
    private float moveValRightHolder;
    private float moveValLeftHolder;
    private float moveValUpHolder;
    [SerializeField]
    private float moveSpeed;

    private Rigidbody rigid;
    [SerializeField]
    private bool grounded = false;

    
    private void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();

    }
    void OnRight(InputValue value)
    {
        moveValRightHolder = value.Get<float>();
        if (grounded)
        {
            moveValRight = moveValRightHolder;
        }
        
    }
    void OnLeft(InputValue value)
    {
        moveValLeftHolder = value.Get<float>();
        if (grounded)
        {
            moveValLeft = moveValLeftHolder;
        }
    }
    void OnJump(InputValue value)
    {
        if (grounded)
        {
            moveValUp = value.Get<float>();
            rigid.velocity = new Vector3(moveValRight - moveValLeft, (rigid.velocity.y / moveSpeed) + moveValUp, 0f) * moveSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, -Vector3.up, 0.75f);
        rigid.velocity = new Vector3(moveValRight - moveValLeft, (rigid.velocity.y/moveSpeed), 0f) * moveSpeed;
        if (grounded)
        {
            moveValRight = moveValRightHolder;
            moveValLeft = moveValLeftHolder;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            moveValRight = 0f;
            moveValLeft = 0f;
            rigid.velocity = new Vector3(0f, (rigid.velocity.y / moveSpeed), 0f) * moveSpeed;
        }
    }
}
