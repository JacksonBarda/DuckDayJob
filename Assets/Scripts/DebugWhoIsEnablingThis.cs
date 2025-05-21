using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugWhoIsEnablingThis : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.LogError("I just got enabled!", this);
    }
}
