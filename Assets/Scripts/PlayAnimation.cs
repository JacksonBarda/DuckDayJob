using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator anim = null;
    [SerializeField]
    private string clipToPlay;
    [SerializeField]
    private bool camFollow = false;
    [SerializeField] 
    private bool willMove = false;
    [SerializeField]
    private FollowPlayer camAnchor = null;
    [SerializeField]
    private Transform startAnimTrans = null;
    [SerializeField]
    private Transform endAnimTrans = null;
    [SerializeField]
    float totalMovementTime = 5f; //the amount of time you want the movement to take


    private Transform taskTransform = null;
    private Vector3 PHStartAnimPos;
    private Vector3 PHEndAnimPos;
    private Transform ph;
    private Interactable task;


    void Start()
    {
        anim.StopPlayback();
        PHStartAnimPos = startAnimTrans.position;
        PHEndAnimPos = endAnimTrans.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayAnimInteract(Interactable _task)
    {
        task = _task;
        taskTransform = task.transform;
        anim.Play(clipToPlay);
        if(willMove)
        {
            if( camAnchor != null)
            {                
                ph = camAnchor.Player;
                if (camFollow)
                {
                    camAnchor.Player = taskTransform;
                }
            }
        }
		StartCoroutine(moveObject());
	}
    public IEnumerator moveObject()
    {

        float currentMovementTime = 0f;//The amount of time that has passed
        while (Vector3.Distance(taskTransform.position, PHEndAnimPos) > 0.1f)
        {
            //Debug.Log(Vector3.Distance(taskTransform.position, PHEndAnimPos));
            currentMovementTime += Time.deltaTime;
            taskTransform.position = Vector3.Lerp(taskTransform.position, PHEndAnimPos, currentMovementTime / totalMovementTime);
            yield return null;

        }
        camAnchor.Player = ph;
        if(task is AnimInteractable)
            task.Complete();
        if(task is DialogueTool)
            task.Complete();
    }
}
