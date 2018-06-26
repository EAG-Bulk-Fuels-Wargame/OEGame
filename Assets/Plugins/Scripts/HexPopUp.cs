using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexPopUp : MonoBehaviour
{

    public GameObject ActionPopupPrefab;
    GameObject ActionPopupPrefabClone;
    string dropdownVal = "";
    string actionName = "";

    public void Commit(string s)
    {
        dropdownVal = s;
        //Debug.Log("Choice is: " + dropdownVal);
    }

    public void Submit()
    {
        //Debug.Log("Choice is: " + dropdownVal);
        Destroy(ActionPopupPrefabClone);
        ActionProcess.runAction(actionName, dropdownVal);
    }

    void Update()
    {
        if (
            Input.GetMouseButton(0) &&
            !EventSystem.current.IsPointerOverGameObject()
        )
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        //if (Input.GetMouseButton(0) && GameObject.Find("ActionPopUp(Clone)") == null)
        //{
        //    List<string> m_DropOptions = new List<string> { "Kill", "Defend", "Fight", "Sleep", "Battle" };
        //    CreatePopup(new Vector2(0,0), m_DropOptions, "unitScript");
        //}
    }

    public void CreatePopup(Vector2 loc, List<string> s, string action) {
        ActionPopupPrefabClone = Instantiate(ActionPopupPrefab, transform.position, Quaternion.identity) as GameObject;//transform.position
        Image popUpBox = GameObject.FindWithTag("actionPopUpBox").GetComponent<Image>();
        actionName = action;
        //Vector2 v;
        //v.x = -100;
        //v.y = -100;
        popUpBox.GetComponent<RectTransform>().anchoredPosition = loc;
        //Debug.Log(m_Rect);
        //float m_XAxis = 0.5f;
        //float m_YAxis = 0.5f;
        //GUI.Label(new Rect(0, 20, 150, 80), "Anchor Position X : ");
        //GUI.Label(new Rect(300, 20, 150, 80), "Anchor Position Y : ");
        //m_XAxis = GUI.HorizontalSlider(new Rect(150, 20, 100, 80), m_XAxis, -50.0f, 50.0f);
        //m_YAxis = GUI.HorizontalSlider(new Rect(450, 20, 100, 80), m_YAxis, -50.0f, 50.0f);
        //m_Rect.anchoredPosition = new Vector2(m_XAxis, m_YAxis);
        //popUpBox.rectTransform.SetPositionAndRotation(pos, Quaternion.identity);
        //popUpBox.transform.TransformPoint(800, 800, 0);
        Dropdown m_Dropdown= GameObject.FindWithTag("dropper").GetComponent<Dropdown>();
        m_Dropdown.ClearOptions();
        m_Dropdown.AddOptions(s);
    }

} 

