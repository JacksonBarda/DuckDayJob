using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WirePuzzle : Interactable
{
    public List<Wire> wires;
   
    public float rotationInterval = 1f;
    //public AudioClip alertSound;

   // private AudioSource audioSource;
    public List<bool> initialStates;


    private int wiresCut;



    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override void Interact()
    {
        player.puzzleMode = true;
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
        initialStates = new List<bool>(wires.Count);
        for (int i = 0; i < wires.Count; i++)
        {
            initialStates.Add(Random.value > 0.5f);
        }
        StartCoroutine(RotatePowerStates());
    }

    public override void Complete()
    {
        base.Complete();

        for (int i = 0; i < wires.Count; i++)
        {
            wires[i].transform.gameObject.SetActive(true);
        }
        wiresCut = 0;

        AudioManager.Instance.PlaySFX("SFX_Complete");
    }


    private IEnumerator RotatePowerStates()
    {

        int currentIndex = 0;
        bool currentState = false;

        while (true)
        {
            initialStates[currentIndex] = currentState;
            currentIndex = (currentIndex + 1) % wires.Count;
            currentState = !currentState;
            yield return new WaitForSeconds(rotationInterval);
        }
    }

    public void CutWire(int index)
    {
        if (!initialStates[index])
        {
            wires[index].transform.gameObject.SetActive(false);
            wiresCut++;
            AudioManager.Instance.PlaySFX("SFX_WireCut");
            if (wiresCut == wires.Count)
            {
                StopCoroutine(RotatePowerStates());
                Complete();
                
            }
        }
        else
        {
            //audioSource.PlayOneShot(alertSound);
            ResetWires();
        }
    }

    private void ResetWires()
    {
        for (int i = 0; i < wires.Count; i++)
        {
            wires[i].transform.gameObject.SetActive(true);
        }
        wiresCut = 0;
    }
}


