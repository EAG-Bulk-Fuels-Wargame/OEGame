using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class City : MonoBehaviour {

    private float fuel;
  //private float propaganda;
    private float unrest;
    private int citycount;
    public HexCell hexes;
    private HexGrid hexcheck;
    private string name;
    private bool isfortified;
    public GameObject city;
    public Vector3 position;
    private List<HexCell> hexList;
    private int x;

    //  private int health;

    public City(string nam, float fuelamt, float chaos, int count, HexCell h)
    {
        List<HexCell> hexList = new List<HexCell>();
        hexList.Add(h);
        GameObject City = Resources.Load("CityP") as GameObject;
        // cityInstance.transform.localPosition = vec;
        //  cityInstance.transform.SetParent(transform);
        name = nam;
        fuel = fuelamt;
        unrest = chaos;
        citycount = 1; 
        //  HexCell[] hexes = new HexCell[count];
        // hexes[0] = inti;
        isfortified = false;
       //position = vec;
       // position.x = transform.position.x;
       //position.y = transform.position.y;
       // position.z = transform.position.z - 10;
       //transform.localPosition = vec;
       //  transform.SetParent(transform);

    }

    public string GetName()
    {

        return name;

    }

    public void AddTile(HexCell hex)
    {

        hexList.Add(hex);

    }

    public void CityMerge(City Added)
    {
        //add stuff to average out values

        Destroy(Added);

    }



    public void CombineHexLists(List<HexCell> newList)
    {

        hexList = (List<HexCell>)hexList.Union(newList);

    }

    public List<HexCell> GetHexes()
    {

        return hexList;

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
