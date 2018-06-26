using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class HexCell:MonoBehaviour{

    public HexCellData data = new HexCellData();

    public HexCoordinates coordinates;
    public RectTransform uiRect;
    public string type;
    public string building;
    public string name;
    public bool hasUnit = false;
    public HexGrid hexGrid;
    public GameObject Airport;
    public GameObject City;
    public GameObject Coal;
    public GameObject Nuclear;
    public GameObject Oil;
    public HexGridChunk chunk;
    public int cellNum;
    
    int[] neighborNum = new int[6];

    public void render()
    {
        if (building == "city")
        {
            Vector3 pos;
            pos.x = transform.position.x;
            pos.y = transform.position.y;
            pos.z = transform.position.z - 10;

            //Debug.Log(pos.x + "," + pos.y + "," + pos.z);

            GameObject cityInstance = Instantiate(City) as GameObject;
            cityInstance.transform.localPosition = pos;
            cityInstance.transform.SetParent(transform);
            pos.z = transform.position.z;
        }
        else if (building == "airport")
        {
            Vector3 pos;
            pos.x = transform.position.x;
            pos.y = transform.position.y + 1;
            pos.z = transform.position.z;

            GameObject airportInstance = Instantiate(Airport) as GameObject;
            airportInstance.transform.localPosition = pos;
            airportInstance.transform.SetParent(transform);
        }

        else if (building == "nuclear")
        {
            Vector3 pos;
            pos.x = transform.position.x +5;
            pos.y = transform.position.y ;
            pos.z = transform.position.z -10;

            GameObject nuclearInstance = Instantiate(Nuclear) as GameObject;
            nuclearInstance.transform.localPosition = pos;
            nuclearInstance.transform.SetParent(transform);

        }

        else if (building == "fuel")
        {
            Vector3 pos;
            pos.x = transform.position.x -25;
            pos.y = transform.position.y ;
            pos.z = transform.position.z -22;

            GameObject fuelInstance = Instantiate(Oil) as GameObject;
            fuelInstance.transform.localPosition = pos;
            fuelInstance.transform.SetParent(transform);
        }
        else if (building == "power")
        {
            Vector3 pos;
            pos.x = transform.position.x;
            pos.y = transform.position.y ;
            pos.z = transform.position.z -15;
            GameObject coalInstance = Instantiate(Coal) as GameObject;
            coalInstance.transform.localPosition = pos;
            coalInstance.transform.SetParent(transform);
        }
    }

    private void Awake()
    {
        for (int i = 0; i < 6; i++)
            neighborNum[i] = int.MaxValue;
    }
    private void Update()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        render();
    }

    public Color Color {
		get {
			return color;
		}
		set {
			if (color == value) {
				return;
			}
			color = value;
			Refresh();
		}
	}

	public int Elevation {
		get {
			return elevation;
		}
		set {
			if (elevation == value) {
				return;
			}
			elevation = value;
			Vector3 position = transform.localPosition;
			position.y = value * HexMetrics.elevationStep;
			position.y +=
				(HexMetrics.SampleNoise(position).y * 2f - 1f) *
				HexMetrics.elevationPerturbStrength;
			transform.localPosition = position;

			Vector3 uiPosition = uiRect.localPosition;
			uiPosition.z = -position.y;
			uiRect.localPosition = uiPosition;

			if (
				hasOutgoingRiver &&
				elevation < GetNeighbor(outgoingRiver).elevation
			) {
				RemoveOutgoingRiver();
			}
			if (
				hasIncomingRiver &&
				elevation > GetNeighbor(incomingRiver).elevation
			) {
				RemoveIncomingRiver();
			}

			for (int i = 0; i < roads.Length; i++) {
				if (roads[i] && GetElevationDifference((HexDirection)i) > 1) {
					SetRoad(i, false);
				}
			}

			Refresh();
		}
	}

	public bool HasIncomingRiver {
		get {
			return hasIncomingRiver;
		}
	}

	public bool HasOutgoingRiver {
		get {
			return hasOutgoingRiver;
		}
	}

	public bool HasRiver {
		get {
			return hasIncomingRiver || hasOutgoingRiver;
		}
	}

	public bool HasRiverBeginOrEnd {
		get {
			return hasIncomingRiver != hasOutgoingRiver;
		}
	}

	public HexDirection RiverBeginOrEndDirection {
		get {
			return hasIncomingRiver ? incomingRiver : outgoingRiver;
		}
	}

	public bool HasRoads {
		get {
			for (int i = 0; i < roads.Length; i++) {
				if (roads[i]) {
					return true;
				}
			}
			return false;
		}
	}

	public HexDirection IncomingRiver {
		get {
			return incomingRiver;
		}
	}

	public HexDirection OutgoingRiver {
		get {
			return outgoingRiver;
		}
	}

	public Vector3 Position {
		get {
			return transform.localPosition;
		}
	}

    public float RiverSurfaceY
    {
        get
        {
            return
                (elevation + HexMetrics.waterElevationOffset) *
                HexMetrics.elevationStep;
        }
    }

    public float WaterSurfaceY
    {
        get
        {
            return
                (waterLevel + HexMetrics.waterElevationOffset) *
                HexMetrics.elevationStep;
        }
    }

    public float StreamBedY {
		get {
			return
				(elevation + HexMetrics.streamBedElevationOffset) *
				HexMetrics.elevationStep;
		}
	}

	Color color;

	int elevation = int.MinValue;

	bool hasIncomingRiver, hasOutgoingRiver;
	HexDirection incomingRiver, outgoingRiver;
	
    
	[SerializeField]
	bool[] roads;
    public HexCell[] neighbors;

    public HexCell GetNeighbor (HexDirection direction) {
        Debug.Log("direction at get: "+(int)direction);
		return neighbors[(int)direction];
	}

	public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
        Debug.Log((int)direction);
        neighborNum[(int)direction] = cell.cellNum;
		cell.neighbors[(int)direction.Opposite()] = this;
        cell.neighborNum[(int)direction.Opposite()] = cellNum;
	}

    public void SetNeighborsFromData(int[] neighborNums)
    {
        HexGrid hexgr = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        for(int i = 0; i < neighborNums.Length; i ++)
        {
            Debug.Log("neighbor nums at "+ i +": "+neighborNums[i]);
            Debug.Log("# of cells in grid: " + hexgr.cells.Count);
            bool neighborsBoolCheck = (neighborNums[i] != int.MaxValue);
            if (neighborNums[i] != int.MaxValue)
                neighbors[i] = hexgr.cells[neighborNums[i]];
        }
    }

	public HexEdgeType GetEdgeType (HexDirection direction) {
		return HexMetrics.GetEdgeType(
			elevation, neighbors[(int)direction].elevation
		);
	}

	public HexEdgeType GetEdgeType (HexCell otherCell) {
		return HexMetrics.GetEdgeType(
			elevation, otherCell.elevation
		);
	}

	public bool HasRiverThroughEdge (HexDirection direction) {
		return
			hasIncomingRiver && incomingRiver == direction ||
			hasOutgoingRiver && outgoingRiver == direction;
	}

	public void RemoveIncomingRiver () {
		if (!hasIncomingRiver) {
			return;
		}
		hasIncomingRiver = false;
		RefreshSelfOnly();

		HexCell neighbor = GetNeighbor(incomingRiver);
		neighbor.hasOutgoingRiver = false;
		neighbor.RefreshSelfOnly();
	}

	public void RemoveOutgoingRiver () {
		if (!hasOutgoingRiver) {
			return;
		}
		hasOutgoingRiver = false;
		RefreshSelfOnly();

		HexCell neighbor = GetNeighbor(outgoingRiver);
		neighbor.hasIncomingRiver = false;
		neighbor.RefreshSelfOnly();
	}

	public void RemoveRiver () {
		RemoveOutgoingRiver();
		RemoveIncomingRiver();
	}

	public void SetOutgoingRiver (HexDirection direction) {
		if (hasOutgoingRiver && outgoingRiver == direction) {
			return;
		}

		HexCell neighbor = GetNeighbor(direction);
		if (!neighbor || elevation < neighbor.elevation) {
			return;
		}

		RemoveOutgoingRiver();
		if (hasIncomingRiver && incomingRiver == direction) {
			RemoveIncomingRiver();
		}
		hasOutgoingRiver = true;
		outgoingRiver = direction;

		neighbor.RemoveIncomingRiver();
		neighbor.hasIncomingRiver = true;
		neighbor.incomingRiver = direction.Opposite();

		SetRoad((int)direction, false);
	}

	public bool HasRoadThroughEdge (HexDirection direction) {
		return roads[(int)direction];
	}

	public void AddRoad (HexDirection direction) {
		if (
			!roads[(int)direction] && !HasRiverThroughEdge(direction) &&
			GetElevationDifference(direction) <= 1
		) {
			SetRoad((int)direction, true);
		}
	}

	public void RemoveRoads () {
		for (int i = 0; i < neighbors.Length; i++) {
			if (roads[i]) {
				SetRoad(i, false);
			}
		}
	}

	public int GetElevationDifference (HexDirection direction) {
		int difference = elevation - GetNeighbor(direction).elevation;
		return difference >= 0 ? difference : -difference;
	}

	void SetRoad (int index, bool state) {
		roads[index] = state;
		neighbors[index].roads[(int)((HexDirection)index).Opposite()] = state;
		neighbors[index].RefreshSelfOnly();
		RefreshSelfOnly();
	}

	void Refresh () {
		if (chunk) {
			chunk.Refresh();
			for (int i = 0; i < neighbors.Length; i++) {
				HexCell neighbor = neighbors[i];
				if (neighbor != null && neighbor.chunk != chunk) {
					neighbor.chunk.Refresh();
				}
			}
		}
	}

	void RefreshSelfOnly () {
		chunk.Refresh();
	}

    public int WaterLevel
    {
        get
        {
            return waterLevel;
        }
        set
        {
            if (waterLevel == value)
            {
                return;
            }
            waterLevel = value;
            Refresh();
        }
    }

    int waterLevel;

    public bool IsUnderwater
    {
        get
        {
            return waterLevel > elevation;
        }
    }

    public void StoreData()
    {
        data.type = type;
        data.building = building;
        data.name = name;
        data.roads = roads;
        data.xpos = coordinates.X;
        data.ypos = coordinates.Y;
        data.zpos = coordinates.Z;
        data.x = transform.position.x;
        data.y = transform.position.y;
        data.z = transform.position.z;
        data.cellNum = cellNum;
        data.neighbors = neighborNum;
    }

    public void LoadData()
    {
        type = data.type;
        building = data.building;
        name = data.name;
        roads = data.roads;
        coordinates = HexCoordinates.FromOffsetCoordinates(data.xpos, data.zpos);
        cellNum = data.cellNum;
        Vector3 pos;
        pos.x = data.x;
        pos.y = data.y;
        pos.z = data.z;
        transform.localPosition = pos;
        neighborNum = data.neighbors;
    }

    public void OnEnable()
    {
        SaveData.OnLoaded += delegate { LoadData(); };
        SaveData.OnBeforeSave += delegate { StoreData(); };
        SaveData.OnBeforeSave += delegate { SaveData.AddCellData(data); };
    }

    public void OnDisable()
    {
        SaveData.OnLoaded += delegate { LoadData(); };
        SaveData.OnBeforeSave += delegate { StoreData(); };
        SaveData.OnBeforeSave += delegate { SaveData.AddCellData(data); };
    }
}
public class HexCellData
{
    [XmlAttribute("Type")]
    public string type;
    [XmlAttribute("Building")]
    public string building;
    [XmlAttribute("Name")]
    public string name;
    [XmlElement("Roads")]
    public bool[] roads;
    [XmlElement("xpos")]
    public int xpos;
    [XmlElement("ypos")]
    public int ypos;
    [XmlElement("zpos")]
    public int zpos;
    [XmlElement("x")]
    public float x;
    [XmlElement("y")]
    public float y;
    [XmlElement("z")]
    public float z;
    [XmlElement("cellNum")]
    public int cellNum;
    [XmlArray("neighbors")]
    public int[] neighbors;
}

[XmlRoot("HexCellCollection")]
public class HexCellContainer
{
    [XmlElement("cellcountx")]
    public int cellcountx;
    [XmlElement("cellcounty")]
    public int cellcounty;
    [XmlArray("HexCells")]
    [XmlArrayItem("HexCell")]
    public List<HexCellData> cells = new List<HexCellData>();
}