using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ExecuteInEditMode))]
public class ScriptSwapper : Editor 
{
    public override void OnInspectorGUI() 
    {
        DrawDefaultInspector();

        MonoScript yourComponent = (MonoScript)target;
        
        if(GUILayout.Button("Toggle ExecuteInEditMode")) 
        {
            if (yourComponent.GameObject().GetComponent<ExecuteInEditMode>() is null) 
            {
                //yourComponent.GameObject().AddComponent<ExecuteInEditMode>();
                DestroyImmediate(yourComponent);
            }
            else
            {
                //yourComponent.GameObject().AddComponent<MonoScript>();
                DestroyImmediate(yourComponent);
            }
        }
    }
}
