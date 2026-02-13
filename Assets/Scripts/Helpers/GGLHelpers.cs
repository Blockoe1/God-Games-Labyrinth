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
        private const string CHAMPION_LAYER_NAME = "Champion";

        private static LayerMask mazeMask;
        private static LayerMask championMask;

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

        public static LayerMask ChampionMask
        {
            get
            {
                if (championMask == 0)
                {
                    championMask = LayerMask.GetMask(CHAMPION_LAYER_NAME);
                }
                return championMask;
            }
        }
    }
}
