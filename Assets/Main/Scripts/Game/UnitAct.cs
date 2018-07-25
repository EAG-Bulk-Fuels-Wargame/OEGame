using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitAct
{

    static List<Unit> unitList = new List<Unit>();

    public static void AddUnit(Unit u)
    {

        unitList.Add(u);

    }

    public static void PopulateUnits(List<HexCell> hexcells, string team)
    {
        foreach (HexCell h in hexcells)
        {
            Unit u = new Unit(h, team);
            AddUnit(u);
        }
    }

    public static List<Unit> GetUnitList()
    {
        return unitList;
    }

    public static Unit GetTeamUnit(string team, HexCell h)
    {
        foreach (Unit u in unitList)
        {
            if (u.GetUnitCell() == h && u.GetTeam() == team)
            {
                return u;
            }
        }
        return null;
    }

    public static bool HasUnit(HexCell h)
    {
        foreach (Unit u in unitList)
        {
            if (u.GetUnitCell() == h)
            {
                return true;
            }
        }
        return false;
    }

    public static bool HasTeamUnit(string team, HexCell h)
    {
        foreach (Unit u in unitList)
        {
            if (u.GetUnitCell() == h && u.GetTeam() == team)
            {
                return true;
            }
        }
        return false;
    }

    public static bool RemoveUnit(Unit rm)
    {
        for (int i = 0; i < unitList.Count; i++)
        {
            if (unitList[i] == rm)
            {
                unitList.Remove(rm);
                return true;
            }
        }
        return false;
    }

    public static Unit combineUnit(Unit u1, Unit u2)
    {
        string newUnitModelPrefab;
        string newUnitName;
        int newHealth;
        int newStrength;
        int newStack;
        float newEffectiveness;
        newUnitModelPrefab = u1.GetUnitModelPrefab();
        newUnitName = u1.GetUnitName();
        newEffectiveness = u1.effectiveness + u2.effectiveness;
        newHealth = u1.GetHealth() + u2.GetHealth();
        newStrength = u1.GetStrength() + u2.GetStrength();
        newStack = u1.GetStacked() + u2.GetStacked();
        Unit finalUnit = new Unit(newUnitModelPrefab, u1.GetUnitCell(), newUnitName, newEffectiveness, newHealth, newStrength, newStack, u1.GetTeam(), true);
        RemoveUnit(u1);
        RemoveUnit(u2);
        AddUnit(finalUnit);
        return finalUnit;
    }

    public static void MoveUnit(Unit u, HexCell h)
    {
        int dist = h.GetHexDistance(u.GetUnitCell(), h);
        int movementLeft = u.GetMovement() - dist;
        u.SetMovement(movementLeft);
        u.ChangeTile(h);
    }

    public static List<HexCell> MoveTexturize(Unit u)
    {
        HexGrid hexgr = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        HexGridChunk hexch = GameObject.FindWithTag("Chunk").GetComponent<HexGridChunk>();
        HexCell h = u.GetUnitCell();
        List<HexCell> radius = h.GetRadiusOfCells(h, u.GetMovement());//do not exceed 8 or half the size of the map
        List<HexCell> l = u.GetWalkableTiles(radius);
        Color col = l[0].Color;
        col.r *= 1.5F;
        col.g *= 1.3F;
        col.b *= .1F;
        l[0].Color = col;
        //foreach (HexCell hc in l) {
        for (int i = 1; i < l.Count; i++)
        {
            HexCell hc = l[i];
            Color c = hc.Color;
            c.r *= 1.2F;
            c.g *= 1.2F;
            c.b *= 2F;
            hc.Color = c;
        }
        hexch.Refresh();
        return l;

    }

    public static void UnTexturize(Unit u)
    {
        HexGrid hexgr = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        HexCell h = u.GetUnitCell();
        List<HexCell> l = u.GetWalkableTiles(hexgr.cells);
        foreach (HexCell hc in l)
        {
            hc.ResetColor();
        }
    }

    public static List<Unit> GetUnitsInRange(HexCell hex, int range)
    {
        List<Unit> units = new List<Unit>();
        List<HexCell> hexes = hex.GetRadiusOfCells(hex, range);
        foreach (HexCell h in hexes)
        {
            if (h.hasUnit)
            {
                if (GetTeamUnit("blue", h) != null)
                    units.Add(GetTeamUnit("blue", h));
                if (GetTeamUnit("red", h) != null)
                    units.Add(GetTeamUnit("red", h));
            }

        }
        return units;
    }

    public static List<Unit> GetEnemyUnitsInRange(HexCell hex, int range, string teamColor)
    {
        string enemyColor;
        if (teamColor == "blue")
        {
            enemyColor = "red";
        }
        else
        {
            enemyColor = "blue";
        }
        List<Unit> units = new List<Unit>();
        List<HexCell> hexes = hex.GetRadiusOfCells(hex, range);
        foreach (HexCell h in hexes)
        {
            if (HasTeamUnit(enemyColor, h))
                units.Add(GetTeamUnit(enemyColor, h));
        }
        return units;
    }


    public static void ColorCombatants(HexCell selHex, List<Unit> combatants)
    {
        HexGridChunk hexch = GameObject.FindWithTag("Chunk").GetComponent<HexGridChunk>();
        Color col = selHex.Color;
        col.r *= 1.5F;
        col.g *= 1.3F;
        col.b *= .1F;
        selHex.Color = col;
        for (int i = 0; i < combatants.Count; i++)
        {
            HexCell hc = combatants[i].GetUnitCell();
            Color c = hc.Color;
            c.r *= 1.8F;
            c.g *= .1F;
            c.b *= .2F;
            hc.Color = c;
        }
        //foreach (Unit u in combatants) {
        //    HexCell hc = u.GetUnitCell();
        //    Color c = hc.Color;
        //    c.r *= 1.8F;
        //    c.g *= .1F;
        //    c.b *= .2F;
        //    hc.Color = c;
        //}
        hexch.Refresh();
    }

    public static List<Unit> GetTeamCell(string team)
    {
        List<Unit> units = new List<Unit>();
        foreach (Unit u in unitList)
        {
            if (u.GetTeam() == team)
                units.Add(u);
        }
        return units;
    }

    public static List<Unit> GetTeamCell(string team, List<Unit> cells)
    {
        List<Unit> units = new List<Unit>();
        foreach (Unit u in cells)
        {
            if (u.GetTeam() == team)
                units.Add(u);
        }
        return units;
    }

    public static void RefreshTeam(string team)
    {
        List<Unit> units = GetTeamCell(team);
        foreach (Unit u in units)
        {
            u.RefreshUnit();
        }

    }

}
