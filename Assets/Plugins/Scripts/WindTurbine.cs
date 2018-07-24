using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTurbine : MonoBehaviour {
   
    private float Effectiveness;
    private string name;

    public WindTurbine(int turbinenum)
    {
       
        Effectiveness = 100;
        name = "name" + turbinenum;

    }
    public WindTurbine(string nam, float Effectiveness)
    {
        Effectiveness = 100;
        name = nam;

    }
}
