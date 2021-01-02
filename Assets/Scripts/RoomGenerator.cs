using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomGenerator : MonoBehaviour {

    [SerializeField] 
    private GameObject groundPrefab;
    [SerializeField] 
    private GameObject wallPrefab;

    [SerializeField]
    [Min(3)]
    private int width;
    [SerializeField]
    [Min(3)]
    private int height;

    private GameObject ground;
    private GameObject[] walls;
    

    private void CleanRoom() {
        Action<GameObject> destroyFunc;
        if (Application.isEditor) {
            destroyFunc = DestroyImmediate;
        }
        else {
            destroyFunc = Destroy;
        }
        
        if (ground) {
            destroyFunc(ground);
        }

        if (walls != null) {
            foreach (var wall in walls) {
                destroyFunc(wall);
            }

            walls = null;
        }
    }

    public void GenerateRoom() {
        CleanRoom();

        ground = Instantiate(groundPrefab, transform);
        ground.transform.localScale = new Vector3(width, 1f, height);
        ground.transform.localPosition = new Vector3((float)width/2, 0f, (float)height/2);
        ground.transform.name = "Ground";
        
        
        //0-left 1-right 2-bottom 3-top
        walls = new GameObject[4];
        for (int i = 0; i < walls.Length; i++) {
            walls[i] = Instantiate(wallPrefab, transform);
            walls[i].transform.name = $"Wall {i}";
        }

        //place walls
        walls[0].transform.localPosition = new Vector3(.5f, 1f, (float)height/2);
        walls[1].transform.localPosition = new Vector3(width - .5f, 1f, (float)height/2);
        walls[2].transform.localPosition = new Vector3((float)width/2, 1f, .5f);
        walls[3].transform.localPosition = new Vector3((float)width/2, 1f, height - .5f);
        
        //scale them to fit entire room
        walls[0].transform.localScale = new Vector3(1f, 1f, height);
        walls[1].transform.localScale = new Vector3(1f, 1f, height);
        walls[2].transform.localScale = new Vector3(width - 2f, 1f, 1f);
        walls[3].transform.localScale = new Vector3(width - 2f, 1f, 1f);

        NavMeshSurface surface = transform.GetComponentInChildren<NavMeshSurface>();
        surface.BuildNavMesh();

    }
}
