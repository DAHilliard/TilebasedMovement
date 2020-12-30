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
                    BFSAlgorithm(startTile.gameObject, iterations);
                }
            }
        }

        private Tile FindStarting(List<GameObject> world)
        {
            foreach (GameObject tile in world)
            {
                Tile tileScript = tile.GetComponent<Tile>();
                if (tileScript.StartTile)
                {
                    return tileScript;
                }
            }
            return null;
        }

        private void ResetClosedTiles(List<GameObject> world)
        {
            foreach (GameObject tile in world)
            {
                Tile tileScript = tile.GetComponent<Tile>();
                if (tileScript.ClosedTile)
                {
                    tileScript.ClosedTile = false;
                }
            }
        }

        private List<GameObject> BFSAlgorithm(GameObject startTile, int _iterations)
        {
            ResetClosedTiles(worldTile.allTiles);
            List<GameObject> returningList = new List<GameObject>();
            List<GameObject> neighborList = new List<GameObject>();
            int tempIterations = _iterations;

            returningList.Add(startTile);
            while (tempIterations > 0)
            {
                for (int i = 0; i < returningList.Count; i++)
                {
                    Tile currentScript = returningList[i].GetComponent<Tile>();

                    if (!currentScript.ClosedTile)
                    {
                        // Add neighbors of the returning list to the neighbor list to be evaluated later
                        Pathfinding.Algorithms.AStar.AddNeighbors(worldTile.allTiles, neighborList, currentScript.gameObject);
                        currentScript.ClosedTile = true;
                    }
                }

                while (neighborList.Count > 0)
                {
                    Tile nextScript = neighborList.First().GetComponent<Tile>();
                    neighborList.RemoveAt(0);
                    returningList.Add(nextScript.gameObject);
                }
                tempIterations--;
            }
            return returningList;
        }

    }
}
