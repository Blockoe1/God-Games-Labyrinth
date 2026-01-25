using System;
using System.Linq;
using UnityEngine;

namespace GGL
{
    public class Scoreboard
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
