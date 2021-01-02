using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats {

    public void SetWeaponType(WeaponType weapon) {
        //this wont be used very often
        GetComponent<Weapon>().SetWeaponType(weapon);
    }
}
