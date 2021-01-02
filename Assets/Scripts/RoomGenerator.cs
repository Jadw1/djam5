using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

[ExecuteInEditMode]
public class RoomGenerator : MonoBehaviour {

    [SerializeField] 
    private GameObject groundPrefab;
    [SerializeField] 
    private GameObject wallPrefab;

    [Header("Random generation")] 
    
    public Vector2Int randomSize;
    public Vector2Int variation;
    public int sizeTreshold = 5;

    [Space(10)] 
    [Header("Fixed generation")] 
    [SerializeField]
    private Vector2Int size;

    private GameObject ground;
    private GameObject[] walls;
    private NavMeshSurface surface;

    private void Start() {
        surface = transform.GetComponentInChildren<NavMeshSurface>();
    }

    public void CleanRoom() {
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
        surface.BuildNavMesh();
    }

    public void GenerateRoom() {
        CleanRoom();

        ground = Instantiate(groundPrefab, transform);
        ground.transform.localScale = new Vector3(size.x, 1f, size.y);
        ground.transform.localPosition = new Vector3((float)size.x/2, 0f, (float)size.y/2);
        ground.transform.name = "Ground";
        
        
        //0-left 1-right 2-bottom 3-top
        walls = new GameObject[4];
        for (int i = 0; i < walls.Length; i++) {
            walls[i] = Instantiate(wallPrefab, transform);
            walls[i].transform.name = $"Wall {i}";
        }

        //place walls
        walls[0].transform.localPosition = new Vector3(.5f, 1f, (float)size.y/2);
        walls[1].transform.localPosition = new Vector3(size.x - .5f, 1f, (float)size.y/2);
        walls[2].transform.localPosition = new Vector3((float)size.x/2, 1f, .5f);
        walls[3].transform.localPosition = new Vector3((float)size.x/2, 1f, size.y - .5f);
        
        //scale them to fit entire room
        walls[0].transform.localScale = new Vector3(1f, 1f, size.y);
        walls[1].transform.localScale = new Vector3(1f, 1f, size.y);
        walls[2].transform.localScale = new Vector3(size.x - 2f, 1f, 1f);
        walls[3].transform.localScale = new Vector3(size.x - 2f, 1f, 1f);
        
        surface.BuildNavMesh();

    }
    
    public void GenerateRandomRoom() {
        Vector2Int localVariation = new Vector2Int(
            UnityEngine.Random.Range(-variation.x, variation.y + 1),
            UnityEngine.Random.Range(-variation.y, variation.y + 1));
        
        size = randomSize + localVariation;
        if (size.x < sizeTreshold) {
            size.x = sizeTreshold;
        }
        if (size.y < sizeTreshold) {
            size.y = sizeTreshold;
        }
        
        GenerateRoom();
    }
}
