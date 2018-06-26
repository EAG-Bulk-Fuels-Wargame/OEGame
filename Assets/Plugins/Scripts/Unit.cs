using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    Vector3 position;
    string unitClass;
    int health;
    int strength;

    private void Start()
    {
        unitClass = "land";
        health = 10;
        strength = 10;
    }

    public string GetUnitClass() {
        return unitClass;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetStrength()
    {
        return strength;
    }

    void moveAct()
    {

    }
}
