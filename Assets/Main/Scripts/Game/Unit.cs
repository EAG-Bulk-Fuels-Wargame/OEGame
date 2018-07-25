using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    GameObject unit;
    string unitModelPrefab;
    public GameObject natoUnit;
    public GameObject rebelUnit;
    GameObject unitModel;
    HexCell occupiedCell;
    private readonly string unitName;
    private bool visibility;
    private readonly int health;
    private readonly int strength;
    private readonly int stackedUnits;
    private readonly string team;
    public float effectiveness;
    int movement;
    bool blueVisibility;
    bool hidden = false;


    public Unit(HexCell hex, string t)
    {
        //GameObject go = Instantiate(Resources.Load("UnitP")) as GameObject;
        if (t == "red")
            unitModelPrefab = "terry";//give it the name of a model in the "Resources" folder
        else
        {
            unitModelPrefab = "ct1";//give it the name of a model in the "Resources" folder
        }
        occupiedCell = hex;
        hex.hasUnit = true;
        unitName = "infantry";
        health = 10;
        strength = 10;
        unitModel = CreateUnit(occupiedCell);
        stackedUnits = 1;
        team = t;
        movement = 3;
        effectiveness = 100;
        blueVisibility = true;

        if (team == "NATO")
        {

            visibility = true;

        }
        else if (team == "Insurgents" && hex.bbreakaway == true)
        {

            visibility = false;
        }
        else
        {

            visibility = true;

        }
    }

    public Unit(string ump, HexCell hex, string name, float e, int h, int str, int stk, string t, bool bv)
    {
        unitModelPrefab = ump;
        occupiedCell = hex;
        hex.hasUnit = true;
        unitName = name;
        health = h;
        strength = str;
        unitModel = CreateUnit(occupiedCell);
        stackedUnits = stk;
        team = t;
        blueVisibility = bv;
        effectiveness = e;
        if (e >= 100)
        {
            effectiveness = 100;
        }
        else if (e <= 0)
        {
            effectiveness = 1;
        }
        else
        {
            effectiveness = e;
        }

        if (team == "NATO")
        {

            visibility = true;

        }
        else if (team == "Insurgents" && hex.bbreakaway == false)
        {

            visibility = false;
        }
        else
        {

            visibility = true;

        }
    }

    private void Start()
    {
        //HexGrid hexgr = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        //occupiedCell = hexgr.cells[1];
        //unitModel = Instantiate(Resources.Load(unitModelPrefab)) as GameObject;
        //unitModel.transform.SetPositionAndRotation(position,Quaternion.identity);

    }

    private GameObject CreateUnit(HexCell h)
    {
        Vector3 pos;
        unit = Instantiate(Resources.Load(unitModelPrefab)) as GameObject;
        pos.x = unit.transform.position.x + h.transform.position.x;
        pos.y = unit.transform.position.y + h.transform.position.y;
        pos.z = unit.transform.position.z + h.transform.position.z;
        //GameObject unit = Instantiate(natoUnit, pos, Quaternion.identity) as GameObject;
        //GameObject unit = Instantiate(unitModelPrefabObject) as GameObject;
        unit.transform.localPosition = pos;
        return unit;

    }

    public void ChangeTile(HexCell hex)
    {
        Vector3 oriHexPos = occupiedCell.transform.position;
        Vector3 unitPos = unit.transform.position;
        Vector3 diff = unitPos - oriHexPos;
        occupiedCell.hasUnit = false;
        hex.hasUnit = true;
        occupiedCell = hex;
        Vector3 pos;
        pos.x = hex.transform.position.x + diff.x;
        pos.y = hex.transform.position.y + diff.y;
        pos.z = hex.transform.position.z + diff.z;
        GetModel().transform.localPosition = pos;
    }

    public List<HexCell> GetWalkableTiles(List<HexCell> h)
    {
        //write method that returns what cells a unit can move on to
        List<HexCell> walkableTiles = new List<HexCell>();
        walkableTiles = h;
        return walkableTiles;
    }

    public void Update()
    {
        if (effectiveness < 1)
            Destroy(this, 0);
    }

    public string GetUnitName()
    {
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

    public GameObject GetModel()
    {
        return unitModel;
    }

    public string GetUnitModelPrefab()
    {
        return unitModelPrefab;
    }

    public Vector3 GetLocation()
    {
        return unitModel.transform.position;
    }

    public HexCell GetUnitCell()
    {
        return occupiedCell;
    }

    public int GetStacked()
    {
        return stackedUnits;
    }

    public string GetTeam()
    {
        return team;
    }

    public int GetMovement()
    {
        return movement;
    }

    public FuelRegion GetFuelRegion()
    {       
        return occupiedCell.fuelRegion;
    }

    public void SetMovement(int m)
    {
        movement = m;
    }

    public void RefreshUnit()
    {
        movement = 3;

    }

    public string GetEnemy()
    {
        if (team == "blue")
            return "red";
        else
            return "blue";
    }


    public void SetVisible(bool b)
    {
        if (b)
        {
            unit.GetComponent<Renderer>().enabled = true;
            hidden = false;
        }
        else
        {
            unit.GetComponent<Renderer>().enabled = false;
            hidden = true;
        }
    }

    public void checkvisibility()
    {
        if (this.visibility == false)
        {
            unitModel.SetActive(false);
        }
        else
        {
            unitModel.SetActive(true);
        }
    }

    public void Resupply(City city)
    {
        effectiveness = (25 - city.Uncommod());
    }

    public float counteratt()
    {
        return effectiveness / 25;
    }

    public bool IsHidden()
    {
        return hidden;
    }

}