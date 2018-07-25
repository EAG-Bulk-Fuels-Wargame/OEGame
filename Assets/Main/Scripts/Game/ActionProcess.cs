using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionProcess : MonoBehaviour
{

    static Unit selectedUnit;
    static HexCell selectedHex;
    static FuelRegion selectedFuelRegion;
    static InfoBarScript ibs;
    static UserInteractionScript uis;
    static string action;
    static List<HexCell> actionCells = new List<HexCell>();
    static List<Unit> actionUnits;
    static Player user;

    static Unit lastUnit;
    static Vector2 lastVector;
    static List<string> lastStrings;
    static string lastString;
    static string errorMessage;



    void Update()
    {
        if (action == "Completed")
        {
            ibs = GameObject.Find("InfoUI").GetComponent<InfoBarScript>();
            uis = GameObject.Find("InfoUI").GetComponent<UserInteractionScript>();
            uis.GetPlayer().SetInAction(false);
            ibs.SetStopSearch(false);
            selectedUnit = null;
            action = "";

        }
        if (action == "Ground Raid")
        {
            if (Input.GetMouseButton(0))
            {
                HexCell clicked = GetCellClicked();
                foreach (HexCell h in actionCells)
                {
                    if (selectedUnit.GetUnitCell() == clicked)
                    {
                        UnitAct.UnTexturize(selectedUnit);
                        action = "Completed";
                        break;
                    }
                    else if (h == clicked)
                    {
                        Unit def = UnitAct.GetTeamUnit(selectedUnit.GetEnemy(), h);
                        float damageDealt = 25 + rollmod(selectedUnit);
                        float damageTaken = def.counteratt();
                        Debug.Log("COMBAT: Attacker dealt " + damageDealt + " damage, defender dealt " + damageTaken + " damage");
                        def.effectiveness = def.effectiveness - damageDealt;
                        selectedUnit.effectiveness = selectedUnit.effectiveness - damageTaken;
                        Debug.Log("COMBAT: Attacker health is  " + selectedUnit.effectiveness + ", defender health is " + def.effectiveness);
                        UnitAct.UnTexturize(selectedUnit);
                        user.TakeAction();
                        action = "Completed";
                        break;
                    }
                }
            }
        }
        if (action == "SOF Raid")
        {
            if (Input.GetMouseButton(0))
            {
                HexCell clicked = GetCellClicked();
                foreach (HexCell h in actionCells)
                {
                    if (selectedUnit.GetUnitCell() == clicked)
                    {
                        UnitAct.UnTexturize(selectedUnit);
                        action = "Completed";
                        break;
                    }
                    else if (h == clicked)
                    {
                        Unit def = UnitAct.GetTeamUnit(selectedUnit.GetEnemy(), h);
                        float damageDealt = 25 + rollmod(selectedUnit);
                        float damageTaken = def.counteratt();
                        Debug.Log("COMBAT: Attacker dealt " + damageDealt + " damage, defender dealt " + damageTaken + " damage");
                        def.effectiveness = def.effectiveness - damageDealt;
                        selectedUnit.effectiveness = selectedUnit.effectiveness - damageTaken;
                        UnitAct.UnTexturize(selectedUnit);
                        user.TakeAction();
                        action = "Completed";
                        break;
                    }
                }
            }
        }
        if (action == "Ground Unit Relocation")
        {
            if (Input.GetMouseButton(0))
            {
                HexCell clicked = GetCellClicked();
                foreach (HexCell h in actionCells)
                {


                    if (selectedUnit.GetUnitCell() == clicked)
                    {
                        UnitAct.UnTexturize(selectedUnit);
                        action = "Completed";
                        break;
                    }
                    else if (h == clicked)
                    {
                        if (selectedUnit.GetMovement() == 3)
                            user.TakeAction();
                        UnitAct.MoveUnit(selectedUnit, h);
                        UnitAct.UnTexturize(selectedUnit);
                        action = "Completed";
                        break;

                    }
                }
            }
        }
        if (action == "Establish Farp")
        {
            user.TakeAction();
            action = "Completed";
        }
        if (action == "Incomplete")
        {
            ibs = GameObject.Find("InfoUI").GetComponent<InfoBarScript>();
            uis = GameObject.Find("InfoUI").GetComponent<UserInteractionScript>();
            uis.GetPlayer().SetInAction(false);
            ibs.SetStopSearch(false);
            selectedUnit = null;
            Canvas popUpSystem = GameObject.FindWithTag("PopUpUI").GetComponent<Canvas>();
            HexPopUp other = (HexPopUp)popUpSystem.GetComponent(typeof(HexPopUp));
            MakeAction(lastUnit, lastVector, lastStrings, lastString);
            other.WriteErrorMessage("Invalid Option: " + errorMessage);
            action = "";
        }

    }

    public static void runAction(string scenarioName, string choice)
    {
        HexGridChunk hexch = GameObject.FindWithTag("Chunk").GetComponent<HexGridChunk>();
        user = uis.GetPlayer();
        ibs = GameObject.Find("InfoUI").GetComponent<InfoBarScript>();
        selectedHex = selectedUnit.GetUnitCell();
        selectedFuelRegion = selectedUnit.GetFuelRegion();
        //create if statement asking for actionName
        //use choice to decide result accordingly
        //if (scenarioName.Equals("Air_Action")) {
        //  exampleMethod(choice)
        //}
        if (scenarioName.Equals("Default_Action"))
        {
            Debug.Log("User Choice: " + choice);
        }
        if (scenarioName.Equals("Infantry_Action"))
        {
            switch (choice)
            {
                case "Ground Raid":
                    Debug.Log("You have chosen: Ground Raid");
                    ibs.SetStopSearch(true);
                    actionUnits = UnitAct.GetEnemyUnitsInRange(selectedUnit.GetUnitCell(), 1, selectedUnit.GetTeam());
                    //actionUnits = UnitAct.GetTeamCell(user.GetEnemy(), actionUnits);
                    if (actionUnits.Count > 0)
                    {

                        UnitAct.ColorCombatants(selectedUnit.GetUnitCell(), actionUnits);
                        //Color col = selectedUnit.GetUnitCell().Color;
                        Debug.Log("Hex Coords: " + selectedHex.coordinates);
                        actionCells.Clear();
                        foreach (Unit u in actionUnits)
                        {
                            actionCells.Add(u.GetUnitCell());
                        }
                        selectedFuelRegion.Subtract(20000);
                        action = choice;
                        
                    }
                    else
                    {
                        errorMessage = "No Adjacient Enemy Units";
                        action = "Incomplete";
                    }
                    break;
                case "Ground Unit Relocation":
                    if (selectedUnit.GetMovement() != 0)
                    {
                        Debug.Log("Ground Unit Relocation");
                        action = choice;
                        ibs.SetStopSearch(true);
                        actionCells = UnitAct.MoveTexturize(selectedUnit);
                        selectedFuelRegion.Subtract(10000);
                    }
                    else
                    {
                        errorMessage = "Unit out of Movement";
                        action = "Incomplete";
                    }
                    break;
                case "Fortify":
                    Debug.Log("You have chosen fortify");
                    action = choice;
                    break;
                case "Construct Camp":
                    Debug.Log("You have chosen: Construct Camp");
                    if (selectedHex.type == "Campsite" && selectedHex.hasCamp == false && !UnitAct.HasTeamUnit(user.GetEnemy(), selectedHex))
                    {
                        selectedFuelRegion.Subtract(100000);
                        action = choice;
                    }
                    else
                    {
                        errorMessage = "Not  appropriate location to build a camp";
                        action = "Incomplete";
                    }
                    break;
                case "Establish Farp":
                    Debug.Log("You have chosen: Establish Farp");
                    if (selectedHex.type == "Campsite" && selectedHex.hasCamp == true && !UnitAct.HasTeamUnit(user.GetEnemy(), selectedHex))
                    {
                        selectedFuelRegion.Subtract(100000);
                        action = choice;
                    }
                    else
                    {
                        errorMessage = "Not  appropriate location to build a camp";
                        action = "Incomplete";
                    }
                    break;
                case "SOF Raid":
                    Debug.Log("You have chosen: SOF Raid");
                    ibs.SetStopSearch(true);
                    actionUnits = UnitAct.GetEnemyUnitsInRange(selectedUnit.GetUnitCell(), 1, selectedUnit.GetTeam());
                    //actionUnits = UnitAct.GetTeamCell(user.GetEnemy(), actionUnits);
                    if (actionUnits.Count > 0)
                    {

                        UnitAct.ColorCombatants(selectedUnit.GetUnitCell(), actionUnits);
                        Debug.Log("Hex Coords: " + selectedHex.coordinates);
                        actionCells.Clear();
                        foreach (Unit u in actionUnits)
                        {
                            actionCells.Add(u.GetUnitCell());
                        }
                        selectedFuelRegion.Subtract(25000);
                        action = choice;
                    }
                    else
                    {
                        errorMessage = "No Adjacient Enemy Units";
                        action = "Incomplete";
                    }
                    break;
                case "Reduce Security Operations at Location":
                    Debug.Log("You have chosen: Reduce Security Operations at Location");
                    selectedFuelRegion.Add(10000);
                    action = choice;
                    break;
                case "Repair Critical Energy Infastructure":
                    Debug.Log("You have chosen: Repair Critical Energy Infastructure");
                    selectedFuelRegion.Subtract(50000);
                    action = choice;
                    break;
                case "Public Works Project":
                    Debug.Log("You have chosen: Public Works Project");
                    selectedFuelRegion.Subtract(10000);
                    action = choice;
                    break;
                case "Allocate Fuel to Unit":
                    Debug.Log("You have chosen: Allocate Fuel to Unit");
                    selectedFuelRegion.Subtract(10000);
                    action = choice;
                    break;
                case "Cancel":
                    action = "Completed";
                    break;
                default:
                    break;

            }
        }
    }

    //v = location where popup should appear (static), s = options for dropdown list
    //actionName = what string you're using to determine what action should be run
    public static void MakeAction(Unit u, Vector2 v, List<string> s, string scenarioName)
    {
        lastUnit = u;
        lastVector = v;
        lastStrings = s;
        lastString = scenarioName;
        uis = GameObject.Find("InfoUI").GetComponent<UserInteractionScript>();
        selectedUnit = u;
        Canvas popUpSystem = GameObject.FindWithTag("PopUpUI").GetComponent<Canvas>();
        HexPopUp other = (HexPopUp)popUpSystem.GetComponent(typeof(HexPopUp));
        other.CreatePopup(v, s, scenarioName);
        uis.GetPlayer().SetInAction(true);
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

    public float rollmod(Unit u)
    {
        System.Random rnd = new System.Random();
        int x = rnd.Next(1, 20);
        Debug.Log("ROLLED "+x);
        if (x >= 1 && x <= 5)
        {

            return (float)(u.effectiveness * .1);

        }
        else if (x >= 6 && x <= 10)
        {
            return (float)(u.effectiveness *.2);
        }
        else if (x >= 11 && x <= 15)
        {
            return (float)(u.effectiveness * 1.2);
        }
        else if (x >= 12 && x <= 19)
        {
            return (float)(u.effectiveness * 1.2);
        }
        else
        {
            return (float)(u.effectiveness * 1.5);
        }
    }
}
