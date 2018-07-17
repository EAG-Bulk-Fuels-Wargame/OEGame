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
    private float effectiveness;
    private int stackedUnits;
    private string team;
    private int Height;
    private Boolean terrorist;
    private bool water;
    private bool intelligence;
    private bool visibility;
    private int movement;
    private float fuelCT;
    private int fuelM;
    private int defM;


    public Unit(HexCell hex, string t)
    {
        //GameObject go = Instantiate(Resources.Load("UnitP")) as GameObject; 
        unitModelPrefab = "Temp Soldier";//give it the name of a model in the "Resources" folder
        occupiedCell = hex;
        unitName = "infantry";
       // health = 10;
      //  strength = 10;
        unitModel = CreateUnit(occupiedCell);
        stackedUnits = 1;
        team = t;
        effectiveness = 100;
        movement = 3;
        fuelCT = 0;
        fuelM = 0;
        defM = 0;

        if (team == "NATO")
        {

            visibility = true;

        }
        else if(team == "Insurgents" && hex.getBrakeaway() == true )
        {

            visibility = false;
        }
        else
        {

            visibility = true;

        }
        

    }





    public Unit(string ump, HexCell hex, string name, float e, int stk, string t, bool terry)
    {
        unitModelPrefab = ump;
        occupiedCell = hex;
        unitName = name;
       // health = h;
       // strength = str;
        unitModel = CreateUnit(occupiedCell);
        terrorist = terry;
        stackedUnits = stk;
        effectiveness = e;
        team = t;
        movement = 3;
        fuelCT = 0;
        fuelM = 0;
        defM = 0;
        if (e >= 100)
        {
            effectiveness = 100;
        }
        else if(e<=0)
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
        else if (team == "Insurgents" && hex.getBrakeaway() == true)
        {

            visibility = false;
        }
        else
        {

            visibility = true;

        }


    }

    /*
    public void strenghmodif()
    {
       // float i = this.GetStrength() * (this.GetHealth() / 100);
       
        //int z = (int)i;
      //  this.SetStrength(z);

    }
    */

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
       // GetModel().transform.localPosition = pos;
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

        public bool GetVisibility()
    {

        return visibility;
    }

    public void SetVisibility(bool x)
    {
       visibility = x;
    }
    public string GetUnitName()
    {
        return unitName;
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
    public void SetMovement(int m)
    {
        movement = m;
    }
    public float GetFuelCT()
    {
        return fuelCT;
    }
    public void SetFuelCT(float f)
    {
        fuelCT = f;
    } 

    public void SetFuelM(int f)
    {
        fuelM = f;
    }
    public int GetFuelM()
    {
        return fuelM;
    }
    public void SetDefM(int d)
    {
        defM = d;
    }
    public int GetDefM()
    {
        return defM;
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

    public float GetEffective()
    {
        return effectiveness;
    }
    public void SetEffective(float x)
    {
         effectiveness = x;
    }

    /*  public int GetHealth()
      {
          return health;
      }

      public int GetStrength()
      {
          return strength;
      }
    
      */

    public bool GetTerrorist()
    {
        return terrorist;
    } 

    public void Resupply(City city)
    {
        this.SetEffective(25 - city.Uncommod());   
    }

    public float counteratt()
    {
        return this.GetEffective() / 25;
    }
    //Aggressive Actions
    public void Comt(Unit att, Unit def,  int option)
    {
        // def.SetHealth(def.GetHealth() - (att.GetStrength()/10));
        //    att.SetHealth(att.GetHealth() - (def.GetStrength()/5));
        //need to create specific modifiers for height situation
       if(option == 0)
        {
            
            def.SetEffective(def.GetEffective() - 100000);
        }
      else if (option == 1)
        {
            //Ground Raid
            def.SetEffective(def.GetEffective() - (25 + att.rollmod()));
            att.SetEffective(att.GetEffective() - (def.counteratt()));
        }
     
        else if (option == 3)
        {
            //SOF Raid
            def.SetEffective(def.GetEffective() - (30 + att.rollmod()));
            att.SetEffective(att.GetEffective() - (def.counteratt()));
        }
        else if (option == 4)
        {
            //Air Strike
            def.SetEffective(def.GetEffective() - (70 + att.rollmod()));
        }
        else  
        {
            //UAV Strike
            def.SetEffective(def.GetEffective() - (60 + att.rollmod()) );
        }

    }

    public float rollmod()
    {
        System.Random rnd = new System.Random();
        int x = rnd.Next(1, 20);
        if (x >= 1 && x <= 5)
        {

            return -(float)(this.GetEffective() / 5);

        }
        else if (x >= 6 && x <= 10)
        {
            return -(float)(this.GetEffective() / 10);
        }
        else if (x >= 11 && x <= 15)
        {
            return -(float)(this.GetEffective()*1.2);
        }
        else if(x>=12 && x<=19)
        {
            return (float)(this.GetEffective()*1.2);
        }
        else
        {
            return (float)(this.GetEffective() * 1.5);
        }
    }

   
    //Reconnaissance method
    public void Scout(int option)
    {
        if (option == 0)
        {
            //Ground Reconnaissance

        }
        else 
        {
            //UAV Reconnaissance

        }

    }


    //Non-Aggressive actions
    public void NonAggroMoves(int option)
    {
        if(option == 0)
        {
            //Maintain Security Operations at Location

        }
        else if (option == 1)
        {
            //Reduce Security Operations at Location
            this.SetFuelCT(this.GetFuelCT() + -5000);
        }
        else if (option == 2)
        {
            //Ground Unit Relocation
            this.SetFuelCT(this.GetFuelCT() + 10000);
        }
        else if (option == 3)
        {
            //Uav/Airplane relocation
            this.SetFuelCT(this.GetFuelCT()+110000);
        }
        else if (option == 4)
        {
            //NGO Security Detail
            this.SetFuelCT(this.GetFuelCT() + 5000);
        }
        else if (option == 5)
        {
            //Construct Camp
            this.SetFuelCT(this.GetFuelCT() + 100000);
        }
        else if (option == 6)
        {
            //Establish FARP

        }
        else if (option == 7)
        {
            this.SetFuelCT(this.GetFuelCT() + 50000);
            //Repair Critical Energy Infastructure

        }
        else if (option == 8)
        {
            this.SetFuelCT(this.GetFuelCT() + 10000);
            //Public Works Project
            //reduce unrest when unrest is implemented in the U.I.
        }
        else if (option == 9)
        {
            //Allocate Fuel to Unit
            fuelCT = this.GetFuelCT() + 10000;
            fuelM = this.GetFuelM() + 1;
        }
        else if (option == 10)
        {
            //Propaganda
            if (this.rollmod() < 0)
            {
                //unrest++
            }
            else
            {
                //reduce unrest
            }

        }

    }



    //boat stuff?

    public int transcost() {

        return 10;
    }
}
