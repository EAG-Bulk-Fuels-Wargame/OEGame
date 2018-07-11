using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour {

    private float fuel;
    private float propaganda;
    private float happiness;
    private int citycount;
    public HexCell hexes;
    private HexGrid hexcheck;
    private string name;
    private bool isfortified;
    public GameObject Cityp;
    private int health;


    public City(string nam, float fuelamt, float prop, float hap, int count, HexCell inti, Vector3 vec)
    {
        name = nam;
        fuel = fuelamt;
        propaganda = prop;
        happiness = hap;
        citycount = 1; 
        HexCell[] hexes = new HexCell[count];
        hexes[0] = inti;
        isfortified = false;
    }

    public float GetFuel()
    {
        return 10;
    }

    public float GetPropaganda()
    {
        return 10;
    }

    public float GetHappiness()
    {
        return 10;
    }

}
