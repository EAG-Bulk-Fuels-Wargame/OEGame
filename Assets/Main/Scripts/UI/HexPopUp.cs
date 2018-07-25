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
    GameObject fullPopUp;

    //Used when user selects an option on the action toolbar dropdown list
    public void Commit(string s)
    {
        dropdownVal = s;
    }

    //Used when user submits their choice from the action toolbar dropdown list
    //Gets rid of the toolbar and sends the user's choice to the ActionProcess script
    public void Submit()
    {
        //GameObject h = GameObject.FindWithTag("actionPopUpBox");
        //h.tag = "junk_tag";
        //Debug.Log(h.tag);
        Destroy(fullPopUp);
        //Object.Destroy(GameObject.FindWithTag("actionPopUpBox"));
        ActionProcess.runAction(scenarioName, dropdownVal);
        //Destroy(GameObject.FindWithTag("actionPopUpBox"));
        //GameObject.FindWithTag("actionPopUpBox").SetActiveRecursively(false);

    }

    public void Cancel()
    {
        //GameObject h = GameObject.FindWithTag("actionPopUpBox");
        //h.tag = "junk_tag";
        //Debug.Log(h.tag);
        Destroy(fullPopUp);
        ActionProcess.runAction(scenarioName, "Cancel");
        //Object.Destroy(GameObject.FindWithTag("actionPopUpBox"));
        //Destroy(GameObject.FindWithTag("actionPopUpBox"));
        //GameObject.FindWithTag("actionPopUpBox").SetActiveRecursively(false);

    }

    void Update()
    {

    }

    //Creates the action toolbar popup
    public void CreatePopup(Vector2 loc, List<string> s, string scenario)
    {
        fullPopUp = Instantiate(ActionPopUpPrefab) as GameObject;
        popUpBox = fullPopUp.GetComponentInChildren<Image>();
        //popUpBox = a.GetComponent<Image>();
        //popUpBox = GameObject.FindWithTag("actionPopUpBox").GetComponent<Image>();
        scenarioName = scenario;
        popUpBox.GetComponent<RectTransform>().anchoredPosition = loc;
        Dropdown m_Dropdown = popUpBox.GetComponentInChildren<Image>().GetComponentInChildren<Dropdown>();
        //Dropdown m_Dropdown = GameObject.FindWithTag("dropper").GetComponent<Dropdown>();
        m_Dropdown.ClearOptions();
        m_Dropdown.AddOptions(s);
    }

    public void WriteErrorMessage(string s)
    {
        Text[] t = popUpBox.GetComponentInChildren<Image>().GetComponentsInChildren<Text>();
        t[1].text = s;
    }

}

