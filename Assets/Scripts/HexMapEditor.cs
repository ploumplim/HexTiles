using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

	// public Color[] colors;

	public HexGrid hexGrid;

	// private Color activeColor;

	void Awake () {
		// SelectColor(0);
	}

	void Update () {
		if (
			Input.GetMouseButton(0) &&
			!EventSystem.current.IsPointerOverGameObject()
		) {
			 HandleInput();
		}
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			HexCell cell = hexGrid.GetCell(hit.point);
			if (cell != null) {
				Debug.Log($"Cell at position: {cell.coordinates}");
			}
		}
	}

	// public void SelectColor (int index) {
	// 	activeColor = colors[index];
	// }
}