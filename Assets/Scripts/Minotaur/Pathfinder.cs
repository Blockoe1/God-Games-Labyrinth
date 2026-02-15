/*****************************************************************************
// File Name : Pathfinder.cs
// Author : Brandon Koederitz
// Creation Date : 2/15/2026
// Last Modified : 2/15/2026
//
// Brief Description : Finds a path along the grid from one space to another using JPA.
*****************************************************************************/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GGL.Minotaur
{
    public class Pathfinder : MonoBehaviour
    {
        #region CONSTS
        private static readonly Vector2Int[] DEFAULT_NODE_SUCCESSORS = new Vector2Int[]
        {
            Vector2Int.right,
            Vector2Int.left,
            Vector2Int.up,
            Vector2Int.down,
        };
        #endregion

        [SerializeField, Tooltip("The tilemap to use as wall based tiles.")] private Tilemap collisionTilemap;
        [SerializeField] private Transform debugPosition;

        #region Nested
        /// <summary>
        /// Represnets a node along the path that should be evaluated.
        /// </summary>
        private class PathNode
        {
            internal Vector2Int tile;
            internal Vector2Int[] successors;
            internal int g;
            internal int h;

            internal PathNode previousNode;

            internal int F
            {
                get
                {
                    return g + h;
                }
            }

            internal PathNode(Vector2Int tile, Vector2Int[] successors, Vector2Int start, Vector2Int end)
            {
                this.tile = tile;
                this.successors = successors;
                CalculateMDistance(start, end); 
            }

            /// <summary>
            /// Calculates the weight of this node based on it's manhatten distance from the start and end tiles.
            /// </summary>
            /// <param name="start">The starting tile of the path</param>
            /// <param name="end">The end tile of the path</param>
            internal void CalculateMDistance(Vector2Int start, Vector2Int end)
            {
                g = MathHelpers.FindManhattenDistance(start, tile);
                h = MathHelpers.FindManhattenDistance(tile, end);
            }
        }
        #endregion

        public Vector2[] FindPath(Vector2 endingPos)
        {
            return FindPath(PosToTile(endingPos));
        }
        public Vector2[] FindPath(Vector2Int endingTile)
        {
            return FindPath(PosToTile(transform.position), endingTile);
        }
        /// <summary>
        /// Finds the most optimal path from one tile to another.
        /// </summary>
        /// <param name="startingTile">The starting tile of the path.</param>
        /// <param name="endingTile">The ending tile of the path.</param>
        /// <returns>The sequence of nodes from the starting to the ending tiles.</returns>
        public Vector2[] FindPath(Vector2Int startingTile, Vector2Int endingTile)
        {
            // Finalizes the path by looping through nodes in reverse order and adding their position to a list.
            Vector2[] FinalizePath(PathNode startNode, PathNode lastNode)
            {
                List<Vector2> result = new List<Vector2>();
                PathNode current = lastNode;
                while (current != startNode)
                {
                    result.Add(TileToPos(current.tile));
                    current = current.previousNode;
                }
                result.Reverse();
                return result.ToArray();
            }

            List<PathNode> openList = new List<PathNode>();
            List<PathNode> closedList = new List<PathNode>();

            // Create the first node in the path that has a 0 direction, so all directions are searched.
            PathNode startNode = new PathNode(startingTile, DEFAULT_NODE_SUCCESSORS, startingTile, endingTile);
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                // Get the node that has the lowest cost to evaluate next.
                PathNode currentNode = openList.OrderBy(item => item.F).FirstOrDefault();
                // Mark the current node as closed.
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                // If we reached the ending node, then finish the pathfinding and construct our path.
                if (currentNode.tile == endingTile)
                {
                    // Construct path.
                    return FinalizePath(startNode, currentNode);
                }

                // Functions for checking forced neighbors along the horizontal and vertical directions.  Vertical
                // is the dominant direction, so it has to check for horizontals as well.
                bool CheckHorizontal(Vector2Int startingTile, Vector2Int direction)
                {

                }
                bool CheckVertical(Vector2Int startingTile, Vector2Int direction) // Dominant direction check.
                {

                }

                // Perform checks for forced neighbors.
                foreach(Vector2Int successorDirection in currentNode.successors)
                {
                    // If the successor direction is vertical, perform additional checks.
                    if (successorDirection.y > 0)
                    {

                    }
                }
            }

            // If we cannot find a path, return null.
            return null;
        }



        /// <summary>
        /// Gets the tile position of a certain world position.
        /// </summary>
        /// <param name="pos">The world position to get the tile position of.</param>
        /// <returns>The tile position</returns>
        private Vector2Int PosToTile(Vector2 pos)
        {
            return (Vector2Int)collisionTilemap.WorldToCell(pos);  
        }

        /// <summary>
        /// Converts a tile position to a position in world space.
        /// </summary>
        /// <param name="tile">The tile to convert.</param>
        /// <returns>The position of the tile in world space.</returns>
        private Vector2 TileToPos(Vector2Int tile)
        {
            return collisionTilemap.
        }

        #region Debug
        [ContextMenu("Debug Path")]
        public void DebugPath()
        {
            Vector2[] path = FindPath(debugPosition.position);
            for (int i = 0; i < path.Length - 1; i++)
            {
                Debug.DrawLine(path[i], path[i + 1], Color.red, 5f);
            }
        }
        #endregion
    }
}
