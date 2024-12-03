using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour {

    Mesh hexMesh;
    public List<Vector3> vertices;
    public List<Color> colors;
    public List<int> triangles;

    MeshCollider meshCollider;

    void Awake () {
        // Initialize the mesh and mesh collider
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        hexMesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        colors = new List<Color>();
        triangles = new List<int>();
    }

    public void GenerateHexMesh (HexCell[] cells) {
        // Clear previous mesh data
        hexMesh.Clear();
        vertices.Clear();
        colors.Clear();
        triangles.Clear();
        // Triangulate each cell
        for (int i = 0; i < cells.Length; i++) {
            Triangulate(cells[i]);
        }
        // Assign the new mesh data
        hexMesh.vertices = vertices.ToArray();
        hexMesh.colors = colors.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.RecalculateNormals();
        meshCollider.sharedMesh = hexMesh;
    }

    void Triangulate (HexCell cell) {
        // Get the center position of the cell
        Vector3 center = cell.transform.localPosition;
        // Create triangles for each corner of the hexagon
        for (int i = 0; i < 6; i++) {
            AddTriangle(
                center,
                center + HexMetrics.corners[i],
                center + HexMetrics.corners[i + 1]
            );
            AddTriangleColor(cell.color);
        }
    }

    void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3) {
        // Add vertices and triangles to the mesh
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex); triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }

    void AddTriangleColor (Color color) {
        // Add color to the triangle vertices
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }
}