/*****************************************************************************
// File Name : AutoMoveCollision.cs
// Author : Brandon Koederitz
// Creation Date : 2/24/2026
// Last Modified : 2/24/2026
//
// Brief Description : Automatically populates this tilemap with a square collision tile based on a reference tilemap.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GGL.Maze
{
    public class AutoMoveCollision : MonoBehaviour
    {
        [SerializeField] private Tilemap referenceTilemap;
        [SerializeField] private Tilemap targetTilemap;
        [SerializeField] private TileBase fillTile;

        /// <summary>
        /// Fills the target tilemap with fillTiles based on the reference tilemap.
        /// </summary>
        [Button]
        public void BakeTilemap()
        {
            BoundsInt bounds = referenceTilemap.cellBounds;
            for (int i = bounds.yMin; i <= bounds.yMax; i++)
            {
                for(int j = bounds.xMin; j <= bounds.xMax; j++)
                {
                    // Skip empty tiles.
                    if (referenceTilemap.GetTile(new Vector3Int(j, i)) == null) { continue; }
                    targetTilemap.SetTile(new Vector3Int(j, i), fillTile);
                }
            }
        }
    }
}
