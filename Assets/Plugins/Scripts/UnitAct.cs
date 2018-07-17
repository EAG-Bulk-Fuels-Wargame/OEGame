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

    public static Unit GetUnit(HexCell h)
    {
        foreach (Unit u in unitList)
        {
            if (u.GetUnitCell() == h)
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
        float newEffectiveness;
        int newStack;
        bool newTerry;
        newUnitModelPrefab = u1.GetUnitModelPrefab();
        newUnitName = u1.GetUnitName();
        // newHealth = u1.GetHealth() + u2.GetHealth();
        //  newStrength = u1.GetStrength() + u2.GetStrength();
        newEffectiveness = u1.GetEffective() + u2.GetEffective();
        newStack = u1.GetStacked() + u2.GetStacked();
        newTerry = u1.GetTerrorist();
        Unit finalUnit = new Unit(newUnitModelPrefab, u1.GetUnitCell(), newUnitName, newEffectiveness, newStack, u1.GetTeam(), newTerry);
        RemoveUnit(u1);
        RemoveUnit(u2);
        AddUnit(finalUnit);
        return finalUnit;
    }

    public static void MoveUnit(Unit u)
    {
        

    }

    public static void MoveTexturize(Unit u)
    {
        HexGrid hexgr = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        HexGridChunk hexch = GameObject.FindWithTag("Chunk").GetComponent<HexGridChunk>();
        HexCell h = u.GetUnitCell();
        List<HexCell> l = u.GetWalkableTiles(hexgr.cells);
        foreach (HexCell hc in l)
        {
            Color c = hc.Color;
            c.r *= c.g *= c.b *= 1.2F;
            hc.Color = c;
        }
        hexch.Refresh();
    }

    public static void UnTexturize(Unit u)
    {
        HexGrid hexgr = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        HexCell h = u.GetUnitCell();
        List<HexCell> l = u.GetWalkableTiles(hexgr.cells);
        foreach (HexCell hc in l)
        {
            Color c = hc.Color;
            c.r /= 1.2F;
            c.g /= 1.2F;
            c.b /= 1.2F;
            hc.Color = c;
        }
    }
}
