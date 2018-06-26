using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

    public void Button_Onclick() {
        Debug.Log("Button Clicked");//It registers
        GameObject.Find("PopUpSystem").SendMessage("Submit");
    }

}