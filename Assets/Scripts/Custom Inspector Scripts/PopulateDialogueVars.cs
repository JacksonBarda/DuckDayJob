using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueTool))]
public class PopulateDialogueVars : Editor

{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DialogueTool dt = (DialogueTool)target;
        if (GUILayout.Button("Auto-Populate Variables (Not working)"))
        {
            dt.assignVariables();
        }
    }
}
