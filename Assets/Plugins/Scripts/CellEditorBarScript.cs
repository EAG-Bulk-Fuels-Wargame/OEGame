using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellEditorBarScript : MonoBehaviour {

    private HexCell currentHex;
    private InputField[] Fields;
    public Text[] Words;
    public string name;
    public string fuelRegion;

    private void Update()
    {
        //Creates text size and location
        Image s = GameObject.FindWithTag("UIBar").GetComponent<Image>();
        Fields = s.GetComponentsInChildren<InputField>();
        Words = s.GetComponentsInChildren<Text>();
        s.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 16);
        s.rectTransform.position = new Vector2(Screen.width / 2, Screen.height / 16);
        float height = s.rectTransform.sizeDelta.y;
        float width = s.rectTransform.sizeDelta.x;
        Words[0].fontSize = 20;
        Words[1].fontSize = 20;
        if ((Words[0].preferredWidth) * 1.4 < width / 2)
        {
            Words[0].fontSize += 2;
        }
        if (Words[0].preferredWidth > width / 16)
        {
            Words[0].fontSize -= 2;
        }
        //Cell Clicked

        if (GetMouseCell() != null && Input.GetMouseButton(0))
        {
            currentHex = GetMouseCell();
            Fields[0].text = name = currentHex.name;
            Debug.Log(currentHex.name);
            Debug.Log(Words[0].text);
            Fields[1].text = fuelRegion = currentHex.fuelRegion;

            //UnitAction(GetMouseCell(),new Unit());
        }
    }
    public void SetName()
    {
        currentHex.name = Fields[0].text;
    }
    public void SetFuelRegion()
    {
        currentHex.fuelRegion = Fields[1].text;
    }

    public HexCell GetMouseCell()
    {
        //returns the HexCell clicked by the mouse
        HexGrid hexgr = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        HexGridChunk hexch = GameObject.FindWithTag("Chunk").GetComponent<HexGridChunk>();

        HexCell h = null;
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit) && hexgr != null && Input.GetMouseButton(0))
        {
            if (currentHex != null)
            {
                currentHex.ResetColor();
            }
            h = hexgr.GetCell(hit.point);
            Color c = h.Color;
            c.r *= 1.2F;
            c.g *= 1.2F;
            c.b *= 1.2F;
            h.Color = c;
            currentHex = h;
            hexch.Refresh();
        }
        return h;
    }
}
