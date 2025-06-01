using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Unity.VisualScripting;

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
    private Vector3 movingThreshold;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private Rigidbody rigid;
    [SerializeField]
    private bool grounded = false;
    [SerializeField]
    private GameObject customLocation;
    [SerializeField]
    private List<Interactable> interactable = new List<Interactable>();

    public static bool puzzleMode = false;
    public bool mazeMode = false;

   
    
    private void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
        rigid.useGravity = true;
        interactable.Remove(interactable[0]);
        //animator = GetComponent<Animator>();
    }
    void OnLeave()
    {
        //interactable[0].Complete();
    }
    void OnRight(InputValue value)
    {
        moveValRightHolder = value.Get<float>();
        if (grounded && moveValLeft < .01)
        {
            moveValRight = moveValRightHolder;
            animator.SetFloat("Input", moveValRight);
            //spriteRenderer.flipX = false;
        }
    }
    void OnLeft(InputValue value)
    {
        moveValLeftHolder = value.Get<float>();
        if (grounded && moveValRight < .01)
        {
            moveValLeft = moveValLeftHolder;
           // animator.SetFloat("Input", moveValLeft);

        }
        
    }

    private Interactable holder;

    void OnInteract(InputValue value)
    {
        moveValUpHolder = value.Get<float>();
        if (interactable.Count > 0 && !mazeMode && !puzzleMode)
        {

            holder = interactable[interactable.Count-1];
            try
            {
                Debug.Log("PlayerMove: interacted - " + interactable[interactable.Count - 1]);
                interactable.Clear();
                UIManager.Instance.SetInteractPopupText();
                moveValDown = 0;
                moveValRight = 0;
                moveValLeft = 0;
                moveValUp = 0;
                rigid.velocity = new Vector3(moveValRight - moveValLeft, moveValUp - moveValDown, 0f) * moveSpeed;
                animator.SetBool("isMoving", false);
                holder.Interact();

				interactable.Add(holder);

				//interactable.Clear();
			}
            catch (ArgumentOutOfRangeException)
            {
                //Debug.LogWarning("No interactable found");
            } 
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
        if(interactable != null)
        {
           foreach(Interactable task in interactable.ToList())
           {
                if (!task.gameObject.activeSelf)
                {
                    interactable.Remove(task);
                }
            }

           //for (int i = 0; i < interactable.Count-1; i++)
           // {
           //     if (!interactable[i].gameObject.activeSelf)
           //     {
           //         interactable.Remove(interactable[i]);
           //     }

           //     if (i == interactable.Count - 1)
           //     {
           //         interactable[i]
           //     }
           // }

            
        }
 
        grounded = Physics.Raycast(transform.position, -Vector3.up, 0.75f);

        if (!puzzleMode)
        {
            if (!mazeMode)
            {
                rigid.velocity = new Vector3(moveValRight - moveValLeft +((rigid.velocity.x) - (rigid.velocity.x * 0.1f)), (rigid.velocity.y), 0f) * moveSpeed;
                
                moveValRight = moveValRightHolder;
                moveValLeft = moveValLeftHolder;
                rigid.useGravity = true;
            }

            if(mazeMode)
            {
                moveValDown = moveValDownHolder;
                moveValRight = moveValRightHolder;
                moveValLeft = moveValLeftHolder;
                moveValUp = moveValUpHolder;
                rigid.velocity = new Vector3(moveValRight - moveValLeft, moveValUp-moveValDown, 0f) * moveSpeed * 3;
                rigid.useGravity = false;
            }

            //float i = (float)Math.Sqrt(Math.Pow(rigid.velocity.x, 2) + Math.Pow(rigid.velocity.y, 2));
            animator.SetFloat("Input", Mathf.Abs((float)Math.Sqrt(Math.Pow(rigid.velocity.x, 2) + Math.Pow(rigid.velocity.y, 2))));
            if (rigid.velocity.x > 0.1f)
            {
                //animator.SetFloat("Input", Mathf.Abs(rigid.velocity.x));
                spriteRenderer.flipX = false;
            }
            else if (rigid.velocity.x < -0.1f)
            {
                //animator.SetFloat("Input", Mathf.Abs(rigid.velocity.x));
                spriteRenderer.flipX = true;
            }
            if (rigid.velocity.x == 0f)
            {
                //animator.SetFloat("Input", Mathf.Abs(rigid.velocity.x));
            }


            movingThreshold = new Vector3(.01f, .01f, .01f);
            if ((rigid.velocity - movingThreshold).sqrMagnitude > .1f)
            {
                if (animator.GetBool("isMoving") == false)
                {
                    animator.SetBool("isMoving", true);
                }
            }
            else
            {
                if (animator.GetBool("isMoving") == true)
                {
                    animator.SetBool("isMoving", false);
                }
            }
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
            if (interactable != null)
                Debug.Log(collision.gameObject.name);
                interactable.Add(collision.gameObject.GetComponent<Interactable>());

        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Interactable")
        {
			if (interactable.Count > 0)
			{
				// Create a temporary list to store items to remove
				List<Interactable> itemsToRemove = new List<Interactable>();

				foreach (Interactable interactableObject in interactable)
				{
					// Check if the interactable's name contains the target name
					if (interactableObject != null &&
						interactableObject.name.Contains(collision.gameObject.GetComponent<Interactable>().name))
					{
						// Add the interactable object to the removal list
						itemsToRemove.Add(interactableObject);
					}
				}

				// Remove all items in the temporary list
				foreach (Interactable item in itemsToRemove)
				{
                    interactable.Remove(item);
				}
			}


		}
    }
    
    public void Maze(bool value)
    {
        mazeMode = value;
    }
    public void CheatMoveToCustomLocation()
    {
        transform.position = customLocation.transform.position;
    }
    public Interactable GetInteractable() 
    {


        if (interactable != null && interactable.Count > 0)
        {
            //Debug.Log("not null");
            return interactable[interactable.Count - 1];
        }
        else
        {
            //Debug.Log("null");
            return null;
        }
    }



}
