using  UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RoomCreator))]
public class RoomCreatorEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        RoomCreator creator = (RoomCreator) target;
        if (GUILayout.Button("Generate room")) {
            creator.GenerateRoom();
        }
    }
}
