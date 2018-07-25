using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelRegion : MonoBehaviour {

    public string name;
    public int fuelQty;

    public FuelRegion(string n, int fuel)
    {
        name = n;
        fuelQty = fuel;
    }

    public void Add(int i)
    {
        fuelQty = fuelQty + i;
    }
    public void Subtract(int i)
    {
        fuelQty = fuelQty - i;
    }
    
}
