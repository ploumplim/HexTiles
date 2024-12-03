using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

    public Color[] colors;
    public HexGrid hexGrid;
    public Color activeColor;

    private bool inputHandled = false;

    void Awake () {
        SelectColor(0);
    }

    void Update () {
        if (
            Input.GetMouseButton(0) &&
            !EventSystem.current.IsPointerOverGameObject() &&
            !inputHandled
        ) {
            HandleInput();
            inputHandled = true;
        }

        if (Input.GetMouseButtonUp(0)) {
            inputHandled = false;
        }
    }

    void HandleInput () {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            hexGrid.ColorCell(hit.point, activeColor);
        }
    }

    public void SelectColor (int index) {
        activeColor = colors[index];
    }
}