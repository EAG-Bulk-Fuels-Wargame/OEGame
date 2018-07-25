using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    string team;
    string enemy;
    string userName;
    bool isTurn;
    bool playable;
    int actions;
    bool inAction;

    // Use this for initialization
    public Player()
    {
        team = "blue";
        enemy = "red";
        userName = "observer";
        isTurn = false;
        playable = false;
        actions = 7;
        inAction = false;
    }

    public Player(string c, string un, bool it, bool p)
    {
        team = c;
        if (team == "red")
        {
            enemy = "blue";
        }
        else
        {
            enemy = "red";
        }
        userName = un;
        isTurn = it;
        playable = p;
        actions = 7;
        inAction = false;

    }

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (!isTurn)
        {
            RefreshActions();
        }
        //List<Unit> unitList = UnitAct.GetUnitList();
        //foreach (Unit u in unitList) {
        //    foreach (Unit v in unitList)
        //    {

        //        if (!u.IsHidden() && !v.IsHidden() && u.GetUnitCell() == v.GetUnitCell()) {
        //            u.SetVisible(false);
        //            v.SetVisible(false);
        //        }
        //    }
        //}
    }

    public void RefreshUnitTile(HexCell occ)
    {
        if (UnitAct.HasTeamUnit("blue", occ) && UnitAct.HasTeamUnit("red", occ))
        {

        }
    }

    public bool GetIsTurn()
    {
        return isTurn;
    }

    public void SetIsTurn(bool it)
    {
        isTurn = it;
    }

    public void SetInAction(bool ia)
    {
        inAction = ia;
    }

    public void TakeAction()
    {
        actions--;
    }

    public void RefreshActions()
    {
        actions = 7;
    }

    public bool CanAct()
    {
        if (actions > 0 && isTurn && !inAction)
        {
            return true;
        }
        return false;
    }

    public string GetTeam()
    {
        return team;
    }

    public string GetEnemy()
    {
        return enemy;
    }

}
