using System;
using System.Collections.Generic;
using UnityEngine;

public class HexagonGrid : MonoBehaviour
{
    [SerializeField] private int gridWidth = 60;
    [SerializeField] private int gridHeight = 60;
    [SerializeField] private float tileScale = 1f;
    [SerializeField] private GameObject hexagonTilePrefab;
    
    [SerializeField] private GameObject[,] tileInstances;

    private void Awake()
    {
        tileInstances = new GameObject[gridWidth, gridHeight];
    }

    private void Start()
    {
        for (int y = 0; y < gridWidth; y++)
        {
            for (int x = 0; x < gridHeight; x++)
            {
                float xPosition = y % 2 == 0 ? x * tileScale : x * tileScale + tileScale / 2.0f;
                float yPosition = y * 0.9f * tileScale;
                Vector3 position = new Vector3(xPosition, 0.0f, yPosition);
                Vector3 scale = Vector3.one * tileScale;
                
                tileInstances[x, y] = Instantiate(hexagonTilePrefab, position, Quaternion.identity, transform);
                tileInstances[x, y].transform.localScale = scale;
                
                HexagonTile tile = tileInstances[x, y].GetComponent<HexagonTile>();
                tile.parentGrid = this;
            }
        }
    }
    
    
}
