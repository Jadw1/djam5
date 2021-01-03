using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;

public class RoomCreator : MonoBehaviour {
    public RoomElement[] elements;
    public int minDepth = 5;
    public int maxDepth = 7;
    public int seed = 0;

    private GameObject testRoom;

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
    
    public GameObject CreateRoom() {
        if (seed == 0) {
            seed = (int)System.DateTime.Now.Ticks;
        }
        Random rng = new Random(seed);
        
        GameObject room = new GameObject("Room");
        Stack<StackElement> stack = new Stack<StackElement>();

        RoomElement enter = elements
            .FirstOrDefault(e => e.type == ElementType.ENTER);
        Instantiate(enter, room.transform);
        CrossingEntity crossing = enter.crossings[0];
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

            if (nextEl.type == ElementType.CROSSING) {
                CrossingEntity ce = nextEl.crossings[0];
                
                StackElement newStackElement = new StackElement();
                newStackElement.depth = el.depth + 1;
                newStackElement.direction = CalculateDirection(el.direction, ce.direction);
                newStackElement.entryPoint = tr.localPosition + ce.crossing.localPosition;
                
                stack.Push(newStackElement);
            }
        }

        return room;
    }

    public void Test() {
        testRoom = CreateRoom();
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
