using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depot : MonoBehaviour {

    private float effectiveness;
    private string name;

    public Depot(int num)
    {
        name = "name" + num;
        effectiveness = 100;

    }

    public Depot(string nam, float effect)
    {
        name = nam;
        effectiveness = effect;
    }

}
