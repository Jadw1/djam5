using System;
using  UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public interface IProject {
    string Name { get; set; }
    DateTime Start { get; set; }
    DateTime End { get; set; }
}

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

    public int GetLongest(List<bool> values) {
        int length = 0;
        foreach (var value in values) {
            length = (value) ? (length + 1) : 0;
        }

        return length;
    }

    public bool IsProjectPossible(List<IProject> existing, IProject project) {
        //I assume that all projects are valid, that means project.Start < project.End
        foreach (var existingProject in existing) {
            if (project.Start.Day <= existingProject.End.Day && existingProject.Start.Day <= project.End.Day) {
                return false;
            }
        }

        return true;
    }
}
