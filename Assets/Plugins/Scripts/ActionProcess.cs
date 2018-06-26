using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionProcess : MonoBehaviour {

    public static void runAction(string actionName, string choice) {
        //create if statement asking for actionName
        //use choice to decide result accordingly
        //if (actionName.Equals("unitAction")) {
            //exampleMethod(choice)
        //}
    }

    //v = location where popup should appear (static), s = options for dropdown list
    //actionName = what string you're using to determine what action should be run
    public static void makeAction(Vector2 v, List<string> s, string actionName) {
        var popUp = new HexPopUp();
        popUp.CreatePopup(v, s, actionName);
    }

}
