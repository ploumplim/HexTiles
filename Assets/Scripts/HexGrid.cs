using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

    // Grid dimensions
    public int width = 6;
    public int height = 6;

    // Default color for cells
    public Color defaultColor = Color.white;

    // Prefabs for cells and labels
    public HexCell cellPrefab;
    public Text cellLabelPrefab;
    
    public GameObject centralCellGO;
    public GameObject Camera;

    // Array to hold all cells
    public HexCell[] cells;

    // Canvas for grid labels
    Canvas gridCanvas;

    void Awake () {
        // Initialize canvas
        gridCanvas = GetComponentInChildren<Canvas>();

        // Initialize cells array
        cells = new HexCell[height * width];

        // Create cells in a grid pattern
        for (int z = 0, i = 0; z < height; z++) {
            for (int x = 0; x < width; x++) {
                CreateCell(x, z, i++);
            }
        }
    }

    void Start () {
        Debug.Log(GetCentralCell(cells, width, height).coordinates.ToString());
        centralCellGO.transform.position = GetCentralCell(cells, width, height).transform.position;
        Camera.transform.position = new Vector3(centralCellGO.transform.position.x, centralCellGO.transform.position.y + 60, centralCellGO.transform.position.z - 25);
    }
    
    
    
    public HexCell GetCentralCell(HexCell[] cells, int width, int height)
    {
        // Calculate the center coordinates
        int centerX = width / 2;
        int centerZ = height / 2;

        // Find the cell with the closest coordinates to the center
        foreach (HexCell cell in cells)
        {
            if (cell.coordinates.X == centerX && cell.coordinates.Z == centerZ)
            {
                return cell;
            }
        }

        // If no exact match is found, return null
        return null;
    }
    public HexCell GetCell(Vector3 position) {
        position = transform.InverseTransformPoint(position);
        int x = Mathf.RoundToInt(position.x / (HexMetrics.innerRadius * 2f));
        int z = Mathf.RoundToInt(position.z / (HexMetrics.outerRadius * 1.5f));
        int index = z * width + x;
        if (index >= 0 && index < cells.Length) {
            return cells[index];
        }
        return null;
    }
    
    void CreateCell (int x, int z, int i) {
        // Calculate cell position
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        // Instantiate and initialize the cell
        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

        // Instantiate and initialize the label
        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }
    
}