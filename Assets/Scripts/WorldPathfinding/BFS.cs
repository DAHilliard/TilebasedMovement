using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pathfinding.Algorithms;

/// <summary>
/// BFS pathfinding will just be used to determine the movement "radius" available per character ---ie where you can move
/// AStar pathfinding will handle the actual movement for the character ---ie how you can move
/// </summary>

namespace Pathfinding.Algorithms
{

    public class BFS : MonoBehaviour
    {
        //Use a "walking" bool from the player movement class
        //use a "isTurn" bool/enum (game state) from the player (movement/battle)? class
        public bool temporaryWalking = false;
        public bool temporaryIsTurn = true;
        public int iterations = 3; // The number of steps a character can take

        private GameObject _player;
        private WorldTile worldTile;

        private void Start()
        {
            _player = GameObject.Find("Player");
            worldTile = GetComponent<WorldTile>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Tile startTile = FindStarting(worldTile.allTiles);
                if (startTile != null)
                {
                    BFSAlgorithm(startTile, iterations);
                }
            }
        }

        private Tile FindStarting(List<Tile> world)
        {
            foreach (Tile tile in world)
            {
                if (tile.startTile)
                {
                    return tile;
                }
            }
            return null;
        }

        private void ResetClosedTiles(List<Tile> world)
        {
            foreach (Tile tile in world)
            {
                if (tile.closedTile)
                {
                    tile.closedTile = false;
                }
            }
        }

        private List<Tile> BFSAlgorithm(Tile startTile, int _iterations)
        {
            ResetClosedTiles(worldTile.allTiles);
            List<Tile> returningList = new List<Tile>();
            List<Tile> neighborList = new List<Tile>();
            int tempIterations = _iterations;

            returningList.Add(startTile);
            while (tempIterations > 0)
            {
                for (int i = 0; i < returningList.Count; i++)
                {
                    if (!returningList[i].closedTile)
                    {
                        // Add neighbors of the returning list to the neighbor list to be evaluated later
                        Pathfinding.Algorithms.AStar.AddNeighbors(worldTile.allTiles, neighborList, returningList[i]);
                        returningList[i].closedTile = true;
                    }
                }

                while (neighborList.Count > 0)
                {
                    Tile nextTile = neighborList.First();
                    neighborList.RemoveAt(0);
                    returningList.Add(nextTile);
                }
                tempIterations--;
            }
            return returningList;
        }

    }
}
