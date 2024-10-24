using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatsForDebug : MonoBehaviour
{
    public TaskManager TaskManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnJump()
    {
        TaskManager.findCurrentInteractable().Complete();


	}
}
