using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Base_Mission), true)]
[CanEditMultipleObjects]
public class Base_MissionEditor : Editor {

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        if (GUILayout.Button("Remove quest"))
        {
            DestroyImmediate(target);

        }
    }

}
