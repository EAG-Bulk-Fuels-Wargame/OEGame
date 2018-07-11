using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBarScript : MonoBehaviour
{

    Text[] Words;
    HexCell currentHex;
    Unit p;
    HexGrid hexgr;
    ActionProcess scriptAP = new ActionProcess();
    bool actionOpen = false;
    bool stopSearch;

    // Use this for initialization
    void Start()
    {
        stopSearch = false;
    }

    public HexCell GetMouseCell()
    {
        //returns the HexCell clicked by the mouse
        hexgr = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        HexGridChunk hexch = GameObject.FindWithTag("Chunk").GetComponent<HexGridChunk>();
        HexCell h = null;
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit) && hexgr != null && Input.GetMouseButton(0))
        {
            if (currentHex != null)
            {
                UnselectHexCell(currentHex);
            }
            h = hexgr.GetCell(hit.point);
            SelectHexCell(h);
            currentHex = h;
        }
        return h;
    }

    public void SelectHexCell(HexCell hex)
    {
        HexGridChunk hexch = GameObject.FindWithTag("Chunk").GetComponent<HexGridChunk>();
        Color c = hex.Color;
        c.r *= 1.2F;
        c.g *= 1.2F;
        c.b *= 1.2F;
        hex.Color = c;
        hexch.Refresh();

    }

    public void UnselectHexCell(HexCell hex)
    {
        HexGridChunk hexch = GameObject.FindWithTag("Chunk").GetComponent<HexGridChunk>();
        hex.ResetColor();
        hexch.Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        //Creates text size and location
        Image s = GameObject.FindWithTag("UIBar").GetComponent<Image>();
        s.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 8);
        s.rectTransform.position = new Vector2(Screen.width / 2, Screen.height / 16);
        Words = s.GetComponentsInChildren<Text>();
        float height = s.rectTransform.sizeDelta.y;
        float width = s.rectTransform.sizeDelta.x;
        Words[0].fontSize = 20;
        if ((Words[0].preferredWidth) * 1.4 < width / 2)
        {
            Words[0].fontSize += 2;
        }
        if (Words[0].preferredWidth > width / 8)
        {
            Words[0].fontSize -= 2;
        }

        //Cell Clicked
        if (!stopSearch)
            if (Input.GetMouseButton(0) && !actionOpen && GetMouseCell() != null)
            {
                if (UnitAct.HasUnit(GetMouseCell()))
                    Words[0].text = ("<b>Name:</b> " + GetMouseCell().name + " \t <b>Building:</b> " + GetMouseCell().building + "\n <b>Terrain:</b> " + GetTerrain(GetMouseCell().Color) + " \t <b>Unit:</b> " + UnitAct.GetUnit(GetMouseCell()).GetUnitName() + " (" + UnitAct.GetUnit(GetMouseCell()).GetStacked() + ")");
                else
                    Words[0].text = ("<b>Name:</b> " + GetMouseCell().name + " \t <b>Building:</b> " + GetMouseCell().building + "\n <b>Terrain:</b> " + GetTerrain(GetMouseCell().Color));
                //if (UnitAct.HasUnit(GetMouseCell()))
                //{
                //    ChangeAction();
                //    UnitAction(GetMouseCell(), UnitAct.GetUnit(GetMouseCell()));
                //}
                //p.ChangeTile(hexgr.cells[GetMouseCell().cellNum]);
                //UnitAction(GetMouseCell(),new Unit());
            }

    }

    public void UnitAction(HexCell h, Unit u)
    {
        //Call when a unit action should take place and bring up a popup
        Vector2 vector = new Vector2(0, 0);
        scriptAP.MakeAction(u, vector, GetOptions(h, u), GetScenarioName(h, u));
    }

    public List<string> GetOptions(HexCell h, Unit u)
    {
        List<string> s = new List<string>();
        //add some conditions for what should be added to the options list
        //i.e. "Embark" only available if on coastal tile.
        if (u.GetUnitName() == "infantry")
        {
            s.Add("Fight");
            s.Add("Move");
            s.Add("Fortify");
        }
        else
        {
            s.Add("Fight");
            s.Add("Move");
            s.Add("Fortify");
        }
        return s;
    }

    public string GetScenarioName(HexCell h, Unit u)
    {
        string s;
        string unitName = u.GetUnitName();
        switch (unitName)
        {
            case "infantry":
                s = "Infantry_Action";
                break;
            default:
                s = "Default_Action";
                break;
        }
        //add some conditions to see what scenario the unit's interaction is.
        //i.e. An air unit's version of "Fight" will be different from a land unit.
        //so call the scenario something like "Air_Action"
        return s;
    }

    public string GetTerrain(Color col)
    {
        //Changes the color of the tile selected to be brighter and returns the
        //terrain type of the tile.
        List<Color> terrainType = new List<Color>(9)
        {
            new Color(.61F, .7F, .5F, 1),
            new Color(1, .6F, .6F, 1),
            new Color(.2F, .2F, 1F, 1),
            new Color(.5F, .5F, .5F, 1),
            new Color(1F, .54F, 0, 1),
            new Color(1F, 1F, 0, 1),
            new Color(1F, .75F, .80F, 1),
            new Color(.2F, 1F, 1F, 1),
            new Color(.3F, .3F, 1F, 1)
        };
        List<string> terrainName = new List<string>(9)
        {
            "grass",
            "breakaway",
            "ocean",
            "city",
            "power",
            "nuclear",
            "fuel",
            "airport",
            "pipeline"
        };
        col.r /= 1.2F;
        col.g /= 1.2F;
        col.b /= 1.2F;
        for (int i = 0; i < terrainType.Count; i++)
        {
            if (col == terrainType[i])
            {
                return (terrainName[i]);
            }
        }
        return "";
    }

    public void SetStopSearch(bool b)
    {
        stopSearch = b;
        if (currentHex != null)
            UnselectHexCell(currentHex);
    }
}
