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

		// Get the central cell of the grid
		HexCell centralCell = GetCentralCell(cells, width, height);
		Debug.Log($"Central Cell Coordinates: {centralCell.coordinates}, Position: {centralCell.transform.localPosition}");
		centralCell.isAlive = true;

		// Color the central cell and its neighbors
		if (centralCell.isAlive) {
			centralCell.color = Color.yellow;
			GetNeighboors(centralCell);
		}

		Debug.Log("Central Cell: " + centralCell.coordinates.ToString());
		hexMesh.GenerateHexMesh(cells); // Re-triangulate the mesh to update colors
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
		GetNeighboors(cell);
		// Re-triangulate the mesh to update colors
		hexMesh.GenerateHexMesh(cells);
	}

	public void GetNeighboors(HexCell cell)
	{
		HexCoordinates coordinates = cell.coordinates;
		HexCoordinates[] directions = {
			new HexCoordinates(1, 0),
			new HexCoordinates(1, -1),
			new HexCoordinates(0, -1),
			new HexCoordinates(-1, 0),
			new HexCoordinates(-1, 1),
			new HexCoordinates(0, 1)
		};

		for (int i = 0; i < 6; i++)
		{
			HexCoordinates newCoordinates = HexCoordinates.Add(coordinates, directions[i]);

			if (IsValidCoordinates(newCoordinates))
			{
				int index = newCoordinates.X + newCoordinates.Z * width + newCoordinates.Z / 2;
				if (index >= 0 && index < cells.Length)
				{
					HexCell newCell = cells[index];
					newCell.color = Color.grey;
					Debug.Log("Neighbor colored grey: " + newCoordinates.ToString());
				}
				else
				{
					Debug.Log("Index out of bounds: " + index);
				}
			}
			else
			{
				Debug.Log("Invalid coordinates: " + newCoordinates.ToString());
			}
		}
		Debug.Log("--------------------");
	}

	private bool IsValidCoordinates(HexCoordinates coordinates)
	{
		int x = coordinates.X;
		int z = coordinates.Z;
		return x >= 0 && x < width && z >= 0 && z < height;
	}
	
	public HexCell GetCentralCell(HexCell[] cells, int width, int height)
	{
		// Calculate the center coordinates
		int centerX = (width - 1) / 2;
		int centerZ = (height - 1) / 2;

		Debug.Log($"Calculated center coordinates: ({centerX}, {centerZ})");

		// Find the cell with the closest coordinates to the center
		foreach (HexCell cell in cells)
		{
			if (cell.coordinates.X == centerX && cell.coordinates.Z == centerZ)
			{
				Debug.Log($"Found central cell at coordinates: {cell.coordinates}");
				return cell;
			}
		}

		// If no exact match is found, return the closest cell
		HexCell closestCell = null;
		float closestDistance = float.MaxValue;
		Vector3 centerPosition = new Vector3(centerX, 0, centerZ);

		foreach (HexCell cell in cells)
		{
			float distance = Vector3.Distance(cell.transform.localPosition, centerPosition);
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestCell = cell;
			}
		}

		Debug.Log($"Closest cell found at coordinates: {closestCell.coordinates} with position: {closestCell.transform.localPosition}");
		return closestCell;
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