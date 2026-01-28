/*****************************************************************************
// File Name : Scoreboard.cs
// Author : Brandon Koederitz
// Creation Date : 1/28/2026
// Last Modified : 1/28/2026
//
// Brief Description : Controls managing the persistent god scoreboard.
*****************************************************************************/
using System;
using System.Linq;
using UnityEngine;

namespace GGL.Scoring
{
    public class Scoreboard : MonoBehaviour
    {
        /// <summary>
        /// Gets an array to use as the scoreboard based on the number of gods there are.
        /// </summary>
        /// <returns></returns>
        public static int[] GetScoreboardArray()
        {
            int godNum = (int)(Enum.GetValues(typeof(GodID)).Cast<GodID>().Max()) + 1;
            int[] returnVal = new int[godNum];
            return returnVal;
        }
    }
}
