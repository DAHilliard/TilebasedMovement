using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3 tileLocation;
    public bool unwalkable;
    public bool startTile;
    public bool goalTile;
    public bool pathTile;
    public bool closedTile; // optional bool to show the tiles checked during pathfinding

    public Tile parentTile;
    public float GCost; // The distance betw start and current
    public float HCost; // The distance betw current and goal tile
    public float FCost; // GCost + HCost

    private Renderer _rend;
    private Color _unwalkColor = Color.red;
    private Color _startColor = Color.green;
    private Color _goalColor;
    private Color _pathColor = Color.cyan;
    private Color _closedColor = Color.blue; // The tiles that can be chosen during BFS
    private Color _default;

    private void Start()
    {
        if(this.GetComponent<Renderer>() != null)
        {
            _rend = this.GetComponent<Renderer>();
            _default = _rend.material.color;
        }

        _goalColor = _startColor;
        tileLocation = transform.position;
    }

    private void Update()
    {
        ChangeTileColor();
    }

    private void ChangeTileColor()
    {
        if (unwalkable)
        {
            _rend.material.color = _unwalkColor;
        }
        else if (startTile)
        {
            _rend.material.color = _startColor;
        }
        else if (goalTile)
        {
            _rend.material.color = _goalColor;
        }
        else if (pathTile)
        {
            _rend.material.color = _pathColor;
        }
        else if (closedTile)
        {
            _rend.material.color = _closedColor;
        }
        else
        {
            _rend.material.color = _default;
        }
    }

}
