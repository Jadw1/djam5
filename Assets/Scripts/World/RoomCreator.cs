using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public enum GenerationProgress {
    START,
    WIP,
    ERROR,
    END,
    IDLE
}

public class RoomCreator : MonoBehaviour {
    public float waitFor = 0.1f;
    public RoomElement[] elements;
    public int minDepth = 5;
    public int maxDepth = 7;

    public GameObject playerPrefab;
    
    public int seed = 0;
    private NavMeshSurface navMesh;
    private Transform testRoom;
    private EnemySpawner spawner;

    private bool generationFailed = false;

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
        spawner = GetComponent<EnemySpawner>();
        
        GenerateRoom();
    }
    
    private Transform newRoom;
    private OutParams newParams;
    
    private Transform currentRoom;
    private OutParams currentParams;
    
    private Transform oldRoom;
    private OutParams oldParams;

    private bool generateRoom;
    private GenerationProgress progress = GenerationProgress.IDLE;

    private int counter;
    private void Update() {
        if(!generateRoom)
            return;

        if (progress == GenerationProgress.START) {
            RandomSeed();
            Debug.Log("Starting new generation.");

            newParams = new OutParams {enemySpawns = new List<Transform>()};

            progress = GenerationProgress.WIP;
            counter++;
            StartCoroutine(CreateRoom());
        }
        else if (progress == GenerationProgress.WIP) {
            
        }
        else if (progress == GenerationProgress.ERROR) {
            Destroy(newRoom.gameObject);
            Debug.Log("GENERATION ERROR!!!");
            progress = GenerationProgress.START;
        }
        else if (progress == GenerationProgress.END) {
            if (generationFailed) {
                progress = GenerationProgress.ERROR;
                return;
            }
            
            navMesh.BuildNavMesh();
            
            //spawner.SpawnEnemies(newParams.enemySpawns.ToArray(), 0.2f);

            generateRoom = false;
            progress = GenerationProgress.IDLE;
            Debug.Log($"Generation ended in {counter} attempts.");

            if (firstLevel) {
                LoadFirstLevel();
            }
        }
        
    }

    private IEnumerator CreateRoom() {
        Random rng = new Random(seed);
        newRoom = new GameObject("New Room").transform;
        newRoom.parent = transform;
        newRoom.position = new Vector3(0.0f, 200.0f, 0.0f);
        Stack<StackElement> stack = new Stack<StackElement>();
        generationFailed = false;


        RoomElement enter = elements
            .FirstOrDefault(e => e.type == ElementType.ENTER);
        Transform enterTrans = Instantiate(enter, newRoom).transform;
        enterTrans.localPosition = Vector3.zero;
        
        RoomElement enterElement = enterTrans.GetComponent<RoomElement>();
        enterElement.RegisterElement(ElementCollision);
        CrossingEntity crossing = enterElement.crossings[0];
        newParams.playerSpawn = enterElement.playerSpawnPoint;
        newParams.enemySpawns.AddRange(enterElement.enemySpawnPoints);

        stack.Push(new StackElement(crossing.crossing.localPosition, crossing.direction, 1));

        int branches = 1;
        bool exitCreated = false;
        while (stack.Count > 0) {
            if (generationFailed) {
                progress = GenerationProgress.ERROR;
                yield break;
            }
            
            StackElement el = stack.Pop();
            int requirements = SelectElements(el.depth, exitCreated, branches);

            RoomElement[] els = elements.Where(e => ((int) e.type & requirements) > 0).ToArray();
            RoomElement nextEl = els[rng.Next(0, els.Length)];

            Transform tr = Instantiate(nextEl, newRoom.transform).transform;
            Vector3 pos = new Vector3(el.entryPoint.x, 0.0f, el.entryPoint.z);
            tr.localPosition = pos;
            tr.localRotation = DirectionToRotation(el.direction);
            RoomElement re = tr.GetComponent<RoomElement>();
            re.RegisterElement(ElementCollision);
            newParams.enemySpawns.AddRange(re.enemySpawnPoints);
            

            if (nextEl.type == ElementType.CROSSING) {
                if (re.crossings.Count > 1) {
                    branches += re.crossings.Count - 1;
                }

                foreach (var ce in re.crossings) {
                    StackElement newStackElement = new StackElement();
                    newStackElement.depth = el.depth + 1;
                    newStackElement.direction = CalculateDirection(el.direction, ce.direction);
                    //newStackElement.entryPoint = tr.localPosition + ce.crossing.localPosition;
                    newStackElement.entryPoint = ce.crossing.position;
                
                    stack.Push(newStackElement);
                }
            }
            else if (nextEl.type == ElementType.EXIT) {
                exitCreated = true;
            }

            if (nextEl.type == ElementType.EXIT || nextEl.type == ElementType.DEAD_END) {
                branches--;
            }
            
            //yield return new WaitForSeconds(waitFor);
            yield return new WaitForFixedUpdate();
        }
        
        //yield return new WaitForSeconds(waitFor);
        yield return new WaitForFixedUpdate();
        
        progress = GenerationProgress.END;
    }

    private void RandomSeed() {
        seed = (int)DateTime.Now.Ticks;
    }
    
    public void GenerateRoom() {
        if (generateRoom) {
            Debug.Log("Already generating!");
            return;
        }

        generateRoom = true;
        progress = GenerationProgress.START;
        counter = 0;
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

    private void ElementCollision() {
        generationFailed = true;
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

    private bool firstLevel = true;
    public int levelCounter = 0;
    private void LoadFirstLevel() {
        currentRoom = newRoom;
        currentParams = newParams;
        
        currentRoom.name = "Room";
        currentRoom.localPosition = new Vector3();
        
        Transform player = Instantiate(playerPrefab).transform;
        player.position = newParams.playerSpawn.position;

        levelCounter++;
        firstLevel = false;
        
        GenerateRoom();
    }

    public bool LoadNextLevel(Transform origin) {
        if (progress != GenerationProgress.IDLE) {
            return false;
        }
        
        oldRoom = currentRoom;
        oldParams = newParams;
        oldRoom.name = "Old Room";

        currentRoom = newRoom;
        currentParams = newParams;
        currentRoom.name = "Room";

        DeactivateDoor(currentRoom);
        currentRoom.position = new Vector3(origin.position.x, 0.0f, origin.position.z);
        currentRoom.rotation = origin.rotation;
        
        GenerateRoom();
        return true;
    }

    public void DestroyOldLevel() {
        ActivateDoor(currentRoom);
        StartCoroutine(DestroyOld());
    }

    private void ActivateDoor(Transform room) {
        RoomElement el = room.GetChild(0).GetComponent<RoomElement>();
        el.ActivateDoor();
    }
    
    private void DeactivateDoor(Transform room) {
        RoomElement el = room.GetChild(0).GetComponent<RoomElement>();
        el.DeactivateDoor();
    }

    private IEnumerator DestroyOld() {
        yield return new WaitForSeconds(1.0f);
        Destroy(oldRoom.gameObject);
    }
}
