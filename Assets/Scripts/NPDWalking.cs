using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.Threading.Tasks;

public class NPDWalking : MonoBehaviour
{

   // public bool idleWalking = false;        //for NPD walking around map
   // public bool walkAfterDialogue = false;  //for NPD walking after dialogue (ie walk off screen)
   // public string currentRoom = "location";
   // //public string direction = "left";
   // private float moveVal = 0;
   // [SerializeField]
   // private bool isWalking = false;         //is currently walking at given moment
   // private bool active = true;            //has active walk cycle; active when player is in the same room, inactive when not
   // public float moveSpeed;
   // private int directionRNG;               //determine direction to walk with rng
   // private int durationRNG;                //determine idle walk duration with rng
   // private Vector3 movingThreshold;

   // //[SerializeField]
   //// private NPDWalkDirection setDirection;  //left or right

   // private Rigidbody rigid;
   // [SerializeField]
   // private Animator animator;
   // [SerializeField]
   // private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        //rigid = gameObject.GetComponent<Rigidbody>();
        //if (idleWalking) IdleWalk();
        //if (idleWalking == true)
        //{
        //    StartCoroutine(IdleWalk());

        //}
    }

    // Update is called once per frame
    void Update()
    {
        //if (active)
        //{
        //    rigid.velocity = new Vector3(moveVal * moveSpeed, 0, 0);        //movement
        //    if (rigid.velocity.x > 0.1f)                                    //if moving right
        //    {
        //        animator.SetFloat("Input", Mathf.Abs(rigid.velocity.x));
        //        spriteRenderer.flipX = false;
        //        isWalking = true;
        //    }
        //    else if (rigid.velocity.x < -0.1f)                              //if moving left
        //    {
        //        animator.SetFloat("Input", Mathf.Abs(rigid.velocity.x));
        //        spriteRenderer.flipX = true;
        //        isWalking = true;
        //    }
        //    if (rigid.velocity.x == 0f)                                     //if not moving
        //    {
        //        animator.SetFloat("Input", Mathf.Abs(rigid.velocity.x));
        //        isWalking = false;
        //    }

        //    movingThreshold = new Vector3(.01f, .01f, .01f);
        //    if ((rigid.velocity - movingThreshold).sqrMagnitude > .1f)
        //    {
        //        if (animator.GetBool("isMoving") == false)
        //        {
        //            animator.SetBool("isMoving", true);
        //            //AudioManager.Instance.PlayMovement("WalkSound");
        //        }
        //    }
        //    else
        //    {
        //        if (animator.GetBool("isMoving") == true)
        //        {
        //            animator.SetBool("isMoving", false);
        //            //AudioManager.Instance.PlayMovement("Silence");
        //        }
        //    }
        //}
        //else if (!active)// || !isWalking)                                     //if not walking or if player leaves room
        //{
        //    rigid.velocity = new Vector3(0, 0, 0);
        //}

    }

    //IEnumerator IdleWalk()
    //{
    //    while (active)
    //    {
    //        directionRNG = UnityEngine.Random.Range(0, 9);
    //        durationRNG = UnityEngine.Random.Range(3, 7);
    //        if (directionRNG < 5) //left
    //        {
    //            //direction = "left";
    //            //setDirection = NPDWalkDirection.Left;
    //        }
    //        else //right
    //        {
    //            //setDirection = NPDWalkDirection.Right;
    //        }

    //        yield return StartCoroutine(Walk(durationRNG));

    //        yield return new WaitForSeconds(UnityEngine.Random.Range(3, 7));
    //        //await Task.Delay(3000); 
    //    }
    //}

    //private IEnumerator Walk(int duration = 3)
    //{
    //    if (setDirection == NPDWalkDirection.Left)
    //    {
    //        moveVal = -1;
    //    }
    //    else if (setDirection == NPDWalkDirection.Right)
    //    {
    //        moveVal = 1;
    //    }

    //    //isWalking = true;


    //    yield return new WaitForSeconds(duration);

    //    moveVal = 0;
    //    //isWalking = false;
    //}
}
