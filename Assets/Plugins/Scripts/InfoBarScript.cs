using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBarScript : MonoBehaviour
{

    string uiText;
    public Text[] Words;
    HexGrid hexgrid = new HexGrid();
    HexCell currentHex;

    // Use this for initialization
    void Start()
    {
        //Debug.LogError("Screen Width : " + Screen.width);
        //Debug.LogError("Screen height: " + Screen.height);


        //Text words = GameObject.FindWithTag("UIBar").GetComponent<Text>();
        //Words[0].rectTransform.sizeDelta = new Vector2(1, 16);
        //Words[1].rectTransform.sizeDelta = new Vector2(1, 16);

    }

    public string getText()
    {
        //Vector3 posa = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //float x = posa.x;
        //float y = posa.y;
        //Debug.Log("X = " + x + " Y = " + y);
        //hexgrid.GetCell(posa);
        string s = "Hello World";
        return s;
    }

    public HexCell getMouseCell()
    {
        HexGrid hexgr = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        HexGridChunk hexCh = GameObject.FindWithTag("Chunk").GetComponent<HexGridChunk>();

        HexCell h = null;
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit) && hexgr != null && Input.GetMouseButton(0))
        {
            if (currentHex != null)
            {
                Color x = currentHex.Color;
                x.r /= 1.2F;
                x.g /= 1.2F;
                x.b /= 1.2F;
                currentHex.Color = x;
            }
            h = hexgr.GetCell(hit.point);
            Color c = h.Color;
            c.r *= 1.2F;
            c.g *= 1.2F;
            c.b *= 1.2F;
            h.Color = c;
            currentHex = h;
            hexCh.Refresh();
        }
        return h;
    }

    // Update is called once per frame
    void Update()
    {
        Image s = GameObject.FindWithTag("UIBar").GetComponent<Image>();
        s.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 8);
        s.rectTransform.position = new Vector2(Screen.width / 2, Screen.height / 16);
        Words = s.GetComponentsInChildren<Text>();
        float height = s.rectTransform.sizeDelta.y;
        float width = s.rectTransform.sizeDelta.x;
        Words[0].rectTransform.position = new Vector2((int)(Words[0].rectTransform.position.x), (int)(s.rectTransform.position.y + 10));
        Words[1].rectTransform.position = new Vector2((int)(Words[1].rectTransform.position.x), (int)(s.rectTransform.position.y + 10));
        while ((Words[0].preferredHeight) * 1.4 < height / 2 && (Words[0].preferredWidth) * 1.4 < width / 2)
        {
            Words[0].fontSize++;
        }
        while (Words[0].preferredHeight > height / 2 || Words[0].preferredWidth > width / 2)
        {
            Words[0].fontSize--;

        }
        while ((Words[1].preferredHeight) * 1.4 < height / 2 && (Words[1].preferredWidth) * 1.4 < width / 2)
        {
            Words[1].fontSize++;
        }
        while (Words[1].preferredHeight > height / 2 || Words[1].preferredWidth > width / 2)
        {
            Words[1].fontSize--;
        }

        uiText = getText();
        if (getMouseCell() != null && Input.GetMouseButton(0))
        {
            Words[0].text = ("<b>Name:</b> " + getMouseCell().name + " \t <b>Building:</b> " + getMouseCell().building + "\t <b>Terrain:</b> " + getTerrain(getMouseCell().Color));
            Words[1].text = ("You are at cell: " + getMouseCell().Position);
        }
    }

    public string getTerrain(Color col)
    {
        List<Color> terrainType = new List<Color>(9);
        List<string> terrainName = new List<string>(9);
        terrainName.Add("grass");
        terrainName.Add("breakaway");
        terrainName.Add("ocean");
        terrainName.Add("city");
        terrainName.Add("power");
        terrainName.Add("nuclear");
        terrainName.Add("fuel");
        terrainName.Add("airport");
        terrainName.Add("pipeline");
        terrainType.Add(new Color(.61F, .7F, .5F, 1));
        terrainType.Add(new Color(1, .6F, .6F, 1));
        terrainType.Add(new Color(.2F, .2F, 1F, 1));
        terrainType.Add(new Color(.5F, .5F, .5F, 1));
        terrainType.Add(new Color(1F, .54F, 0, 1));
        terrainType.Add(new Color(1F, 1F, 0, 1));
        terrainType.Add(new Color(1F, .75F, .80F, 1));
        terrainType.Add(new Color(.2F, 1F, 1F, 1));
        terrainType.Add(new Color(.3F, .3F, 1F, 1));
        //breakaway = new Color(1, .6F, .6F, 1);
        //ocean = new Color(.2F, .2F, 1F, 1);
        //city = new Color(.5F, .5F, .5F, 1);
        //power = new Color(1F, .54F, 0, 1);
        //nuclear = new Color(1F, 1F, 0, 1);
        //fuel = new Color(1F, .75F, .80F, 1);
        //airport = new Color(.2F, 1F, 1F, 1);
        //pipeline = new Color(.3F, .3F, 1F, 1);
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
}
