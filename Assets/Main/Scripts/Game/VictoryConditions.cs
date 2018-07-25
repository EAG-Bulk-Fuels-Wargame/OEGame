using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryConditions : MonoBehaviour {

    private bool Camp;
    private bool FARP;
    private bool CAS1;
    private bool CAS2;
    private int turncounter;
    private int timeamt;

    //intializes victoryconditions
	public VictoryConditions()
    {
        turncounter = 0;
        timeamt = 7;
        Camp = false;
        FARP = false;
        CAS1 = false;
        CAS2 = false;

    }

    public VictoryConditions(bool cam, bool FAR, bool CAS11, bool CAS22, int amttime)
    {
        Camp = cam;
        FARP = FAR;
        CAS1 = CAS11;
        CAS2 = CAS22;
        turncounter = 0;
        timeamt = amttime;
    }

    //Lists victory conditions for this tile
    public string Checkvicconditions()
    {
       
         if(CAS1 == true)
        {
            return "Must successfully conduct a second airstrike from this location to defeat red team";

        }
         else if (FARP == true)
        {

            return "Must successfully conduct two airstrikes to win";
        }
      else if(Camp == true)
        {
            return "Must build a FARP to conduct CAS operations";

        }
        else
        {

            return "Must build a camp at this location before Farp construction can continue";
        }
    }

    //this method is be used at the end of every turn to check to see if blue has won
    public void checkwinstatus()
    {
        if(Camp == true && FARP == true && CAS1 == true && CAS2 == true)
        {
            //do something


        }
        else if (timeamt == turncounter)
        {

            //do something for red team

        }
        
    }
}
