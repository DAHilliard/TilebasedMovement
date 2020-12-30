using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding.Algorithms
{

    public class AStar : MonoBehaviour
    {
        public Stack<Tile> totalPath = new Stack<Tile>();

        private WorldTile _worldTile;

        private void Start()
        {
            _worldTile = GetComponent<WorldTile>();
        }

        //Builds the path
        private Stack<Tile> ReconstructPath(Tile goalTile)
        {
            totalPath.Clear();
            totalPath.Push(goalTile);

            while (goalTile.parentTile != null)
            {
                goalTile.pathTile = true;
                goalTile = goalTile.parentTile;
                totalPath.Push(goalTile);
            }
            return totalPath;
        }

        //AStar algorithm
        private void FindGoal(Tile startTile, Tile goalTile)
        {
            List<Tile> openSet = new List<Tile>();
            List<Tile> neighbors = new List<Tile>();
            SetDefaultCosts(_worldTile.allTiles, startTile, goalTile);

            openSet.Add(startTile);
            while (openSet.Count > 0)
            {
                //Determine the tile in the open set with the lowest F Cost
                Tile currentTile = openSet.ElementAt(0);
                neighbors.Clear();

                for (int i = 0; i < openSet.Count; i++)
                {
                    openSet[i].FCost = FCost(openSet[i], startTile, goalTile);
                    if (openSet[i].FCost < currentTile.FCost)
                    {
                        currentTile = openSet[i];
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
                currentTile.closedTile = true; //  Optional
                currentTile.GCost = GCost(startTile, currentTile);
                AddNeighbors(_worldTile.allTiles, neighbors, currentTile);

                for (int k = 0; k < neighbors.Count; k++)
                {
                    float testGCosts = currentTile.GCost + GCost(neighbors[k], currentTile);
                    if (testGCosts < neighbors[k].GCost)
                    {
                        neighbors[k].parentTile = currentTile;
                        neighbors[k].GCost = testGCosts;
                        neighbors[k].FCost = neighbors[k].GCost + HCost(goalTile, neighbors[k]);

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
        public static void AddNeighbors(List<Tile> _worldTiles, List<Tile> neighborList, Tile currentTile)
        {
            AddUp(_worldTiles, neighborList, currentTile);
            AddRight(_worldTiles, neighborList, currentTile);
            AddDown(_worldTiles, neighborList, currentTile);
            AddLeft(_worldTiles, neighborList, currentTile);
        }
        private static void AddUp(List<Tile> _worldTiles, List<Tile> neighborList, Tile currentTile)
        {
            foreach (Tile tile in _worldTiles)
            {
                if (tile.tileLocation.x == currentTile.tileLocation.x + 1 &&
                    tile.tileLocation.z == currentTile.tileLocation.z)
                {
                    if (tile != null && !tile.unwalkable)
                    {
                        neighborList.Add(tile);
                    }
                }
            }
        }
        private static void AddRight(List<Tile> _worldTiles, List<Tile> neighborList, Tile currentTile)
        {
            foreach (Tile tile in _worldTiles)
            {
                if (tile.tileLocation.x == currentTile.tileLocation.x &&
                    tile.tileLocation.z == currentTile.tileLocation.z + 1)
                {
                    if (tile != null && !tile.unwalkable)
                    {
                        neighborList.Add(tile);
                    }
                }
            }
        }
        private static void AddDown(List<Tile> _worldTiles, List<Tile> neighborList, Tile currentTile)
        {
            foreach (Tile tile in _worldTiles)
            {
                if (tile.tileLocation.x == currentTile.tileLocation.x - 1 &&
                    tile.tileLocation.z == currentTile.tileLocation.z)
                {
                    if (tile != null && !tile.unwalkable)
                    {
                        neighborList.Add(tile);
                    }
                }
            }
        }
        private static void AddLeft(List<Tile> _worldTiles, List<Tile> neighborList, Tile currentTile)
        {
            foreach (Tile tile in _worldTiles)
            {
                if (tile.tileLocation.x == currentTile.tileLocation.x &&
                    tile.tileLocation.z == currentTile.tileLocation.z - 1)
                {
                    if (tile != null && !tile.unwalkable)
                    {
                        neighborList.Add(tile);
                    }
                }
            }
        }
        #endregion

        //Set Cost values
        #region
        private float GCost(Tile startTile, Tile currentTile)
        {
            //Manhattan distance
            float manhattanD = Mathf.Abs(startTile.tileLocation.x - currentTile.tileLocation.x)
                + Mathf.Abs(startTile.tileLocation.z - currentTile.tileLocation.z);
            return manhattanD;
        }
        private float HCost(Tile goalTile, Tile currentTile)
        {
            //Manhattan distance
            float manhattanD = Mathf.Abs(goalTile.tileLocation.x - currentTile.tileLocation.x)
                + Mathf.Abs(goalTile.tileLocation.z - currentTile.tileLocation.z);
            return manhattanD;
        }
        private float FCost(Tile startTile, Tile goalTile, Tile currentTile)
        {
            float cost = GCost(startTile, currentTile) + HCost(goalTile, currentTile);
            return cost;
        }
        private void SetDefaultCosts(List<Tile> allTiles, Tile startTile, Tile goalTile)
        {
            foreach (Tile tile in _worldTile.allTiles)
            {
                if (tile.startTile)
                {
                    tile.GCost = 0;
                    tile.FCost = HCost(goalTile, tile);
                    continue;
                }
                else
                {
                    tile.GCost = Mathf.Infinity;
                    tile.FCost = Mathf.Infinity;
                }
            }
        }
        #endregion
    }
}

