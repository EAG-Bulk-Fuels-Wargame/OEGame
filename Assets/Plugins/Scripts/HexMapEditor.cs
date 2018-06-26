using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System.Xml.Serialization;
using UnityEngine.UI;
using UnityEditor;

public class HexMapEditor : MonoBehaviour {


    public Button saveButton;
    public Button loadButton;
    public string dataPath = string.Empty;

    public Color[] colors;
    public string[] buildings;

    public HexGrid hexGrid;

    int activeElevation;
    int activeWaterLevel;

    Color activeColor;
    string activeBuilding;

    int brushSize;

    bool applyColor;
    bool applyElevation = true;
    bool applyWaterLevel = true;

    enum OptionalToggle {
        Ignore, Yes, No
    }

    OptionalToggle riverMode, roadMode;

    bool isDrag;
    HexDirection dragDirection;
    HexCell previousCell;

    public void SelectColor(int index) {
        applyColor = index > 0;
        if (applyColor) {
            activeColor = colors[index];
            activeBuilding = buildings[index];
        }
    }

    public void SetApplyElevation(bool toggle) {
        applyElevation = toggle;
    }

    public void SetElevation(float elevation) {
        activeElevation = (int)elevation;
    }

    public void SetBrushSize(float size) {
        brushSize = (int)size;
    }

    public void SetRiverMode(int mode) {
        riverMode = (OptionalToggle)mode;
    }

    public void SetRoadMode(int mode) {
        roadMode = (OptionalToggle)mode;
    }

    public void SetApplyWaterLevel(bool toggle)
    {
        applyWaterLevel = toggle;
    }

    public void SetWaterLevel(float level)
    {
        activeWaterLevel = (int)level;
    }

    public void ShowUI(bool visible) {
        hexGrid.ShowUI(visible);
    }

    void Awake()
    {
        SelectColor(0);
        dataPath = System.IO.Path.Combine(Application.dataPath, @"Resources\cells.xml");
        Debug.Log(dataPath);
        //EditorUtility.DisplayDialog("Data Path", dataPath, "OK", "Cancel");
    }

    


    void Update()
    {
        if (
            Input.GetMouseButton(0) &&
            !EventSystem.current.IsPointerOverGameObject()
        )
        {
            HandleInput();
        }
        else
        {
            previousCell = null;
        }
    }

    void OnEnable()
    {
        saveButton.onClick.AddListener(delegate { SaveData.Save(dataPath, SaveData.hexCellContainer); });
        loadButton.onClick.AddListener(delegate { SaveData.Load(dataPath); });

    }

    void OnDisable()
    {
        saveButton.onClick.RemoveListener(delegate { SaveData.Save(dataPath, SaveData.hexCellContainer); });
        loadButton.onClick.RemoveListener(delegate { SaveData.Load(dataPath); });

    }

    void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			HexCell currentCell = hexGrid.GetCell(hit.point);
			if (previousCell && previousCell != currentCell) {
				ValidateDrag(currentCell);
			}
			else {
				isDrag = false;
			}
			EditCells(currentCell);
			previousCell = currentCell;
		}
		else {
			previousCell = null;
		}
	}

	void ValidateDrag (HexCell currentCell) {
		for (
			dragDirection = HexDirection.NE;
			dragDirection <= HexDirection.NW;
			dragDirection++
		) {
			if (previousCell.GetNeighbor(dragDirection) == currentCell) {
				isDrag = true;
				return;
			}
		}
		isDrag = false;
	}

	void EditCells (HexCell center) {
		int centerX = center.coordinates.X;
		int centerZ = center.coordinates.Z;

		for (int r = 0, z = centerZ - brushSize; z <= centerZ; z++, r++) {
			for (int x = centerX - r; x <= centerX + brushSize; x++) {
				EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}
		for (int r = 0, z = centerZ + brushSize; z > centerZ; z--, r++) {
			for (int x = centerX - brushSize; x <= centerX + r; x++) {
				EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}

	}

	void EditCell (HexCell cell) {
		if (cell) {
			if (applyColor) {
				cell.Color = activeColor;
                cell.building = activeBuilding;
			}
			if (applyElevation) {
				cell.Elevation = activeElevation;
			}
			if (riverMode == OptionalToggle.No) {
				cell.RemoveRiver();
			}
			if (roadMode == OptionalToggle.No) {
				cell.RemoveRoads();
			}
            if (applyWaterLevel)
            {
                cell.WaterLevel = activeWaterLevel;
            }
            if (isDrag) {
				HexCell otherCell = cell.GetNeighbor(dragDirection.Opposite());
				if (otherCell) {
					if (riverMode == OptionalToggle.Yes) {
						otherCell.SetOutgoingRiver(dragDirection);
					}
					if (roadMode == OptionalToggle.Yes) {
						otherCell.AddRoad(dragDirection);
					}
				}
			}
		}
	}
}    
    

