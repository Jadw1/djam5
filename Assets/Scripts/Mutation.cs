using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MutationType {
    WeaponMutationType,
    StatsMutationType
}

public enum EffectType {
    Positive,
    Neutral,
    Negative
}

public abstract class Mutation : ScriptableObject {
    public String name;
    public String description;

    public MutationType type;
    public EffectType effect;
}