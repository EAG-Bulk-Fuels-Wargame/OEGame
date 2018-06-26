using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveData {

    public static HexCellContainer hexCellContainer = new HexCellContainer();
    public delegate void serializeAction();
    public static serializeAction OnBeforeLoaded;
    public static serializeAction OnLoaded;
    public static serializeAction OnBeforeSave;
    

    public static void Load(string path)
    {
        HexGrid hexgr = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        HexGridChunk hexch = GameObject.FindWithTag("Chunk").GetComponent<HexGridChunk>();
        hexCellContainer = LoadCells(path);

        int count = 0;
        foreach (HexCellData data in hexCellContainer.cells)
            count++;
        Debug.Log(count);

        hexch.terrain.Clear();
        //foreach (HexCell cell in hexgr.cells)
        hexgr.DestroyCells();
        hexgr.cells.Clear();
        hexch.cells.Clear();
        Debug.Log(hexch.cells.Count);
        Debug.Log("cells gonna be created");
        hexgr.CreateCells(hexCellContainer);
        Debug.Log("ch:"+hexch.cells.Count);
        Debug.Log("gr:"+hexgr.cells.Count);

        OnLoaded();
    }

    public static void Save(string path, HexCellContainer cells)
    {
        OnBeforeSave();
        SaveCells(path, cells);
        ClearCells();
    }

    public static void AddCellData(HexCellData data)
    {
        hexCellContainer.cells.Add(data);

    }

    public static void ClearCells()
    {
        hexCellContainer.cells.Clear();
    }
    
    private static HexCellContainer LoadCells(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(HexCellContainer));
        FileStream stream = new FileStream(path, FileMode.Open);
        HexCellContainer cells = serializer.Deserialize(stream) as HexCellContainer;
        stream.Close();
        return cells;
    }

    private static void SaveCells(string path, HexCellContainer cells)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(HexCellContainer));
        FileStream stream = new FileStream(path, FileMode.Truncate);
        serializer.Serialize(stream, cells);
        stream.Close();
    }

}
