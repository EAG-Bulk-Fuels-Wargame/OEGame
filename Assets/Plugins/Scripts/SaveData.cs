using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveData
{
    public static HexCellContainer hexCellContainer = new HexCellContainer();

    public delegate void serializeAction();

    public static serializeAction OnBeforeLoaded;
    public static serializeAction OnLoaded;
    public static serializeAction OnBeforeSave;
    public static List<int> cellNumList;

    public static void Load(string path)
    {
        OnBeforeLoaded();

        HexGrid hexgr = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        HexGridChunk hexch = GameObject.FindWithTag("Chunk").GetComponent<HexGridChunk>();
        hexCellContainer = LoadCells(path);

        int count = 0;
        foreach (HexCellData data in hexCellContainer.cells)
            count++;
        Debug.Log(count);

        hexch.terrain.Clear();
        foreach (HexCell cell in hexgr.cells)
            cell.enabled = false;
        hexgr.DestroyCells();
        hexgr.cells.Clear();
        hexch.cells.Clear();
        Debug.Log(hexch.cells.Count);
        Debug.Log("cells gonna be created");
        hexgr.CreateCells(hexCellContainer);
        Debug.Log("ch:" + hexch.cells.Count);
        Debug.Log("gr:" + hexgr.cells.Count);

        OnLoaded();
    }

    public static void Save(string path, HexCellContainer cellContainer)
    {
        Debug.LogAssertion("!!!!NEW SAVE!!!!");

        cellNumList = new List<int>();
        cellNumList.Clear();
        Debug.Log("here's the length of the cell num list: " + cellNumList.Count);
        cellContainer.cells.Clear();
        Debug.Log(cellContainer.cells.Count);
        Debug.Log("there's " + GameObject.FindGameObjectsWithTag("Cell").Length + " cells, yo");
        OnBeforeSave();
        Debug.Log("here's the length of the cell num list: " + cellNumList.Count);
        Debug.Log("when you add " + cellContainer.cells.Count + " cells anyways");
        SaveCells(path, cellContainer);
        ClearCells();
    }

    public static void AddCellData(HexCellData data)
    {
        //Debug.Log(hexCellContainer.cells.Count);
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

    private static void SaveCells(string path, HexCellContainer cellContainer)
    {
        if (File.Exists(path))
            File.Delete(path);
        XmlSerializer serializer = new XmlSerializer(typeof(HexCellContainer));
        FileStream stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, cellContainer);
        stream.Close();
    }
} //hell yeah