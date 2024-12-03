using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
        private bool wasMouseButtonDown = false;

        void Update () {
                bool isMouseButtonDown = Input.GetMouseButton(0);

                if (isMouseButtonDown && !wasMouseButtonDown)
                {
                        HandleInput();
                }

                wasMouseButtonDown = isMouseButtonDown;
        }

        void HandleInput () {
                Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(inputRay, out hit)) {
                        HexagonTile tile = hit.collider.GetComponent<HexagonTile>();
                        if (tile != null) {
                                Debug.Log($"Tile at position {tile.transform.position} was clicked");
                        } else {
                                Debug.Log("No tile was clicked");
                        }
                } else {
                        Debug.Log("Raycast did not hit any collider");
                }
        }
        
        
}
