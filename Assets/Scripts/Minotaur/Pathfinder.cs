/*****************************************************************************
// File Name : Pathfinder.cs
// Author : Brandon Koederitz
// Creation Date : 2/15/2026
// Last Modified : 2/15/2026
//
// Brief Description : Finds a path along the grid from one space to another using JPA.
*****************************************************************************/
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GGL.Minotaur
{
    public class Pathfinder : MonoBehaviour
    {
        [SerializeField, Tooltip("The tilemap to use as wall based tiles.")] private Tilemap collisionTilemap;
        [SerializeField] private Transform debugPosition;

        #region Nested
        /// <summary>
        /// Represnets a node along the path that should be evaluated.
        /// </summary>
        private class PathNode
        {
            internal Vector2Int tile;
            internal int g;
            internal int h;

            internal int F
            {
                get
                {
                    return g + h;
                }
            }

            internal PathNode(Vector2Int tile)
            {
                this.tile = tile;
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
            throw new System.NotImplementedException();
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
