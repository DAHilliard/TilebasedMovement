using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //Tile status bools
    #region
    private delegate void ChangeStatus();
    private event ChangeStatus OnChangeStatus;

    private bool _unwalkable;
    private bool _startTile;
    private bool _goalTile;
    private bool _pathTile;
    private bool _closedTile;

    public Vector3 tileLocation;
    public bool Unwalkable
    {
        get { return _unwalkable; }
        set
        {
            _unwalkable = value;
            //Property changed event
            OnChangeStatus?.Invoke();
        }
    }
    public bool StartTile
    {
        get { return _startTile; }
        set
        {
            _startTile = value;
            OnChangeStatus?.Invoke();
        }
    }
    public bool GoalTile
    {
        get { return _goalTile; }
        set
        {
            _goalTile = value;
            OnChangeStatus?.Invoke();
        }
    }
    public bool PathTile
    {
        get { return _pathTile; }
        set
        {
            _pathTile = value;
            OnChangeStatus?.Invoke();
        }
    }
    public bool ClosedTile
    {
        get { return _closedTile; }
        set
        {
            _closedTile = value;
            OnChangeStatus?.Invoke();
        }
    }
    #endregion

    //AStar reqs
    #region
    public Tile parentTile;
    public float GCost; // The distance betw start and current
    public float HCost; // The distance betw current and goal tile
    public float FCost; // GCost + HCost
    #endregion

    //Colorations
    #region 
    private Renderer _rend;
    private Color _unwalkColor = Color.red;
    private Color _startColor = Color.green;
    private Color _goalColor;
    private Color _pathColor = Color.cyan;
    private Color _closedColor = Color.blue; // The tiles that can be chosen during BFS
    private Color _default;
    #endregion

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (this.GetComponent<Renderer>() != null)
        {
            _rend = this.GetComponent<Renderer>();
            _default = _rend.material.color;
        }

        _goalColor = _startColor;
        tileLocation = transform.position;

        OnChangeStatus += ChangeTileColor;
    }
    private void ChangeTileColor()
    {
        if (Unwalkable)
        {
            _rend.material.color = _unwalkColor;
        }
        else if (StartTile)
        {
            _rend.material.color = _startColor;
            Debug.Log("Changing color.");
        }
        else if (GoalTile)
        {
            _rend.material.color = _goalColor;
        }
        else if (PathTile)
        {
            _rend.material.color = _pathColor;
        }
        else if (ClosedTile)
        {
            _rend.material.color = _closedColor;
        }
        else
        {
            _rend.material.color = _default;
        }
    }

    private void OnDisable()
    {
        OnChangeStatus -= ChangeTileColor;
    }
}
