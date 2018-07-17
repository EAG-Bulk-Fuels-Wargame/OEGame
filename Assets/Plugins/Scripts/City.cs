using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour {

    private float fuel;
    // private float propaganda;
    private float unrest;
    private int citycount;
    public HexCell hexes;
    private HexGrid hexcheck;
    private string name;
    private bool isfortified;
    public GameObject Cityp;
    public Vector3 position;
    
    //  private int health;


    public City(string nam, float fuelamt, float chaos, int count, Vector3 vec)
    {
        GameObject cityInstance = Instantiate(Cityp) as GameObject;
        cityInstance.transform.localPosition = vec;
        cityInstance.transform.SetParent(transform);
        name = nam;
        fuel = fuelamt;
        unrest = chaos;
        citycount = 1; 
      //  HexCell[] hexes = new HexCell[count];
       // hexes[0] = inti;
        isfortified = false;
        position = vec;
    }

    

    public float GetFuel()
    {
        return 10;
    }
    /* //Future method for determining the amount of hostile propaganda/materials in a city
    public float GetPropaganda()
    {
        return 10;
    }
*/
    //unrest-combat-modifier
    public float Uncommod() {

        return this.unrest/10;
    }


  //  public float GetHappiness()
   /// {
   //     return 10;
   // }

}
