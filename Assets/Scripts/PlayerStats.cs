using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlayerStatsDelta {
    public float health;
    public float maxHealth;
    public float speed;
    public float size;
    public float strength;
    public float dexterity;
}

public class PlayerStats : Stats {

    public List<Mutation> mutations;
    
    public Mutation mutation1;
    public Mutation mutation2;
    public Mutation mutation3;


    private void Start() {
        mutations = new List<Mutation>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.B)) {
            ApplyMutation(mutation1);
        }
        else if (Input.GetKeyDown(KeyCode.N)) {
            ApplyMutation(mutation2);
        }
        else if (Input.GetKeyDown(KeyCode.M)) {
            ApplyMutation(mutation3);
        }
    }

    public void SetWeaponType(WeaponType weapon) {
        //this wont be used very often
        GetComponent<Weapon>().SetWeaponType(weapon);
    }

    public void AddMutation(Mutation mutation) {
        ApplyMutation(mutation);
        if (mutation.type == MutationType.StatsMutationType) {
            mutations.Add(mutation);
        }
    }

    private void ApplyMutation(Mutation mutation) {
        if (mutation.type == MutationType.WeaponMutationType) {
            WeaponMutation weaponMutation = mutation as WeaponMutation;
            SetWeaponType(weaponMutation.weapon);
        }
        else if (mutation.type == MutationType.StatsMutationType) {
            StatsMutation statsMutation = mutation as StatsMutation;
            ApplyDeltaStats(statsMutation.modifiers);
        }
        Debug.Log($"Applied mutation: ${mutation.name}");
    }

    private void ApplyDeltaStats(PlayerStatsDelta delta) {
        health += delta.health;
        maxHealth += delta.maxHealth;
        speed += delta.speed;
        size += delta.size;
        strength += delta.strength;
        dexterity += delta.dexterity;
    }
    
    private void Revert(PlayerStatsDelta delta) {
        health -= delta.health;
        maxHealth -= delta.maxHealth;
        speed -= delta.speed;
        size -= delta.size;
        strength -= delta.strength;
        dexterity -= delta.dexterity;
    }
}
