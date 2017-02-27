using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(QuestController))]
[CanEditMultipleObjects]
public class QuestControllerEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Label("Click to add a mission into the scene.");
        QuestController myscript = (QuestController)target;
        if (GUILayout.Button("Find and kill target"))
        {
            myscript.addQuest("KillTarget");
        }
        GUILayout.Label("");
        if (GUILayout.Button("Kill X amount of enemies"))
        {
            myscript.addQuest("KillXTargets");
        }
        GUILayout.Label("");
        if (GUILayout.Button("Collect object"))
        {
            myscript.addQuest("CollectObject");
        }
    }
}
