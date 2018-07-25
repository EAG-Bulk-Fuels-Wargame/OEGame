using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndriodDebugScript : MonoBehaviour {

    
    static string boxText = "";

	// Use this for initialization
	void Start () {
        Text box = GameObject.FindWithTag("AndroidDebugText").GetComponent<Text>();
        //Image imageBox = GameObject.FindWithTag("AndroidDebugText").GetComponent<Image>();
        //imageBox.rectTransform.position.Set(Screen.width,Screen.height,0);

        SetText("Debug Log: ");
    }

    public static void SetText(string s) {
        boxText = s;
    } 

	// Update is called once per frame
	void Update () {
        Text box = GameObject.FindWithTag("AndroidDebugText").GetComponent<Text>();
        box.text = boxText;
	}
}
