using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    string unitModelPrefab;
    //public GameObject unitModelPrefabObject;
    GameObject unitModel;
    HexCell occupiedCell;
    private readonly string unitName;
    private readonly int health;
    private readonly int strength;

    public Unit(HexCell hex) {
        //GameObject go = Instantiate(Resources.Load("UnitP")) as GameObject; 
        unitModelPrefab = "Temp Soldier";//give it the name of a model in the "Resources" folder
        occupiedCell = hex;
        unitName = "infantry";
        health = 10;
        strength = 10;
        unitModel = CreateUnit(occupiedCell);
    }

    public Unit(string ump, HexCell hex, string name, int h, int str)
    {
        unitModelPrefab = ump;
        occupiedCell = hex;
        unitName = name;
        health = h;
        strength = str;
        unitModel = CreateUnit(occupiedCell);
    }

    private void Start()
    {
        //HexGrid hexgr = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        //occupiedCell = hexgr.cells[1];
        //unitModel = Instantiate(Resources.Load(unitModelPrefab)) as GameObject;
        //unitModel.transform.SetPositionAndRotation(position,Quaternion.identity);
        
    }

    private GameObject CreateUnit(HexCell h) {
        Vector3 pos;
        pos.x = h.transform.position.x;
        pos.y = h.transform.position.y;
        pos.z = h.transform.position.z;
        GameObject unit = Instantiate(Resources.Load(unitModelPrefab)) as GameObject;
        //GameObject unit = Instantiate(unitModelPrefabObject) as GameObject;
        unit.transform.localPosition = pos;
        return unit;

    }

    public void Update()
    {
    
    }

    public string GetUnitName() {
        return unitName;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetStrength()
    {
        return strength;
    }

    public GameObject GetModel() {
        return unitModel;
    }

    public Vector3 GetLocation() {
        return unitModel.transform.position;
    }

}
