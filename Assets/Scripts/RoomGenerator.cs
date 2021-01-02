using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomGenerator : MonoBehaviour {
    
    [SerializeField]
    [Min(3)]
    private int width;
    
    [SerializeField]
    [Min(3)]
    private int height;

    private GameObject ground;
    private GameObject[] walls;
    
    // Start is called before the first frame update
    void Start() {
        
    }

    private Vector3 GridToPosition(int x, int y, float height = 0f) {
        return new Vector3(x + .5f, height, y + .5f);
    }

    private void CleanRoom() {
        if (ground) {
            Destroy(ground);
        }

        if (walls != null) {
            foreach (var wall in walls) {
                Destroy(wall);
            }

            walls = null;
        }
    }

    public void GenerateRoom() {
        CleanRoom();
        
        ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.transform.localScale = new Vector3(width, 1f, height);
        ground.transform.localPosition = new Vector3((float)width/2, 0f, (float)height/2);
        ground.transform.parent = transform;
        ground.transform.name = "Ground";
        
        
        //0-left 1-right 2-bottom 3-top
        walls = new GameObject[4];
        for (int i = 0; i < walls.Length; i++) {
            walls[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            walls[i].transform.parent = transform;
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
