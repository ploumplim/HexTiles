using System.Collections.Generic;
using UnityEngine;

public class HexagonTile : MonoBehaviour
{
    [SerializeField] private GameObject emptyHex;
    [SerializeField] private GameObject legalHex;
    [SerializeField] private GameObject tileBasic;
    [SerializeField] private GameObject fusion1;
    [SerializeField] private GameObject starter;
    
    [HideInInspector]
    public HexagonGrid parentGrid;
    [Tooltip("The amount of turns this tile has until it self-destructs")]
    public int lifeTime;
    [Tooltip("The value in lifetime when this tile can be fused.")]
    public int fusionOpportunityTime;
    [Tooltip("If the tile has the IMMUNE effect.")]
    public bool immune;
    [Tooltip("if the tile has the RANGE effect.")]
    public bool extraRange;
    [Tooltip("if the tile has the RETURNING effect.")]
    public bool returning;
    
    [HideInInspector]
    // the longevity of the tile, measured in priority counters.
    public int _priorityCounters;
    [HideInInspector]
    // the moment that the tile becomes available for fusion.
    public bool _fusionDispo;
    [HideInInspector]
    //If true, the tile has the starter tile effects.
    public bool _starterTile;
    
 
    public void SetTile(int tileType)
    {
        emptyHex.SetActive(false);
        legalHex.SetActive(false);
        tileBasic.SetActive(false);
        fusion1.SetActive(false);
        starter.SetActive(false);
        
        switch (tileType)
        {
            case 0:
                emptyHex.SetActive(true);
                break;
            case 1:
                legalHex.SetActive(true);
                break;
            case 2:
                tileBasic.SetActive(true);
                SetLegal();
                break;
            case 3:
                fusion1.SetActive(true);
                break;
            case 4:
                starter.SetActive(true);
                SetLegal();
                break;
        }
    }
    
    public bool IsEmpty()
    {
        return emptyHex.activeSelf;
    }
    
    public void SetLegal()
    {
        var adjacentTiles = GetAdjacentTiles();
        foreach (HexagonTile tile in adjacentTiles)
        {
            // Check if the tile is currently empty before changing it to a legal tile
            if (tile.IsEmpty())
            {
                tile.SetTile(1);
            }
        };
    }
    
    public HexagonTile[] GetAdjacentTiles()
    {
        List<HexagonTile> adjacentTiles = new List<HexagonTile>();
        Vector3[] directions = new Vector3[]
        {
            new Vector3(parentGrid.tileScale * Mathf.Sqrt(3), 0, 0),                                // Right
            new Vector3(-parentGrid.tileScale * Mathf.Sqrt(3), 0, 0),                               // Left
            new Vector3(parentGrid.tileScale * Mathf.Sqrt(3) / 2, 0, parentGrid.tileScale * 1.5f),  // Up-Right
            new Vector3(-parentGrid.tileScale * Mathf.Sqrt(3) / 2, 0, parentGrid.tileScale * 1.5f), // Up-Left
            new Vector3(parentGrid.tileScale * Mathf.Sqrt(3) / 2, 0, -parentGrid.tileScale * 1.5f), // Down-Right
            new Vector3(-parentGrid.tileScale * Mathf.Sqrt(3) / 2, 0, -parentGrid.tileScale * 1.5f) // Down-Left
        };

        foreach (Vector3 direction in directions)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, parentGrid.tileScale * 2))
            {
                adjacentTiles.Add(hit.collider.gameObject.GetComponent<HexagonTile>());
            }
        }

        return adjacentTiles.ToArray();
    }
    
}
