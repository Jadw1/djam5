using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mutation", menuName = "Game/Stats Mutation")]
public class StatsMutation : Mutation {
    public PlayerStatsDelta modifiers;
}