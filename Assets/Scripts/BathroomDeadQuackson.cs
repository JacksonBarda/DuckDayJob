using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathroomDeadQuackson : Interactable
{
    public List<GameObject> bloodSprites;
    public GameObject bathroomDoor;
    [SerializeField]
    public Vector3 newDoorPosition;
    [SerializeField]
    public Vector3 newDoorRotation;

    private void Awake()
    {
        foreach (GameObject blood in bloodSprites)
        {
            blood.SetActive(false);
        }
        bathroomDoor.transform.localPosition = new Vector3(0, 0, 0);
        bathroomDoor.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    public override void Interact()
    {
        foreach (GameObject blood in bloodSprites)
        {
            blood.SetActive(true);
        }
        bathroomDoor.transform.localPosition = newDoorPosition;
        bathroomDoor.transform.localEulerAngles = newDoorRotation;
        //Complete();
    }
}
