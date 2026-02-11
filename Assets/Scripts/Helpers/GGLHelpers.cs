/*****************************************************************************
// File Name : GGLHelpers.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Set of helper functions specifically for interacting with GodGamesLabyrinth's setup.
*****************************************************************************/
using UnityEngine;

namespace GGL
{
    public static class GGLHelpers
    {
        private const string MAZE_LAYER_NAME = "Maze";

        private static LayerMask mazeMask;

        public static LayerMask MazeMask
        {
            get
            {
                if (mazeMask == 0)
                {
                    mazeMask = LayerMask.GetMask(MAZE_LAYER_NAME);
                }
                return mazeMask;
            }
        }
    }
}
