using UnityEngine;

public class HexagonTile : MonoBehaviour
{
    [SerializeField] private GameObject mesh;
    [SerializeField] private GameObject meshTransparent;
    
    public HexagonGrid parentGrid;

    private void OnMouseEnter()
    {
        mesh.SetActive(true);
        meshTransparent.SetActive(false);
    }

    private void OnMouseExit()
    {
        mesh.SetActive(false);
        meshTransparent.SetActive(true);
    }
}
