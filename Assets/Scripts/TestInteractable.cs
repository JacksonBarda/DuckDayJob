using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : Interactable
{
    [SerializeField]
    protected FadeIn fadeIn;
    [SerializeField]
    protected FadeOut fadeOut;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Interact()
    {
        fadeOut.InstantFadeOut();
        fadeIn.FadeImageInOverTime(0.5f);
    }
    public override void Action()
    {

    }
    
}
