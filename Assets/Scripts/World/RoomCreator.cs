using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class RoomCreator : MonoBehaviour {
    public RoomElement[] elements;
    public int minDepth = 5;
    public int maxDepth = 7;

    public GameObject playerPrefab;
    
    private int seed = 0;
    private NavMeshSurface navMesh;
    private Transform testRoom;

    struct StackElement {
        public Vector3 entryPoint;
        public Direction direction;
        public int depth;

        public StackElement(Vector3 v, Direction d, int depth) {
            entryPoint = v;
            direction = d;
            this.depth = depth;
        }
    }

    struct OutParams {
        public Transform playerSpawn;
        public List<Transform> enemySpawns;
    }

    private void Start() {
        navMesh = GetComponentInChildren<NavMeshSurface>();
        
        GenerateRoom();
    }

    private Transform CreateRoom(out OutParams parameters) {
        Random rng = new Random(seed);
        Transform room = new GameObject("Room").transform;
        Stack<StackElement> stack = new Stack<StackElement>();
        List<Transform> spawnPoints = new List<Transform>();
        
        RoomElement enter = elements
            .FirstOrDefault(e => e.type == ElementType.ENTER);
        Transform enterTrans = Instantiate(enter, room).transform;
        enterTrans.localPosition = Vector3.zero;
        
        RoomElement enterElement = enterTrans.GetComponent<RoomElement>();
        CrossingEntity crossing = enterElement.crossings[0];
        parameters.playerSpawn = enterElement.playerSpawnPoint;
        spawnPoints.AddRange(enterElement.enemySpawnPoints);

        stack.Push(new StackElement(crossing.crossing.localPosition, crossing.direction, 1));

        int branches = 1;
        bool exitCreated = false;
        while (stack.Count > 0) {
            StackElement el = stack.Pop();
            int requirements = SelectElements(el.depth, exitCreated, branches);

            RoomElement[] els = elements.Where(e => ((int) e.type & requirements) > 0).ToArray();
            RoomElement nextEl = els[rng.Next(0, els.Length)];

            Transform tr = Instantiate(nextEl, room.transform).transform;
            tr.localPosition = el.entryPoint;
            tr.localRotation = DirectionToRotation(el.direction);
            RoomElement re = tr.GetComponent<RoomElement>();
            spawnPoints.AddRange(re.enemySpawnPoints);
            

            if (nextEl.type == ElementType.CROSSING) {
                CrossingEntity ce = re.crossings[0];
                
                StackElement newStackElement = new StackElement();
                newStackElement.depth = el.depth + 1;
                newStackElement.direction = CalculateDirection(el.direction, ce.direction);
                //newStackElement.entryPoint = tr.localPosition + ce.crossing.localPosition;
                newStackElement.entryPoint = ce.crossing.position;
                
                stack.Push(newStackElement);
            }
            else if (nextEl.type == ElementType.EXIT) {
                exitCreated = true;
            }
        }

        parameters.enemySpawns = spawnPoints;
        return room;
    }

    private void RandomSeed() {
        if (seed == 0) {
            seed = (int)System.DateTime.Now.Ticks;
        }
    }
    
    public void GenerateRoom() {
        RandomSeed();
        OutParams parameters;

        
        testRoom = CreateRoom(out parameters);
        testRoom.parent = transform;
        navMesh.BuildNavMesh();

        
        
        Transform player = Instantiate(playerPrefab).transform;
        player.position = parameters.playerSpawn.position;
        
        
    }

    private int SelectElements(int depth, bool exitCreated, int branches) {
        int types = 0;
        
        if (depth < minDepth) {
            types = (int)ElementType.CROSSING;
        }
        else if (depth >= minDepth && depth < maxDepth) {
            types = (int)ElementType.CROSSING; 
            if(!exitCreated) 
                types |= (int)ElementType.EXIT;
        }
        else { 
            types = exitCreated ? (int)ElementType.DEAD_END : (int)ElementType.EXIT;
        }

        if (branches > 1)
            types |= (int)ElementType.DEAD_END;

        return types;
    }

    private static Quaternion DirectionToRotation(Direction direction) {
        float angle = 0.0f;
        switch (direction) {
            case Direction.FORWARD:
                angle = 0.0f;
                break;
            case Direction.LEFT:
                angle = 270.0f;
                break;
            case Direction.RIGHT:
                angle = 90.0f;
                break;
            case Direction.BACKWARD:
                angle = 180.0f;
                break;
        }

        return Quaternion.AngleAxis(angle, Vector3.up);
    }

    private static Direction CalculateDirection(Direction current, Direction next) {
        switch (current) {
            case Direction.FORWARD:
                return next;
            case Direction.RIGHT:
                return (next == Direction.LEFT) ? Direction.FORWARD : (Direction) ((int) next << 1);
            case Direction.BACKWARD:
                if (next == Direction.FORWARD) return Direction.BACKWARD;
                else if (next == Direction.RIGHT) return Direction.LEFT;
                else if (next == Direction.BACKWARD) return Direction.FORWARD;
                else return Direction.RIGHT;
            case Direction.LEFT:
                return (next == Direction.FORWARD) ? Direction.LEFT : (Direction) ((int) next >> 1);
        }

        return Direction.FORWARD;
    }
}
