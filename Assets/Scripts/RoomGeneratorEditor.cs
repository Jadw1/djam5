using  UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(RoomGenerator))]
public class RoomGeneratorEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        RoomGenerator generator = (RoomGenerator) target;
        if (GUILayout.Button("Generate random room")) {
            generator.GenerateRandomRoom();
        }
        
        if (GUILayout.Button("Clear")) {
            generator.CleanRoom();
        }
        
        if(GUILayout.Button("Generate room")) {
            generator.GenerateRoom();
        }
    }
}
