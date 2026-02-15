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

        /// <summary>
        /// Gets the tile position of a certain world position.
        /// </summary>
        /// <param name="pos">The world position to get the tile position of.</param>
        /// <returns>The tile position</returns>
        private Vector3Int PosToTile(Vector2 pos)
        {
            return collisionTilemap.WorldToCell(pos);  
        }
    }
}
