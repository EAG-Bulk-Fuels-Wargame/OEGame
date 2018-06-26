using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionProcess : MonoBehaviour {

    public static void runAction(string scenarioName, string choice) {
        //create if statement asking for actionName
        //use choice to decide result accordingly
        //if (scenarioName.Equals("Air_Combat")) {
        //  exampleMethod(choice)
        //}
        if (scenarioName.Equals("Default_Combat")) {
            Debug.Log("User Choice: " + choice);
        }
    }

    //v = location where popup should appear (static), s = options for dropdown list
    //actionName = what string you're using to determine what action should be run
    public static void MakeAction(Vector2 v, List<string> s, string scenarioName) {
        Canvas popUpSystem = GameObject.FindWithTag("PopUpUI").GetComponent<Canvas>();
        HexPopUp other = (HexPopUp)popUpSystem.GetComponent(typeof(HexPopUp));
        other.CreatePopup(v, s, scenarioName);
    }

}
