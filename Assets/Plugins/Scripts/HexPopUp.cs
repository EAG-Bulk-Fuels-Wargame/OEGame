using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexPopUp : MonoBehaviour
{

    public GameObject ActionPopUpPrefab;
    Image popUpBox;
    string dropdownVal = "";
    string scenarioName = "";

    //Used when user selects an option on the action toolbar dropdown list
    public void Commit(string s)
    {
        dropdownVal = s;
    }

    //Used when user submits their choice from the action toolbar dropdown list
    //Gets rid of the toolbar and sends the user's choice to the ActionProcess script
    public void Submit()
    {
        Destroy(GameObject.FindWithTag("actionPopUpBox"));
        ActionProcess.runAction(scenarioName, dropdownVal);
    }

    void Update()
    {

    }

    //Creates the action toolbar popup
    public void CreatePopup(Vector2 loc, List<string> s, string scenario) {
        Instantiate(ActionPopUpPrefab);
        popUpBox = GameObject.FindWithTag("actionPopUpBox").GetComponent<Image>();
        scenarioName = scenario;
        popUpBox.GetComponent<RectTransform>().anchoredPosition = loc;
        Dropdown m_Dropdown= GameObject.FindWithTag("dropper").GetComponent<Dropdown>();
        m_Dropdown.ClearOptions();
        m_Dropdown.AddOptions(s);
    }

} 

