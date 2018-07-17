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
    public string fuelRegion;
    private bool bbreakaway;
    public bool hasUnit = false;
    public HexGrid hexGrid;
    public GameObject Airport;
    public GameObject City;
    public GameObject Coal;
    public GameObject Nuclear;
    public GameObject Oil;
    public HexGridChunk chunk;
    public int cellNum;
    public Color cDefault;
    public Color cBreakaway;
    public Color cOcean;
    public Color cCity;
    public Color cPower;
    public Color cNuclear;
    public Color cOil;
    public Color cAirport;

    int[] neighborNum = new int[6];

    public void render() //Places buildings on cells of proper type
    {
        if (building == "city")
        {
           
            Vector3 pos;
            pos.x = transform.position.x;
            pos.y = transform.position.y;
            pos.z = transform.position.z - 10;
            /*
            //Debug.Log(pos.x + "," + pos.y + "," + pos.z);

            GameObject cityInstance = Instantiate(City) as GameObject;
            cityInstance.transform.localPosition = pos;
            cityInstance.transform.SetParent(transform);
            pos.z = transform.position.z;
            */
            City city = new City("name", 32, 0 , 1, pos);

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

    public bool GetCity()
    {
        if(building == "city")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //replace ReturnToDefaultColor with this
    public void ResetColor()
    {
        HexMapEditor heme = GameObject.FindObjectOfType<HexMapEditor>();
        switch (building)
        {
            case "city":
                color = cCity;
                break;
            case "power":
                color = cPower;
                break;
            case "nuclear":
                color = cNuclear;
                break;
            case "fuel":
                color = cOil;
                break;
            case "airport":
                color = cAirport;
                break;
            case "":
                switch (type)
                {
                    case "Breakaway":
                        color = cBreakaway;
                        bbreakaway = true;
                        break;
                    case "Ocean":
                        color = cOcean;
                        break;
                    case "":
                        color = cDefault;     
                        break;
                }
                break;
        }
        Refresh();
    }

    public bool getBrakeaway()
    {
        return bbreakaway;

    }

    private void Awake() 
    {
        //Fills the neighborNum list with garbage that can be filtered out later
        for (int i = 0; i < 6; i++)
            neighborNum[i] = int.MaxValue;
        //Adds colors
        ColorUtility.TryParseHtmlString("#9CB280", out cDefault);
        ColorUtility.TryParseHtmlString("#FF9999", out cBreakaway);
        ColorUtility.TryParseHtmlString("#D6D6CC", out cOcean);
        ColorUtility.TryParseHtmlString("#808080", out cCity);
        ColorUtility.TryParseHtmlString("#FF8A00", out cPower);
        ColorUtility.TryParseHtmlString("#FFFF00", out cNuclear);
        ColorUtility.TryParseHtmlString("#FFBFCC", out cOil);
        ColorUtility.TryParseHtmlString("#33FFFF", out cAirport);
    }

    private void Update()
    {
        //Wipes out building models
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        //Replaces models
        render();
    }

    public Color Color { //Accessor for color
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
	public int Elevation { //Accessor for elevation
		get {
			return elevation;
		}
		set {
			if (elevation == value) {
				return;
			}

            //Changes elevation based on HexMetrics standards
			elevation = value;
			Vector3 position = transform.localPosition;
			position.y = value * HexMetrics.elevationStep;
			position.y +=
				(HexMetrics.SampleNoise(position).y * 2f - 1f) *
				HexMetrics.elevationPerturbStrength;
			transform.localPosition = position;

            //repositions ui labels
			Vector3 uiPosition = uiRect.localPosition;
			uiPosition.z = -position.y;
			uiRect.localPosition = uiPosition;

            //breaks rivers and roads
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

	public bool HasIncomingRiver { //Getter for hasIncomingRiver
		get {
			return hasIncomingRiver;
		}
	}

	public bool HasOutgoingRiver { //Getter for hasIncomingRiver
		get {
			return hasOutgoingRiver;
		}
	}

	public bool HasRiver { //Returns true if a river is present
		get {
			return hasIncomingRiver || hasOutgoingRiver;
		}
	}

	public bool HasRiverBeginOrEnd { //Returns true of a river origin is present
		get {
			return hasIncomingRiver != hasOutgoingRiver;
		}
	} 

	public HexDirection RiverBeginOrEndDirection { //Returns direction of river
		get {
			return hasIncomingRiver ? incomingRiver : outgoingRiver;
		}
	} 

	public bool HasRoads {  //Returns true if roads are present
		get {
			for (int i = 0; i < roads.Length; i++) {
				if (roads[i]) {
					return true;
				}
			}
			return false;
		}
	}

	public HexDirection IncomingRiver { //Returns direction  of incoming river
		get {
			return incomingRiver;
		}
	} 

	public HexDirection OutgoingRiver { //Returns direction of outgoing river 
		get {
			return outgoingRiver;
		}
	} 

	public Vector3 Position { //Returns position of cell in worldspace
		get {
			return transform.localPosition;
		}
	}

    public float RiverSurfaceY { //Returns elevation of river surface
        get
        {
            return
                (elevation + HexMetrics.waterElevationOffset) *
                HexMetrics.elevationStep;
        }
    }

    public float WaterSurfaceY 
    { //Returns elevation of water
        get
        {
            return
                (waterLevel + HexMetrics.waterElevationOffset) *
                HexMetrics.elevationStep;
        }
    }

    public float StreamBedY
    { //Returns elevation of streambed
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

    public HexCell GetNeighbor (HexDirection direction) { //Returns neighboring cell at direction
        //Debug.Log("direction at get: "+(int)direction);
		return neighbors[(int)direction];
	}

	public void SetNeighbor (HexDirection direction, HexCell cell) { //Sets the neighbor in a given direction
		neighbors[(int)direction] = cell;
        neighborNum[(int)direction] = cell.cellNum; //Stores neighbor's number in array
		cell.neighbors[(int)direction.Opposite()] = this; //Sets neighbor of neighboring cell to this one
        cell.neighborNum[(int)direction.Opposite()] = cellNum; //Stores this cell's number in array of neighbor
	}

    public void SetNeighborsFromData(int[] neighborNums) //Takes list of neighbor numbers and assigns the corresponding cells as neighbors
    {
        HexGrid hexgr = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        for(int i = 0; i < neighborNums.Length; i ++)
        {
            //checks if there's actually a neighbor or just garbage
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

    public List<HexCell> GetRadiusOfCells(HexCell hex, int rad)
    {
        List<HexCell> tileArea = new List<HexCell>();
        HexCell secondaryTile = hex;
        HexDirection secDir = HexDirection.E;
        HexDirection[] eastDirPath = new HexDirection[] { HexDirection.SW, HexDirection.W, HexDirection.NW, HexDirection.NE, HexDirection.E, HexDirection.SE };
        HexDirection[] eastRevDirPath = new HexDirection[] { HexDirection.NW, HexDirection.W, HexDirection.SW, HexDirection.SE, HexDirection.E, HexDirection.NE };
        HexDirection[] westDirPath = new HexDirection[] { HexDirection.NE, HexDirection.E, HexDirection.SE, HexDirection.SW, HexDirection.W, HexDirection.NW };
        HexDirection[] westRevDirPath = new HexDirection[] { HexDirection.SE, HexDirection.E, HexDirection.NE, HexDirection.NW, HexDirection.W, HexDirection.SW };
        HexDirection[] forwardPath;
        HexDirection[] reversePath;
        for (int i = 0; i < rad + 1; i++)
        {
            if (secondaryTile != null)
            {
                secondaryTile = secondaryTile.GetNeighbor(HexDirection.E);
            }
            else
            {
                secDir = HexDirection.W;
            }
        }
        if (secDir == HexDirection.E)
        {
            forwardPath = eastDirPath;
            reversePath = eastRevDirPath;
        }
        else
        {
            forwardPath = westDirPath;
            reversePath = westRevDirPath;
        }
        tileArea.Add(hex);
        HexCell startTile = hex;
        for (int i = 0; i < rad; i++)
        {
            startTile = startTile.GetNeighbor(secDir);
            HexCell tile = startTile;
            bool rev = false;
            foreach (HexDirection direx in forwardPath)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    if (tile != null && tile.GetNeighbor(direx) != null && !rev)
                    {
                        tile = tile.GetNeighbor(direx);
                        tileArea.Add(tile);
                    }
                    else
                    {
                        rev = true;
                    }
                }
            }
            if (rev)
            {
                tileArea.Add(startTile);
                tile = startTile;
                bool finished = false;
                foreach (HexDirection direx in reversePath)
                {
                    for (int j = 0; j < i + 1; j++)
                    {
                        if (tile != null && tile.GetNeighbor(direx) != null && !finished)
                        {
                            tile = tile.GetNeighbor(direx);
                            tileArea.Add(tile);
                        }
                        else
                        {
                            finished = true;
                        }
                    }
                }
            }
        }
        return tileArea;
    }

    public int GetHexDistance(HexCell baseCell, HexCell distancedCell)
    {
        HexDirection[] cardinals = new HexDirection[] { HexDirection.E, HexDirection.NE, HexDirection.NW, HexDirection.SE, HexDirection.SW, HexDirection.W };
        List<HexCell> radList = new List<HexCell>();
        List<HexCell> addList = new List<HexCell>();
        int dist = 1;
        radList.Add(baseCell);
        if (baseCell == distancedCell)
            return 0;
        bool notExceeded = true;
        while (notExceeded)
        {
            foreach (HexCell hex in radList)
                foreach (HexDirection dir in cardinals)
                {
                    if (hex.GetNeighbor(dir) != null)
                        addList.Add(hex.GetNeighbor(dir));
                }
            foreach (HexCell hex in addList)
            {
                if (hex == distancedCell)
                    return dist;
            }
            for (int i = 0; i < radList.Count; i++)
            {
                for (int j = 0; j < addList.Count; j++)
                {
                    if (radList[i] == addList[j])
                    {
                        addList.RemoveAt(j);
                        j--;
                    }
                }
            }
            for (int i = 0; i < addList.Count; i++)
            {
                for (int j = 0; j < addList.Count; j++)
                {
                    if (i != j)
                    {
                        if (addList[i] == addList[j])
                        {
                            addList.RemoveAt(j);
                            j--;
                        }
                    }
                }
            }
            radList.Clear();
            foreach (HexCell hex in addList)
                radList.Add(hex);
            addList.Clear();
            dist++;
            if (radList.Count == 0)
            {
                notExceeded = false;
            }
        }
        return -1;
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
        data.color = Color;
    }

    public void LoadData()
    {
        type = data.type;
        building = data.building;
        name = data.name;
        roads = data.roads;
        coordinates = HexCoordinates.FromOffsetCoordinates(data.xpos, data.zpos);
        cellNum = data.cellNum;
        Color = data.color;
        Vector3 pos;
        pos.x = data.x;
        pos.y = data.y;
        pos.z = data.z;
        transform.localPosition = pos;
        neighborNum = data.neighbors;
    }

    public void OnEnable()
    {
        Debug.Log("enabled cell " + cellNum);
        SaveData.OnBeforeLoaded += delegate {
            Destroy(this, 0);
            Debug.Log("Should destroy here");
        };
        SaveData.OnLoaded += delegate {
            if(this != null)
                LoadData();
        };
        SaveData.OnBeforeSave += delegate {
            HexCellData data = new HexCellData();
        };
        SaveData.OnBeforeSave += delegate {
            if (this != null)
            {
                //Debug.Log("Storing cell data...");
                //StoreData();
            }
        };
        SaveData.OnBeforeSave += delegate {
            if (this != null)
            {
                Debug.Log("Storing cell data...");
                Debug.Log("Coords: " + coordinates.X + ", " + coordinates.Y + ", " + coordinates.Z + ", ");
                StoreData();

                if (!SaveData.cellNumList.Contains(cellNum))
                {
                    Debug.Log("haha I'm adding data, my name is cell #" + cellNum);
                    Debug.Log("the cellnum list is now " + SaveData.cellNumList.Count + " cells long");
                    SaveData.cellNumList.Add(cellNum);
                    SaveData.AddCellData(data);
                }
            }
        };
    }

    public void OnDisable()
    {
        Debug.Log("disabled cell " +cellNum);
        SaveData.OnBeforeLoaded += delegate { Destroy(this, 0); };
        SaveData.OnLoaded += delegate {
            if (this != null)
                LoadData();
        };
        SaveData.OnBeforeSave += delegate {
            HexCellData data = new HexCellData();
        };
        SaveData.OnBeforeSave += delegate {
            if (this != null)
            {
                //Debug.Log("Storing cell data...");
                //StoreData();
            }
        };
        SaveData.OnBeforeSave += delegate {
            if (this != null)
            {
                Debug.Log("Storing cell data...");
                StoreData();

                if (!SaveData.cellNumList.Contains(cellNum))
                {
                    Debug.Log("haha I'm adding data, my name is cell #" + cellNum);
                    Debug.Log("the cellnum list is now " + SaveData.cellNumList.Count + " cells long");
                    SaveData.cellNumList.Add(cellNum);
                    SaveData.AddCellData(data);
                }
            }
        };
        
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
    [XmlElement("Color")]
    public Color color;
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
 