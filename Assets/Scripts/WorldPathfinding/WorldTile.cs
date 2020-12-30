using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile : MonoBehaviour
{
    public bool drawGizmos = true;
    public float gridX, gridZ;
    public float minimizeCubes = 0.8f;

    //Tile prefabs
    public GameObject tilePrefab;
    public Transform tileContainer;

    private float _cubeDiameter = 1.0f;
    private float _cubeRadius;
    private float _findTile = 0.3f; // reps the V3.distance betw player/tile when determining starting tile
    private GameObject _player;

    public readonly List<GameObject> allTiles = new List<GameObject>();

    private void Start()
    {
        _cubeRadius = _cubeDiameter / 2;
        _player = GameObject.Find("Player");

        CreateWorld();
    }

    private void Update()
    {
        FindStarting();

    }

    //Creates a Gizmos representation of the world
    /* private void CreateWorld()
     {
         for(int i = 0; i < gridX - 1; i++)
         {
             for(int k = 0; k < gridZ - 1; k++)
             {
                 Tile newTile = new Tile(new Vector3(i - gridX / 2 + _cubeDiameter, transform.position.y, k - gridZ / 2 + _cubeDiameter));
                 allTiles.Add(newTile);
             }
         }
     }
 */
    private void CreateWorld()
    {

        for (int i = 0; i < gridX - 1; i++)
        {
            for (int k = 0; k < gridZ - 1; k++)
            {
                Vector3 newTileLocation = new Vector3(i - gridX / 2 + _cubeDiameter, transform.position.y, k - gridZ / 2 + _cubeDiameter);
                GameObject newTile = Instantiate(tilePrefab, newTileLocation, Quaternion.identity, tileContainer);
                allTiles.Add(newTile);
            }
        }
    }

    private void FindStarting()
    {
        foreach (GameObject tile in allTiles)
        {
            Tile current = tile.GetComponent<Tile>();

            if (current != null)
            {
                if (Vector3.Distance(_player.transform.position, current.tileLocation + new Vector3(0, _cubeRadius, 0)) < _findTile)
                {
                    current.startTile = true;
                }
                else
                {
                    current.startTile = false;
                }
            }

        }
    }

    //Shows path locations
    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, new Vector3(gridX, transform.position.y, gridZ));

            /*foreach (Tile tile in allTiles)
            {
                if (tile.pathTile)
                {
                    Gizmos.color = Color.cyan;
                }
                else if (tile.closedTile)
                {
                    Gizmos.color = Color.blue;
                }
                else if (tile.unwalkable)
                {
                    Gizmos.color = Color.red;
                }
                else if (tile.goalTile)
                {
                    Gizmos.color = Color.green;
                }
                else if (tile.startTile)
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.white;
                }

                Gizmos.DrawCube(tile.tileLocation, Vector3.one * minimizeCubes);
            }*/
        }
    }
}
