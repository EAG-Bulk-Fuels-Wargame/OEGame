using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionProcess : MonoBehaviour
{

    static Unit selectedUnit;
    static InfoBarScript ibs;

    private void Start()
    {
        //ibs = GameObject.Find("InfoUI").GetComponent<InfoBarScript>();
    }

    public static void runAction(string scenarioName, string choice)
    {
        ibs = GameObject.Find("InfoUI").GetComponent<InfoBarScript>();
        //create if statement asking for actionName
        //use choice to decide result accordingly
        //if (scenarioName.Equals("Air_Action")) {
        //  exampleMethod(choice)
        //}
        if (scenarioName.Equals("Default_Action"))
        {
            Debug.Log("User Choice: " + choice);
        }
        if (scenarioName.Equals("Infantry_Action"))
        {
            switch (choice)
            {
                case "Fight":
                    Debug.Log("You have chosen fight");
                    break;
                case "Move":
                    Debug.Log("You have chosen move");
                    ibs.SetStopSearch(true);
                    UnitAct.MoveTexturize(selectedUnit);
                    break;
                case "Fortify":
                    Debug.Log("You have selected POWER DRIVE");
                    break;
                default:
                    break;

            }
        }

    }

    //v = location where popup should appear (static), s = options for dropdown list
    //actionName = what string you're using to determine what action should be run
    public void MakeAction(Unit u, Vector2 v, List<string> s, string scenarioName)
    {
        UserInteractionScript uis = GameObject.Find("InfoUI").GetComponent<UserInteractionScript>();
        selectedUnit = u;
        Canvas popUpSystem = GameObject.FindWithTag("PopUpUI").GetComponent<Canvas>();
        HexPopUp other = (HexPopUp)popUpSystem.GetComponent(typeof(HexPopUp));
        other.CreatePopup(v, s, scenarioName);
        uis.GetPlayer().SetInAction(true);
    }

}
