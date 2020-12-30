using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3 tileLocation;
    public bool startTile;
    public bool goalTile;
    public bool unwalkable;
    public bool pathTile;
    public bool closedTile; // optional bool to show the tiles checked during pathfinding

    public Tile parentTile;
    public float GCost; // The distance betw start and current
    public float HCost; // The distance betw current and goal tile
    public float FCost; // GCost + HCost

    private void Start()
    {
        tileLocation = transform.position;
    }

}
