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

	// Array to hold all cells
	HexCell[] cells;

	// Canvas for grid labels
	Canvas gridCanvas;
	// Mesh for hex grid
	HexMesh hexMesh;

	void Awake () {
		// Initialize canvas and mesh
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();

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
		// Triangulate the mesh with the created cells
		hexMesh.GenerateHexMesh(cells);
	}

	public void ColorCell (Vector3 position, Color color) {
		// Convert position to local coordinates
		position = transform.InverseTransformPoint(position);
		// Get hex coordinates from position
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		// Calculate cell index
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		// Get the cell and change its color
		HexCell cell = cells[index];
		cell.color = color;
		// Re-triangulate the mesh to update colors
		hexMesh.GenerateHexMesh(cells);
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
		cell.color = defaultColor;

		// Instantiate and initialize the label
		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
	}
}