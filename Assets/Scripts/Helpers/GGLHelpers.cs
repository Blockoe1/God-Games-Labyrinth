/*****************************************************************************
// File Name : GGLHelpers.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Set of helper functions specifically for interacting with GodGamesLabyrinth's setup.
*****************************************************************************/
using System;
using System.Collections;
using UnityEngine;

namespace GGL
{
    public static class GGLHelpers
    {
        private const string MAZE_LAYER_NAME = "Maze";
        private const string CHAMPION_LAYER_NAME = "Champion";
        private const string MOVE_LAYER_NAME = "MoveCheck";
        private const string ROOM_LAYER_NAME = "Room";

        private static LayerMask mazeMask;
        private static LayerMask championMask;
        private static LayerMask moveCheckMask;
        private static LayerMask roomMask;

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
        public static LayerMask MoveCheckMask
        {
            get
            {
                if (moveCheckMask == 0)
                {
                    moveCheckMask = LayerMask.GetMask(MOVE_LAYER_NAME);
                }
                return moveCheckMask;
            }
        }

        public static LayerMask RoomMask
        {
            get
            {
                if (roomMask == 0)
                {
                    roomMask = LayerMask.GetMask(ROOM_LAYER_NAME);
                }
                return roomMask;
            }
        }

        /// <summary>
        /// Animates an integer value from a starting to target value over time.
        /// </summary>
        /// <param name="updater">The update function used to change the value of the integer.</param>
        /// <param name="time">The time the animation should take.</param>
        /// <param name="startValue">The initial starting value.</param>
        /// <param name="targetValue">The target value that the int should animate to.</param>
        /// <returns></returns>
        public static IEnumerator AnimateInt(Action<int> updater, float time, int startValue, int targetValue)
        {
            // If no time is specified, immediatealy update the value.
            if (time <= 0 || targetValue == startValue)
            {
                updater(targetValue);
                yield break;
            }

            int value = startValue;
            float timeStep = time / Mathf.Abs(targetValue - startValue);
            while (value != targetValue)
            {
                value = (int)Mathf.MoveTowards(value, targetValue, 1);
                updater(value);
                yield return new WaitForSeconds(timeStep);
            }
            updater(targetValue);
        }
    }
}
