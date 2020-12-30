using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding.Algorithms
{

    public class AStar : MonoBehaviour
    {
        public Stack<GameObject> totalPath = new Stack<GameObject>();

        private WorldTile _worldTile;

        private void Start()
        {
            _worldTile = GetComponent<WorldTile>();
        }

        //Builds the path
        private Stack<GameObject> ReconstructPath(GameObject goalTile)
        {
            totalPath.Clear();
            totalPath.Push(goalTile);
            Tile current = goalTile.GetComponent<Tile>();

            if (current != null)
            {
                while (current.parentTile != null)
                {
                    current.pathTile = true;
                    current = current.parentTile;
                    totalPath.Push(current.gameObject);
                }
            }

            return totalPath;
        }

        //AStar algorithm
        private void FindGoal(GameObject startTile, GameObject goalTile)
        {
            List<GameObject> openSet = new List<GameObject>();
            List<GameObject> neighbors = new List<GameObject>();
            SetDefaultCosts(_worldTile.allTiles, startTile, goalTile);

            openSet.Add(startTile);
            while (openSet.Count > 0)
            {
                //Determine the tile in the open set with the lowest F Cost
                GameObject currentTile = openSet.ElementAt(0);
                Tile tileScript = currentTile.GetComponent<Tile>();
                neighbors.Clear();

                for (int i = 0; i < openSet.Count; i++)
                {
                    Tile nextTile = openSet[i].GetComponent<Tile>();
                    nextTile.FCost = FCost(nextTile.gameObject, startTile, goalTile);
                    if (nextTile.FCost < tileScript.FCost)
                    {
                        currentTile = nextTile.gameObject;
                    }
                }

                //  Found the goal, ReconstructPath();
                if (currentTile == goalTile)
                {
                    Debug.Log("We found the path. Reconstruct path.");
                    ReconstructPath(goalTile);
                    return;
                }

                //Check neighbor of the current node
                openSet.Remove(currentTile);
                tileScript.closedTile = true; //  Optional
                tileScript.GCost = GCost(startTile, currentTile);
                AddNeighbors(_worldTile.allTiles, neighbors, currentTile);

                for (int k = 0; k < neighbors.Count; k++)
                {
                    float testGCosts = tileScript.GCost + GCost(neighbors[k], currentTile);
                    Tile neighborScript = neighbors[k].GetComponent<Tile>();
                    if (testGCosts < neighborScript.GCost)
                    {
                        neighborScript.parentTile = tileScript.parentTile;
                        neighborScript.GCost = testGCosts;
                        neighborScript.FCost = neighborScript.GCost + HCost(goalTile, neighbors[k]);

                        if (!openSet.Contains(neighbors[k]))
                        {
                            openSet.Add(neighbors[k]);
                        }
                    }
                }
            }
            Debug.Log("Did not find a path from the start tile to the goal tile.");
            return;
        }

        //Find neighbors
        #region
        public static void AddNeighbors(List<GameObject> _worldTiles, List<GameObject> neighborList, GameObject currentTile)
        {
            AddUp(_worldTiles, neighborList, currentTile);
            AddRight(_worldTiles, neighborList, currentTile);
            AddDown(_worldTiles, neighborList, currentTile);
            AddLeft(_worldTiles, neighborList, currentTile);
        }
        private static void AddUp(List<GameObject> _worldTiles, List<GameObject> neighborList, GameObject currentTile)
        {
            Tile tileScript = currentTile.GetComponent<Tile>();
            foreach (GameObject tile in _worldTiles)
            {
                Tile nextTile = tile.GetComponent<Tile>();
                if (nextTile.tileLocation.x == tileScript.tileLocation.x + 1 &&
                    nextTile.tileLocation.z == tileScript.tileLocation.z)
                {
                    if (tile != null && !nextTile.unwalkable)
                    {
                        neighborList.Add(tile);
                    }
                }
            }
        }
        private static void AddRight(List<GameObject> _worldTiles, List<GameObject> neighborList, GameObject currentTile)
        {
            Tile tileScript = currentTile.GetComponent<Tile>();
            foreach (GameObject tile in _worldTiles)
            {
                Tile nextTile = tile.GetComponent<Tile>();
                if (nextTile.tileLocation.x == tileScript.tileLocation.x &&
                    nextTile.tileLocation.z == tileScript.tileLocation.z + 1)
                {
                    if (tile != null && !nextTile.unwalkable)
                    {
                        neighborList.Add(tile);
                    }
                }
            }
        }
        private static void AddDown(List<GameObject> _worldTiles, List<GameObject> neighborList, GameObject currentTile)
        {
            Tile tileScript = currentTile.GetComponent<Tile>();
            foreach (GameObject tile in _worldTiles)
            {
                Tile nextTile = tile.GetComponent<Tile>();
                if (nextTile.tileLocation.x == tileScript.tileLocation.x - 1 &&
                    nextTile.tileLocation.z == tileScript.tileLocation.z)
                {
                    if (tile != null && !nextTile.unwalkable)
                    {
                        neighborList.Add(tile);
                    }
                }
            }
        }
        private static void AddLeft(List<GameObject> _worldTiles, List<GameObject> neighborList, GameObject currentTile)
        {
            Tile tileScript = currentTile.GetComponent<Tile>();
            foreach (GameObject tile in _worldTiles)
            {
                Tile nextTile = tile.GetComponent<Tile>();
                if (nextTile.tileLocation.x == tileScript.tileLocation.x &&
                    nextTile.tileLocation.z == tileScript.tileLocation.z - 1)
                {
                    if (tile != null && !nextTile.unwalkable)
                    {
                        neighborList.Add(tile);
                    }
                }
            }
        }
        #endregion

        //Set Cost values
        #region
        private float GCost(GameObject startTile, GameObject currentTile)
        {
            Tile startScript = startTile.GetComponent<Tile>();
            Tile currentScript = currentTile.GetComponent<Tile>();

            //Manhattan distance
            float manhattanD = Mathf.Abs(startScript.tileLocation.x - currentScript.tileLocation.x)
                + Mathf.Abs(startScript.tileLocation.z - currentScript.tileLocation.z);
            return manhattanD;
        }
        private float HCost(GameObject goalTile, GameObject currentTile)
        {
            Tile goalScript = goalTile.GetComponent<Tile>();
            Tile currentScript = currentTile.GetComponent<Tile>();

            //Manhattan distance
            float manhattanD = Mathf.Abs(goalScript.tileLocation.x - currentScript.tileLocation.x)
                + Mathf.Abs(goalScript.tileLocation.z - currentScript.tileLocation.z);
            return manhattanD;
        }
        private float FCost(GameObject startTile, GameObject goalTile, GameObject currentTile)
        {
            float cost = GCost(startTile, currentTile) + HCost(goalTile, currentTile);
            return cost;
        }
        private void SetDefaultCosts(List<GameObject> allTiles, GameObject startTile, GameObject goalTile)
        {
            foreach (GameObject tile in _worldTile.allTiles)
            {
                Tile tileScript = tile.GetComponent<Tile>();

                if (tileScript.startTile)
                {
                    tileScript.GCost = 0;
                    tileScript.FCost = HCost(goalTile, tile);
                    continue;
                }
                else
                {
                    tileScript.GCost = Mathf.Infinity;
                    tileScript.FCost = Mathf.Infinity;
                }
            }
        }
        #endregion
    }
}

