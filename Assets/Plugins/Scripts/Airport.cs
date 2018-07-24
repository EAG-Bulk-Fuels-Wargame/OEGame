using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airport : MonoBehaviour {
    private string name;
    private float effectiveness;
    private int planeAmt;
    public Airport(int num)
    {
        name = name + num;
        effectiveness = 100;
       

    }
    public Airport(string nam,float effect )
    {
        name = nam;
        effectiveness = 100;
        

    }

}
