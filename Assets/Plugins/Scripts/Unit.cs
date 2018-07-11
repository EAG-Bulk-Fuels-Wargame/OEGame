using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    string unitModelPrefab;
    //public GameObject unitModelPrefabObject;
    GameObject unitModel;
    HexCell occupiedCell;
    public string unitName;
    private int health;
    private int strength;
    private int stackedUnits;
    private string team;
    private int Height;
    private Boolean terrorist;
    private bool water;


    public Unit(HexCell hex, string t)
    {
        //GameObject go = Instantiate(Resources.Load("UnitP")) as GameObject; 
        unitModelPrefab = "Temp Soldier";//give it the name of a model in the "Resources" folder
        occupiedCell = hex;
        unitName = "infantry";
        health = 10;
        strength = 10;
        unitModel = CreateUnit(occupiedCell);
        stackedUnits = 1;
        team = t;
    }



    public Unit(string ump, HexCell hex, string name, int h, int str, int stk, string t, bool terry)
    {
        unitModelPrefab = ump;
        occupiedCell = hex;
        unitName = name;
        health = h;
        strength = str;
        unitModel = CreateUnit(occupiedCell);
        terrorist = terry;
        stackedUnits = stk;
        team = t;
    }
    public void strenghmodif()
    {
        float i = this.GetStrength() * (this.GetHealth() / 100);
        int z = (int)i;
        this.SetStrength(z);

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
        pos.x = h.transform.position.x;
        pos.y = h.transform.position.y;
        pos.z = h.transform.position.z;
        Height = (int) pos.y;
        GameObject unit = Instantiate(Resources.Load(unitModelPrefab)) as GameObject;
        //GameObject unit = Instantiate(unitModelPrefabObject) as GameObject;
        unit.transform.localPosition = pos;
        return unit;

    }

    public void ChangeTile(HexCell hex)
    {
        Vector3 pos;
        pos.x = hex.transform.position.x;
        pos.y = hex.transform.position.y;
        pos.z = hex.transform.position.z;
        GetModel().transform.localPosition = pos;
    }

    public List<HexCell> GetWalkableTiles(List<HexCell> h)
    {
        //write method that returns what cells a unit can move on to
        List<HexCell> walkableTiles = new List<HexCell>();
        HexDirection dir = HexDirection.E;
        for (int i = 0; i < 6; i++)
        {
            walkableTiles.Add(occupiedCell.GetNeighbor(dir));
            dir = dir.Next();
        }
        return walkableTiles;
    }

    public void Update()
    {

    }
    // get and set methods
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

    public Vector3 GetLocation()
    {
        return unitModel.transform.position;
    }

    public int GetHeight()
    {
        return Height;
    }

    public void SetUnitName(string n)
    {
        this.name = n;
    }

    public void SetHealth(int x)
    {
        this.health = x;
    }

    public void SetStrength(int x)
    {
        this.strength = x;
    }

    public int GetStacked()
    {
        return stackedUnits;
    }

    public string GetTeam()
    {
        return team;
    }

    public HexCell GetUnitCell()
    {
        return occupiedCell;
    }

    public string GetUnitModelPrefab()
    {
        return unitModelPrefab;
    }

    public bool GetTerrorist()
    {
        return terrorist;
    }


    public void Comt(Unit att, Unit def)
    {
        // def.SetHealth(def.GetHealth() - (att.GetStrength()/10));
        //    att.SetHealth(att.GetHealth() - (def.GetStrength()/5));
        //need to create specific modifiers for height situation
        /*
         * 
         * 
         *
         * 
         * 
         * 
         *
         */
        if (att.unitName == def.unitName)
        {
            if(att.GetHeight() == def.GetHeight())
            {
                def.SetHealth(def.GetHealth() - (att.GetStrength() / 10));
                    att.SetHealth(att.GetHealth() - (def.GetStrength()/5));
                att.strenghmodif();
                def.strenghmodif();
            }
            else if (att.GetHeight() > def.GetHeight())
            {

                def.SetHealth(def.GetHealth() - (att.GetStrength() / 10));
                   att.SetHealth(att.GetHealth() - (def.GetStrength()/5));
                att.strenghmodif();
                def.strenghmodif();
            }
            else
            {
                def.SetHealth(def.GetHealth() - (att.GetStrength() / 10));
                    att.SetHealth(att.GetHealth() - (def.GetStrength()/5));
                att.strenghmodif();
                def.strenghmodif();


            }

        }
        //this is if a bomber is attacking an insurgent unit
        else
        {
            def.SetHealth(def.GetHealth() - att.GetStrength());
            att.strenghmodif();
            def.strenghmodif();
        }


    }
    //not sure if this should be a float?
    //this method is kind of useless but may be used later in the game to simulate different forms of transportation for the units
    //and the associated costs
    public int transcost() {



        return 10;
    }
}
