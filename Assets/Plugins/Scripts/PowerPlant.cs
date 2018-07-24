using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlant : MonoBehaviour {

    private float effectiveness;
    private string name;

    public PowerPlant(int num)
    {
        effectiveness = 100;
        name = "powerplant" + num;

    }
    public PowerPlant(string nam, float effect)
    {
        effectiveness = effect;
        name = nam;

    }

}
