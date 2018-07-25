using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInteractionScript : MonoBehaviour
{

    InfoBarScript ibs;
    Player user;

    // Use this for initialization
    void Start()
    {
        ibs = GameObject.Find("InfoUI").GetComponent<InfoBarScript>();
        user = new Player("blue", "NATO", true, true);

    }

    // Update is called once per frame
    void Update()
    {
        if (user.CanAct())
        {
            UnitSelectSearch();
        }
    }

    public void UnitSelectSearch()
    {
        if (Input.GetMouseButton(0) && GetCellClicked() != null && !GameObject.Find("ActionPopUp(Clone)") && UnitAct.HasTeamUnit(user.GetTeam(),GetCellClicked()))
        {
            ibs.UnitAction(GetCellClicked(), UnitAct.GetTeamUnit(user.GetTeam(), GetCellClicked()));
            //user.TakeAction();

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

    public Player GetPlayer()
    {

        return user;

    }
}
