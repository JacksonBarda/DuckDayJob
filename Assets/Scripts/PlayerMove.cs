using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private float moveValRight;
    private float moveValLeft;
    private float moveValUp;
    private float moveValDown;
    private float moveValRightHolder;
    private float moveValLeftHolder;
    private float moveValUpHolder;
    private float moveValDownHolder;
    [SerializeField]
    private float moveSpeed;

    private Rigidbody rigid;
    [SerializeField]
    private bool grounded = false;
    [SerializeField]
    private Vector3 customLocation;

    private Interactable interactable;


    public bool mazeMode = false;

    
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
        if (interactable != null && mazeMode)
        {
            interactable.Interact();
        }
        if (grounded && !mazeMode)
        {
            moveValUp = value.Get<float>();
            rigid.velocity = new Vector3(moveValRight - moveValLeft, (rigid.velocity.y / moveSpeed) + moveValUp, 0f) * moveSpeed;
        }
    }

    void OnInteract(InputValue value)
    {
        moveValUpHolder = value.Get<float>();
        if (interactable != null && !mazeMode)
        {
            interactable.Interact();
            Debug.Log("interacted");
        }
        if (mazeMode)
        {
            moveValUp = moveValUpHolder;
        }
    }
    void OnDuck(InputValue value)
    {
        moveValDownHolder = value.Get<float>();
        if (mazeMode)
        {
            moveValDown = moveValDownHolder;
        }
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, -Vector3.up, 0.75f);

        if (!mazeMode)
        {
            rigid.velocity = new Vector3(moveValRight - moveValLeft +((rigid.velocity.x) - (rigid.velocity.x * 0.1f)), (rigid.velocity.y), 0f);
            moveValRight = moveValRightHolder;
            moveValLeft = moveValLeftHolder;
        }
        if(mazeMode)
        {
            moveValDown = moveValDownHolder;
            moveValRight = moveValRightHolder;
            moveValLeft = moveValLeftHolder;
            moveValUp = moveValUpHolder;
            rigid.velocity = new Vector3(moveValRight - moveValLeft, moveValUp-moveValDown, 0f) * moveSpeed;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
          //  customLocation = 
            MoveToCustomLocation();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            ContactPoint contact = collision.GetContact(0);
            Vector3 bounceDirection = contact.normal;
            float bounceForce = 50f; // Adjust this value to control the bounce intensity

            // Apply the bounce force
            rigid.velocity = new Vector3(-rigid.velocity.x, (rigid.velocity.y), 0f);
            rigid.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
            
        }

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Interactable")
        {
            interactable = collision.gameObject.GetComponent<Interactable>();
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Interactable")
        {
            interactable = null;
        }
    }
    public void Maze(bool value)
    {
        mazeMode = value;
    }
    private void MoveToCustomLocation()
    {
        transform.position = customLocation;
    }
}
