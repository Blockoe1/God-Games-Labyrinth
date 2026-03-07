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
using static UnityEditor.PlayerSettings;

namespace GGL.Minotaur
{
    public static class Pathfinder
    {
        #region CONSTS
        private static readonly Vector2Int[] DEFAULT_NODE_SUCCESSORS = new Vector2Int[]
        {
            Vector2Int.right,
            Vector2Int.left,
            Vector2Int.up,
            Vector2Int.down
        };
        private const int GRID_BOUNDS = 100;
        #endregion

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

        public static Vector2[] FindPath(Tilemap[] collisionTilemap, Vector2 startingPos, Vector2 endingPos)
        {
            return FindPath(collisionTilemap, PosToTile(collisionTilemap, startingPos), 
                PosToTile(collisionTilemap, endingPos));
        }

        /// <summary>
        /// Finds the most optimal path from one tile to another.
        /// </summary>
        /// <param name="collisionTilemap">The tilemap containing collisions with the pathfinding object.</param>
        /// <param name="startingTile">The starting tile of the path.</param>
        /// <param name="endingTile">The ending tile of the path.</param>
        /// <returns>The sequence of nodes from the starting to the ending tiles.</returns>
        public static Vector2[] FindPath(Tilemap[] collisionTilemap, Vector2Int startingTile, Vector2Int endingTile)
        {
            // Finalizes the path by looping through nodes in reverse order and adding their position to a list.
            Vector2[] FinalizePath(PathNode startNode, PathNode lastNode)
            {
                List<Vector2> result = new List<Vector2>();
                PathNode current = lastNode;
                // The start node should have null set as previous, so once we hit null the path ends.
                while (current != null)
                {
                    result.Add(TileToPos(collisionTilemap, current.tile));
                    current = current.previousNode;
                }
                result.Reverse();
                return result.ToArray();
            }

            List<PathNode> openList = new List<PathNode>();
            List<Vector2Int> closedList = new List<Vector2Int>();

            // Create the first node in the path that has a 0 direction, so all directions are searched.
            PathNode startNode = new PathNode(startingTile, DEFAULT_NODE_SUCCESSORS, startingTile, endingTile);
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                // Get the node that has the lowest cost to evaluate next.
                PathNode currentNode = openList.OrderBy(item => item.F).FirstOrDefault();
                // Mark the current node as closed.
                openList.Remove(currentNode);
                closedList.Add(currentNode.tile);

                //Debug.DrawLine(TileToPos(currentNode.tile), TileToPos(currentNode.tile) + Vector2.up, Color.blue, 100f);

                // If we reached the ending node, then finish the pathfinding and construct our path.
                if (currentNode.tile == endingTile)
                {
                    // Construct path.
                    return FinalizePath(startNode, currentNode);
                }

                #region Jump Point Finders
                // Functions for checking forced neighbors along the horizontal and vertical directions.
                PathNode CheckHorizontal(Vector2Int startingTile, Vector2Int direction)
                {
                    // Calculate the perpendicular vector used for checking forced neighbors.
                    Vector2Int currentTile = startingTile;

                    // Checks for a forced neighbor.
                    bool CheckForcedNeighbor(Vector2Int pVector)
                    {
                        return !CheckObscured(collisionTilemap, currentTile + pVector) && 
                            CheckObscured(collisionTilemap, currentTile - direction + pVector);
                    }

                    while (true)
                    {
                        currentTile = currentTile + direction;

                        // If we found the ending tile, add it as a node to the open list and stop searching.
                        if (currentTile == endingTile)
                        {
                            return new PathNode(currentTile, null, startingTile, endingTile);
                        }

                        // If we hit a filled tile, then stop searching.
                        if (CheckObscured(collisionTilemap, currentTile))
                        {
                            return null;
                        }

                        // Else
                        List<Vector2Int> forcedNeighborDirections = null;
                        if (CheckForcedNeighbor(Vector2Int.up))
                        {
                            forcedNeighborDirections ??= new List<Vector2Int>();
                            forcedNeighborDirections.Add(Vector2Int.up);
                        }
                        if (CheckForcedNeighbor(Vector2Int.down))
                        {
                            forcedNeighborDirections ??= new List<Vector2Int>();
                            forcedNeighborDirections.Add(Vector2Int.down);
                        }

                        // If 1 or more forced neighbors were found, add this as a path node.
                        if (forcedNeighborDirections != null)
                        {
                            forcedNeighborDirections.Add(direction);
                            return new PathNode(currentTile, forcedNeighborDirections.ToArray(),
                                startingTile, endingTile);
                        }
                    }
                }

                // Vertical is the dominant direction, so it also performs horizontal checks for forced neighbors.
                PathNode CheckVertical(Vector2Int startingTile, Vector2Int direction)
                {
                    // Calculate the perpendicular vector used for checking forced neighbors.
                    Vector2Int currentTile = startingTile;

                    while (true)
                    {
                        currentTile = currentTile + direction;

                        // If we found the ending tile, add it as a node to the open list and stop searching.
                        if (currentTile == endingTile)
                        {
                            return new PathNode(currentTile, null, startingTile, endingTile);
                        }

                        // If we hit a filled tile, then stop searching.
                        if (CheckObscured(collisionTilemap, currentTile))
                        {
                            return null;
                        }

                        List<Vector2Int> forcedNeighborDirections = null;
                        if (CheckHorizontal(currentTile, Vector2Int.right) != null)
                        {
                            forcedNeighborDirections ??= new List<Vector2Int>();
                            forcedNeighborDirections.Add(Vector2Int.right);
                        }
                        if (CheckHorizontal(currentTile, Vector2Int.left) != null)
                        {
                            forcedNeighborDirections ??= new List<Vector2Int>();
                            forcedNeighborDirections.Add(Vector2Int.left);
                        }

                        // If 1 or more forced neighbors were found, add this as a path node.
                        if (forcedNeighborDirections != null)
                        {
                            forcedNeighborDirections.Add(direction);
                            return new PathNode(currentTile, forcedNeighborDirections.ToArray(),
                                startingTile, endingTile);
                        }
                    }
                }
                #endregion

                // Perform checks for forced neighbors.
                foreach (Vector2Int successorDirection in currentNode.successors)
                {
                    PathNode node;
                    // Perform checks for forced neighbors.
                    if (Mathf.Abs(successorDirection.x) > Mathf.Abs(successorDirection.y))
                    {
                        node = CheckHorizontal(currentNode.tile, successorDirection);
                    }
                    else
                    {
                        node = CheckVertical(currentNode.tile, successorDirection);   
                    }
                    // If a jump point was found, add it to the open list to be evaluated.
                    if (node != null && !closedList.Contains(node.tile))
                    {
                        node.previousNode = currentNode;
                        openList.Add(node);
                    }
                }
            }

            // If we cannot find a path, return null.
            return null;
        }

        /// <summary>
        /// Checks if a given tile is empty on the collision tilemap.
        /// </summary>
        /// <param name="tile">The tile to check.</param>
        /// <returns>True if obscured, false otherwise.</returns>
        private static bool CheckObscured(Tilemap[] collisionTilemap, Vector2Int tile)
        {
            // If no tilemaps are given, then the point is not obscured.
            if (collisionTilemap == null) { return false; }
            // If the tile is outside the bounds, then it is obscured.
            if (tile.x > GRID_BOUNDS ||
                tile.y > GRID_BOUNDS)
            {
                return true;
            }
            // Check if the tile is not obscured on any collision tilemap.
            foreach(Tilemap tilemap in collisionTilemap)
            {
                if(tilemap.GetTile((Vector3Int)tile) != null)
                {
                    return true;
                }
            }
            return false;
                
        }

        /// <summary>
        /// Gets the tile position of a certain world position.
        /// </summary>
        /// <param name="pos">The world position to get the tile position of.</param>
        /// <returns>The tile position</returns>
        private static Vector2Int PosToTile(Tilemap[] collisionTilemap, Vector2 pos)
        {
            // If there is no tilemap, just convert the vector to a position by rounding.
            if (collisionTilemap == null || collisionTilemap.Length == 0) 
            { 
                return MathHelpers.RoundVectorToInt(pos);
            } 
            return (Vector2Int)collisionTilemap[0].WorldToCell(pos);  
        }

        /// <summary>
        /// Converts a tile position to a position in world space.
        /// </summary>
        /// <param name="tile">The tile to convert.</param>
        /// <returns>The position of the tile in world space.</returns>
        private static Vector2 TileToPos(Tilemap[] collisionTilemap, Vector2Int tile)
        {
            // If there is no tilemap, just convert the vector to a position by rounding.
            if (collisionTilemap == null || collisionTilemap.Length == 0)
            {
                return tile;
            }
            return collisionTilemap[0].LocalToWorld(collisionTilemap[0].CellToLocal((Vector3Int)tile)) +
                collisionTilemap[0].tileAnchor;
        }

        /// <summary>
        /// Draws a path using debug lines.
        /// </summary>
        /// <param name="path"></param>
        public static void DrawPath(Vector2[] path, float duration = 0f)
        {
            for (int i = 0; i < path.Length - 1; i++)
            {
                Debug.DrawLine(path[i], path[i + 1], Color.red, duration);
                Debug.DrawLine(path[i], path[i] + Vector2.up / 2, Color.green, duration);
            }
        }
    }
}
