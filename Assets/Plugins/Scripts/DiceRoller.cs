using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour {
    private Vector3 levec;
	public DiceRoller(Vector3 vec)
    {

        levec = vec;

    }
    //math.sqrt

        
/*
    public static List<Unit> GetNearestCity(HexCell hex, Vector3 unitpos)
    {
        List<Unit> units = new List<Unit>();
        List<City> cities = new List<City>();
        List<float> distance = new List<float>();
        
        List<HexCell> hexes = hex.GetRadiusOfCells(hex, 15);
        foreach (HexCell h in hexes)
        {
            if (h.GetCity() == true)
            {
                cities.Add(h.GetCity());


            }
                
        }
        return hex;
    }
    */
}
