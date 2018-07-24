using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public Color airport;
    public GameObject Airport;
    public Color breakaway;
    public Collider2D cellCollider;
    public Text cellLabelPrefab;
    public HexCell cellPrefab;
    public List<HexCell> cells;
    public int chunkCountX = 1, chunkCountZ = 1;
    public HexGridChunk chunkPrefab;
    public Color city;
    public GameObject City;
    public GameObject Coal;
    public Color defaultColor;
    public Color fuel;
    public Texture2D noiseSource;
    public Color nuclear;
    public GameObject Nuclear;
    public Color ocean;
    public GameObject Oil;
    public Color pipeline;
    public Color power;
    public Collider2D unitCollider;
    public int unitCount;
    public Unit unitPrefab;
    public List<Unit> units;
    private int cellCountX, cellCountZ;
    private HexGridChunk[] chunks;
    private GameObject[] cities;
    private List<City> Cities;
    private City xity;
    private List<WindTurbine> Turbines;
    private WindTurbine turbo;
    private List<Airport> airports;
    private Airport aport;
    private List<Nuclear> nukes;
    private Nuclear nuke;
    private List<PowerPlant> pplants;
    private PowerPlant pplant;
    private List<Depot> depots;
    private Depot depot;


    private void Awake()
    {
        HexMetrics.noiseSource = noiseSource;

        cellCountX = chunkCountX * HexMetrics.chunkSizeX;
        cellCountZ = chunkCountZ * HexMetrics.chunkSizeZ;

        unitCount = 17;
        units = new List<Unit>();

        defaultColor = new Color(.61F, .7F, .5F, 1);
        breakaway = new Color(1, .6F, .6F, 1);
        ocean = new Color(.84F, .84F, .8F, 1);
        city = new Color(.5F, .5F, .5F, 1);
        power = new Color(1F, .54F, 0, 1);
        nuclear = new Color(.9F, .9F, 0, 1);
        fuel = new Color(.9F, .65F, .70F, 1);
        airport = new Color(.1F, .9F, .9F, 1);
        pipeline = new Color(.3F, .3F, 1F, 1);

        CreateChunks();
        CreateCells();
        populateCellData(cells, ocean, city, power, nuclear, airport, pipeline);
        foreach (HexCell cell in cells)
            cell.render();
    }

    public void AddCities(City city)
    {

        Cities.Add(city);

    }

    public void CreateCells(HexCellContainer container)
    {
        cells = new List<HexCell>();
        int i = 0;

        foreach (HexCellData data in container.cells)
        {
            CreateCell(data, i++);
        }

        Debug.Log("Cells created, setting neighbors...");

        for (int j = 0; j < container.cells.Count; j++)
        {
            HexCell cell = cells[j];
            HexCellData data = container.cells[j];
            cell.SetNeighborsFromData(data.neighbors);
        }
    }

    public void DestroyCell(HexCell cell)
    {
        Destroy(cell);
    }

    public void DestroyCells()
    {
        var cell = GameObject.FindGameObjectsWithTag("Cell");

        foreach (var item in cell)
        {
            Destroy(item);
        }
    }

    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index =
            coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
 //       Debug.Log(index);
        if (index <= cells.Count)
            return cells[index];
        else
            return cells[cells.Count - 1];
    }

    public HexCell GetCell(HexCoordinates coordinates)
    {
        int z = coordinates.Z;
        if (z < 0 || z >= cellCountZ)
        {
            return null;
        }
        int x = coordinates.X + z / 2;
        if (x < 0 || x >= cellCountX)
        {
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

    public void ShowUI(bool visible)
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].ShowUI(visible);
        }
    }

    private void AddCellToChunk(int x, int z, HexCell cell)
    {
        //      Debug.Log("pre-chunk coords: " + x + ", " + z);
        int chunkX = x / HexMetrics.chunkSizeX;
        int chunkZ = z / HexMetrics.chunkSizeZ;
        //    Debug.Log("chunk coords: " + chunkX + ", " + chunkZ);
        HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];
        int localX = x - chunkX * HexMetrics.chunkSizeX;
        int localZ = z - chunkZ * HexMetrics.chunkSizeZ;
        chunk.AddCell(cell);
    }


    private void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        //    Debug.Log("ival" + i);
        HexCell cell = Instantiate<HexCell>(cellPrefab);
        cells.Add(cell);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.Color = defaultColor;
        cell.cellNum = i;

        SetNeighbors(cell, x, z, i);

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.anchoredPosition =
            new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
        cell.uiRect = label.rectTransform;

        cell.Elevation = 0;

        AddCellToChunk(x, z, cell);
    }

    private void CreateCell(HexCellData data, int i)
    {
        Vector3 position;
        position.x = data.xpos;
        position.y = data.ypos;
        position.z = data.zpos;

        HexCell cell = Instantiate<HexCell>(cellPrefab);
        cells.Add(cell);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(data.xpos, data.zpos);
        cell.Color = defaultColor;
        cell.data = data;

        Debug.Log("Cell #: " + data.cellNum);
        Debug.Log("data coords: " + data.xpos + ", " + data.zpos);
        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.anchoredPosition =
            new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
        cell.uiRect = label.rectTransform;

        cell.Elevation = 0;

        AddCellToChunk(data.xpos, data.zpos, cell);
    }

    private void CreateCells()
    {
        cells = new List<HexCell>();

        for (int z = 0, i = 0; z < cellCountZ; z++)
        {
            for (int x = 0; x < cellCountX; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    private void CreateChunks()
    {
        chunks = new HexGridChunk[chunkCountX * chunkCountZ];

        for (int z = 0, i = 0; z < chunkCountZ; z++)
        {
            for (int x = 0; x < chunkCountX; x++)
            {
                HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
                chunk.transform.SetParent(transform);
            }
        }
    }

    private void OnDisable()
    {

    }

    private void OnEnable()
    {
        HexMetrics.noiseSource = noiseSource;
    }
    private void populateCellData(List<HexCell> cells, Color ocean, Color city, Color power, Color nuclear, Color airport, Color pipeline)
    {
        foreach (HexCell item in cells)
        {
            item.Color = defaultColor;
            item.Elevation = 1;
        }
        for (int i = 0; i <= (cellCountX * cellCountZ) - 1; i = i + cellCountX) //colors base layer
        {
            //Debug.Log("Left Cell: " + (i));
            //Debug.Log("Right Cell: " + (i + width));

            cells[i].Color = ocean;
            cells[i].type = "Ocean";
            cells[i].Elevation = 0;
            cells[i + 1].Color = ocean;
            cells[i + 1].type = "Ocean";
            cells[i + 1].Elevation = 0;
            cells[i + (cellCountX - 3)].Color = breakaway;
            cells[i + (cellCountX - 3)].type = "Breakaway";
            cells[i + (cellCountX - 2)].Color = breakaway;
            cells[i + (cellCountX - 2)].type = "Breakaway";
            cells[i + (cellCountX - 1)].Color = breakaway;
            cells[i + (cellCountX - 1)].type = "Breakaway";
        }

        Cities = new List<City>();
        Turbines = new List<WindTurbine>();
        airports = new List<Airport>();
        nukes = new List<Nuclear>();
        pplants = new List<PowerPlant>();
        depots = new List<Depot>();


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
   //     Debug.Log(cells.Count);


        foreach (HexCell cell in cells)
        {

            if (cell.building == "city")
            {
                xity = new City(cell.name, 32, 0, 1, cell);
                Cities.Add(xity);
               // Debug.Log(xity.GetName());
            }
            else if (cell.building == "turbine")
            {
                turbo = new WindTurbine(cell.name, 100);
                Turbines.Add(turbo);
            }
            else if (cell.building == "airport")
            {
                aport = new Airport(cell.name, 100);
                airports.Add(aport);
            }
            else if (cell.building == "nuclear")
            {
                nuke = new Nuclear(cell.name, 100);
                nukes.Add(nuke);
            }
            else if (cell.building == "power")
            {
                pplant = new PowerPlant(cell.name, 100);
                pplants.Add(pplant);
            }
            else if (cell.building == "fuel")
            {
                depot = new Depot(cell.name, 100);
                depots.Add(depot);
            }
            else
            {

            }
        }
        //  int bleh = Cities.Count;

        //for (int i = 0; i < Cities.Count; i++)
        //   {
        //      Debug.Log(Cities[i].GetName());
        //  }
        int helper = Cities.Count;

        for (int i = 0; i < helper; i++)
        {
            for (int x = 0; x < helper; x++)
            {
                if (Cities[i].GetName() == Cities[x].GetName())
                {
                    Cities[i].CityMerge(Cities[x]);
                    Cities.RemoveAt(x);
                    helper--; 
                }
            }
         
        }

        for (int i = 0; i < Cities.Count; i++)
        {
        Debug.Log(Cities[i].GetName());
        }


        // Debug.Log(Cities[0].GetName());
        Debug.Log(Cities.Count);
    }

    public void AddCity(City city)
    {
        Cities.Add(city);

    }

    private void SetNeighbors(HexCell cell, int x, int z, int i)
    {
        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
                if (x < cellCountX - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
                }
            }
        }
    }
}