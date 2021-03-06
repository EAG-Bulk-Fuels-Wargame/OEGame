﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

    int cellCountX, cellCountZ;
    public int chunkCountX = 1, chunkCountZ = 1;
    public int unitCount;
    public HexCell cellPrefab;
    public Unit unitPrefab;
    public Text cellLabelPrefab;
    public Color defaultColor;
    public Color breakaway;
    public Color ocean;
    public Color city;
    public Color power;
    public Color nuclear;
    public Color fuel;
    public Color airport;
    public Color pipeline;
    public HexCell[] cells;
    HexGridChunk[] chunks;
    List<Unit> units;
    GameObject[] cities;
    public Collider2D cellCollider;
    public Collider2D unitCollider;
    public GameObject Airport;
    public GameObject City;
    public GameObject Coal;
    public GameObject Nuclear;
    public GameObject Oil;
    public Texture2D noiseSource;
    public HexGridChunk chunkPrefab;

    void Awake () {
		HexMetrics.noiseSource = noiseSource;

		cellCountX = chunkCountX * HexMetrics.chunkSizeX;
		cellCountZ = chunkCountZ * HexMetrics.chunkSizeZ;

		CreateChunks();
		CreateCells();

        unitCount = 17;
        units = new List<Unit>();

        defaultColor = new Color(.61F, .7F, .5F, 1);
        breakaway = new Color(1, .6F, .6F, 1);
        ocean = new Color(.84F, .84F, .8F, 1);
        city = new Color(.5F, .5F, .5F, 1);
        power = new Color(1F, .54F, 0, 1);
        nuclear = new Color(1F, 1F, 0, 1);
        fuel = new Color(1F, .75F, .80F, 1);
        airport = new Color(.2F, 1F, 1F, 1);
        pipeline = new Color(.3F, .3F, 1F, 1);

        populateCellData(cells, ocean, city, power, nuclear, airport, pipeline);

        render3d();
    }

    void CreateChunks () {
		chunks = new HexGridChunk[chunkCountX * chunkCountZ];

		for (int z = 0, i = 0; z < chunkCountZ; z++) {
			for (int x = 0; x < chunkCountX; x++) {
				HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
				chunk.transform.SetParent(transform);
			}
		}
	}

	void CreateCells () {
		cells = new HexCell[cellCountZ * cellCountX];

		for (int z = 0, i = 0; z < cellCountZ; z++) {
			for (int x = 0; x < cellCountX; x++) {
				CreateCell(x, z, i++);
			}
		}
	}

	void OnEnable () {
		HexMetrics.noiseSource = noiseSource;
	}

	public HexCell GetCell (Vector3 position) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index =
			coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
		return cells[index];
	}

	public HexCell GetCell (HexCoordinates coordinates) {
		int z = coordinates.Z;
		if (z < 0 || z >= cellCountZ) {
			return null;
		}
		int x = coordinates.X + z / 2;
		if (x < 0 || x >= cellCountX) {
			return null;
		}
		return cells[x + z * cellCountX];
	}

    public Unit getUnit(Vector3 position)
    {
        //position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
        Debug.Log(index);
        return units[index];
    }

    public void ShowUI (bool visible) {
		for (int i = 0; i < chunks.Length; i++) {
			chunks[i].ShowUI(visible);
		}
	}

	void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.Color = defaultColor;

		if (x > 0) {
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}
		if (z > 0) {
			if ((z & 1) == 0) {
				cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
				if (x > 0) {
					cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
				}
			}
			else {
				cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
				if (x < cellCountX - 1) {
					cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
				}
			}
		}

		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
		cell.uiRect = label.rectTransform;

		cell.Elevation = 0;

		AddCellToChunk(x, z, cell);
	}

	void AddCellToChunk (int x, int z, HexCell cell) {
		int chunkX = x / HexMetrics.chunkSizeX;
		int chunkZ = z / HexMetrics.chunkSizeZ;
		HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

		int localX = x - chunkX * HexMetrics.chunkSizeX;
		int localZ = z - chunkZ * HexMetrics.chunkSizeZ;
		chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);
	}

    void populateCellData(HexCell[] cells, Color ocean, Color city, Color power, Color nuclear, Color airport, Color pipeline)
    {
        foreach (HexCell item in cells)
            item.Color = defaultColor;
        for (int i = 0; i <= (cellCountX * cellCountZ) - 1; i = i + cellCountX) //colors base layer
        {
            //Debug.Log("Left Cell: " + (i));
            //Debug.Log("Right Cell: " + (i + width));

            cells[i].Color = ocean;
            cells[i].type = "Ocean";
            cells[i].Elevation = -1;
            cells[i + 1].Color = ocean;
            cells[i + 1].type = "Ocean";
            cells[i + 1].Elevation = -1;
            cells[i + (cellCountX - 3)].Color = breakaway;
            cells[i + (cellCountX - 3)].type = "Breakaway";
            cells[i + (cellCountX - 2)].Color = breakaway;
            cells[i + (cellCountX - 2)].type = "Breakaway";
            cells[i + (cellCountX - 1)].Color = breakaway;
            cells[i + (cellCountX - 1)].type = "Breakaway";
        }

        cells[41].Color = city;
        cells[41].name = "South City";
        cells[41].building = "city";
        cells[42].Color = city;
        cells[42].name = "South City";
        cells[42].building = "city";

        cells[62].Color = city;
        cells[62].name = "Hagers Town";
        cells[62].building = "city";

        cells[105].Color = city;
        cells[105].name = "Midtown";
        cells[105].building = "city";
        cells[120].Color = city;
        cells[120].name = "Midtown";
        cells[120].building = "city";

        cells[114].Color = city;
        cells[114].name = "Port Town";
        cells[114].building = "city";

        cells[130].Color = city;
        cells[130].name = "Port Town";
        cells[130].building = "city";

        cells[157].Color = city;
        cells[157].name = "Fate City";
        cells[157].building = "city";
        cells[158].Color = city;
        cells[158].name = "Fate City";
        cells[158].building = "city";

        cells[184].Color = city;
        cells[184].name = "Capital City";
        cells[184].building = "city";
        cells[200].Color = city;
        cells[200].name = "Capital City";
        cells[200].building = "city";
        cells[201].Color = city;
        cells[201].name = "Capital City";
        cells[201].building = "city";

        cells[221].Color = city;
        cells[221].name = "Charlie City";
        cells[221].building = "city";
        cells[238].Color = city;
        cells[238].name = "Charlie City";
        cells[238].building = "city";

        cells[260].Color = city;
        cells[260].name = "North City";
        cells[260].building = "city";
        cells[261].Color = city;
        cells[261].name = "North City";
        cells[261].building = "city";

        cells[266].Color = city;
        cells[266].name = "Beta Town";
        cells[266].building = "city";

        cells[75].Color = power;
        cells[75].name = "South Power Plant";
        cells[75].building = "power";

        cells[85].Color = power;
        cells[85].name = "Port Power Plant";
        cells[85].building = "power";

        cells[142].Color = power;
        cells[142].name = "Breakaway Power Plant";
        cells[142].building = "power";

        cells[152].Color = nuclear;
        cells[152].name = "Capital Nuclear Plant";
        cells[152].building = "nuclear";

        cells[245].Color = power;
        cells[245].name = "North Renewable Plant";
        cells[245].building = "power";

        cells[244].Color = fuel;
        cells[244].name = "North Fuel Depot";
        cells[244].building = "fuel";

        cells[183].Color = fuel;
        cells[183].name = "Capital Fuel Depot";
        cells[183].building = "fuel";

        cells[121].Color = fuel;
        cells[121].name = "Midtown Fuel Depot";
        cells[121].building = "fuel";

        cells[38].Color = fuel;
        cells[38].name = "South Fuel Depot";
        cells[38].building = "fuel";

        cells[54].Color = airport;
        cells[54].name = "South Airport";
        cells[54].building = "airport";

        cells[198].Color = airport;
        cells[198].name = "Capital Airport";
        cells[198].building = "airport";

        cells[247].Color = pipeline;
        cells[247].name = "Pipeline Station";
        cells[247].building = "pipeline";
    }

    void render3d()
    {
        for (int i = 0; i < cellCountX * cellCountZ - 1; i++)
        {
            if (cells[i].building == "city")
            {
                Vector3 pos;
                pos.x = cells[i].transform.position.x;
                pos.y = cells[i].transform.position.y;
                pos.z = cells[i].transform.position.z - 10;


                Debug.Log(pos.x + "," + pos.y + "," + pos.z);

                Instantiate(City);
                units.Add(Instantiate<Unit>(unitPrefab));
                City.transform.localPosition = pos;
                pos.z = cells[i].transform.position.z;
                units[units.Count - 1].transform.localPosition = pos;
                GetCell(pos).hasUnit = true;
            }
            if (cells[i].building == "airport")
            {
                Vector3 pos;
                pos.x = cells[i].transform.position.x;
                pos.y = cells[i].transform.position.y + 1;
                pos.z = cells[i].transform.position.x;

                Instantiate(Airport);
                Airport.transform.localPosition = pos;
            }

            if (cells[i].building == "nuclear")
            {
                Vector3 pos;
                pos.x = cells[i].transform.position.x + 5;
                pos.y = cells[i].transform.position.y + 1;
                pos.z = cells[i].transform.position.z - 18;

                Instantiate(Nuclear);
                Nuclear.transform.localPosition = pos;

            }

            if (cells[i].building == "fuel")
            {
                Vector3 pos;
                pos.x = cells[i].transform.position.x - 25;
                pos.y = cells[i].transform.position.y + 1;
                pos.z = cells[i].transform.position.z - 22;

                Instantiate(Oil);
                Oil.transform.localPosition = pos;
            }
            if (cells[i].building == "power")
            {
                Vector3 pos;
                pos.x = cells[i].transform.position.x;
                pos.y = cells[i].transform.position.y + 1;
                pos.z = cells[i].transform.position.z - 15;
                Instantiate(Coal);
                Coal.transform.localPosition = pos;
            }


        }
    }
}
