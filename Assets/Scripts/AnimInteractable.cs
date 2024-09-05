using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimInteractable : Interactable
{
    [SerializeField]
    private PlayAnimation anim;
    public override void Interact()
    {
        //Start anim here
        anim.PlayAnimInteract(this);

    }
    public override void Complete()
    {
        //AfterAnim call complete
        base.Complete();
    }

}
