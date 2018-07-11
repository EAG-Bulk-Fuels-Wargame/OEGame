using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInteractionScript : MonoBehaviour {

    InfoBarScript ibs;
    Player user;

    // Use this for initialization
    void Awake() {
        ibs = GameObject.Find("InfoUI").GetComponent<InfoBarScript>();
        HexGrid hexgr = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        List<HexCell> hexes = new List<HexCell>();
        for (int i = 0; i < 30; i++)
            hexes.Add(hexgr.cells[i * 7]);
        //UnitAct.PopulateUnits(hexes, "blue");

        user = new Player("blue", "UN Force", true, true);

    }

    // Update is called once per frame
    void Update() {
        Debug.Log("User Can Act = " + user.CanAct());
        if (user.CanAct()) {
            UnitSelectSearch();
        }
    }

    public void UnitSelectSearch() {
        if (Input.GetMouseButton(0) && GetCellClicked() != null && !GameObject.Find("ActionPopUp(Clone)") && UnitAct.HasUnit(GetCellClicked())) {
            //ibs.UnitAction(ibs.GetMouseCell(), UnitAct.GetUnit(ibs.GetMouseCell()));
            ibs.UnitAction(GetCellClicked(), UnitAct.GetUnit(GetCellClicked()));
            user.TakeAction();

        }
    }

    public HexCell GetCellClicked()
    {
        //returns the HexCell clicked by the mouse
        HexGrid hexgr = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        HexGridChunk hexch = GameObject.FindWithTag("Chunk").GetComponent<HexGridChunk>();

        HexCell h = null;
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit) && hexgr != null && Input.GetMouseButton(0))
        {
            h = hexgr.GetCell(hit.point);

        }
        return h;
    }

    public Player GetPlayer() {

        return user;

    }



}
