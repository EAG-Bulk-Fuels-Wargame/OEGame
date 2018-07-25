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
    public List<Unit> units;
    private int cellCountX, cellCountZ;
    private HexGridChunk[] chunks;
    public FuelRegion[] Regions;
    public GameObject Line;
    public List<GameObject> Lines;
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

        Regions = new FuelRegion[HexMetrics.fuelRegionCount];


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
        PopulateCellData(cells, ocean, city, power, nuclear, airport, pipeline);
        PopulateUnits();
        PopulateFuelRegions(cells, 2000000);
        foreach (HexCell cell in cells)
            cell.render();
        
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

    public void AddCities(City city)
    {

        Cities.Add(city);
 
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
        int chunkX = x / HexMetrics.chunkSizeX;
        int chunkZ = z / HexMetrics.chunkSizeZ;
        HexGridChunk chunk = chunks[0];
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
    private void PopulateCellData(List<HexCell> cells, Color ocean, Color city, Color power, Color nuclear, Color airport, Color pipeline)
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

        cells[57].Color = city;
        cells[57].name = "South City";
        cells[57].building = "city";
        cells[58].Color = city;
        cells[58].name = "South City";
        cells[58].building = "city";

        cells[78].Color = city;
        cells[78].name = "Hagers Town";
        cells[78].building = "city";

        cells[121].Color = city;
        cells[121].name = "Midtown";
        cells[121].building = "city";
        cells[136].Color = city;
        cells[136].name = "Midtown";
        cells[136].building = "city";

        cells[130].Color = city;
        cells[130].name = "Port Town";
        cells[130].building = "city";

        cells[146].Color = city;
        cells[146].name = "Port Town";
        cells[146].building = "city";

        cells[173].Color = city;
        cells[173].name = "Fate City";
        cells[173].building = "city";
        cells[174].Color = city;
        cells[174].name = "Fate City";
        cells[174].building = "city";

        cells[200].Color = city;
        cells[200].name = "Capital City";
        cells[200].building = "city";
        cells[216].Color = city;
        cells[216].name = "Capital City";
        cells[216].building = "city";
        cells[217].Color = city;
        cells[217].name = "Capital City";
        cells[217].building = "city";

        cells[237].Color = city;
        cells[237].name = "Charlie City";
        cells[237].building = "city";
        cells[254].Color = city;
        cells[254].name = "Charlie City";
        cells[254].building = "city";

        cells[276].Color = city;
        cells[276].name = "North City";
        cells[276].building = "city";
        cells[276].Color = city;
        cells[277].name = "North City";
        cells[277].building = "city";

        cells[282].Color = city;
        cells[282].name = "Beta Town";
        cells[282].building = "city";

        cells[91].Color = power;
        cells[91].name = "South Power Plant";
        cells[91].building = "power";

        cells[101].Color = power;
        cells[101].name = "Port Power Plant";
        cells[101].building = "power";

        cells[158].Color = power;
        cells[158].name = "Breakaway Power Plant";
        cells[158].building = "power";

        cells[168].Color = nuclear;
        cells[168].name = "Capital Nuclear Plant";
        cells[168].building = "nuclear";

        cells[261].Color = power;
        cells[261].name = "North Renewable Plant";
        cells[261].building = "power";

        cells[260].Color = fuel;
        cells[260].name = "North Fuel Depot";
        cells[260].building = "fuel";

        cells[199].Color = fuel;
        cells[199].name = "Capital Fuel Depot";
        cells[199].building = "fuel";

        cells[137].Color = fuel;
        cells[137].name = "Midtown Fuel Depot";
        cells[137].building = "fuel";

        cells[54].Color = fuel;
        cells[54].name = "South Fuel Depot";
        cells[54].building = "fuel";

        cells[70].Color = airport;
        cells[70].name = "South Airport";
        cells[70].building = "airport";

        cells[214].Color = airport;
        cells[214].name = "Capital Airport";
        cells[214].building = "airport";

        cells[263].Color = pipeline;
        cells[263].name = "Pipeline Station";
        cells[263].building = "pipeline";

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

    private void PopulateUnits()
    {
        List<HexCell> goodCities = new List<HexCell>();
        List<HexCell> badCities = new List<HexCell>();

        foreach (HexCell cell in cells)
        {
            if (cell.building == "city" && cell.type != "Breakaway")
                goodCities.Add(cell);
            else if(cell.building == "city" && cell.type == "Breakaway")
                badCities.Add(cell);
        }
        UnitAct.PopulateUnits(goodCities, "blue");
        UnitAct.PopulateUnits(badCities, "red");
    }

    private void PopulateFuelRegions(List<HexCell> cells, int fuelQty)
    {
        for (int i = 0; i < HexMetrics.fuelRegionCount; i ++)
        {
            Lines.Add(GameObject.Instantiate(Line) as GameObject);
            Regions[i] = new FuelRegion("Region " + (i+1), fuelQty / HexMetrics.fuelRegionCount);

            HexCell startCell = cells[i * ((cells.Count / HexMetrics.fuelRegionCount))];
            HexCell endCell = cells[(((i * cells.Count / HexMetrics.fuelRegionCount)) + (cells.Count / HexMetrics.fuelRegionCount)) -1];
            for (int j =  i*((cells.Count / HexMetrics.fuelRegionCount) ); 
                j < (((i * cells.Count / HexMetrics.fuelRegionCount)) + (cells.Count / HexMetrics.fuelRegionCount)); 
                j ++)
            {
                //Debug.Log("i = " + i + ", j = " + j);
                cells[j].fuelRegion = Regions[i];
            }
            LineRenderer Renderer = Lines[i].GetComponent<LineRenderer>();

            if (i != 0)
            {
                Vector3 start = new Vector3(startCell.transform.position.x, 4, startCell.transform.position.z - HexMetrics.innerRadius);
                Vector3 end = new Vector3(endCell.transform.position.x, 4, startCell.transform.position.z - HexMetrics.innerRadius);
                var distance = Vector3.Distance(start, end);
                Renderer.SetPosition(0, start);
                Renderer.SetPosition(1, end);
                Renderer.materials[0].mainTextureScale = new Vector3(distance, 1, 1);

            }
        }
    }

    private void DrawLines()
    {
        
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