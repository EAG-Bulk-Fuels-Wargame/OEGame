using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuclear : MonoBehaviour
{
    private int health;
    private float effectiveness;
    private string name;
    // Use this for initialization
    public Nuclear()
    {
        health = 100;
        effectiveness = 100;
    }
    public Nuclear(string nam, float effect)
    {
        name = nam;
        effectiveness = effect;
    }
}
