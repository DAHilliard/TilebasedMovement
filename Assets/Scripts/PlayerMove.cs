using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Make sure that it's the player's turn;
            SelectTile();
        }
    }

    private Tile SelectTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo))
        {
            Tile tileInfo = hitInfo.collider.GetComponent<Tile>();
            if(tileInfo != null && tileInfo.closedTile)
            {
                tileInfo.goalTile = true;
                return tileInfo;
            }
        }
        return null;
    }
}
