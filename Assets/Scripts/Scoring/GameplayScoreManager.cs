/*****************************************************************************
// File Name : GameplayScoreManager.cs
// Author : Brandon Koederitz
// Creation Date : 1/28/2026
// Last Modified : 1/28/2026
//
// Brief Description : Tracks the score of each god during a play session.
*****************************************************************************/
using UnityEngine;

namespace GGL.Scoring
{
    public class GameplayScoreManager : MonoBehaviour
    {
        private int[] scores;

        /// <summary>
        /// Initialize the scores array.
        /// </summary>
        private void Awake()
        {
            scores = Scoreboard.GetScoreboardArray();
        }

        /// <summary>
        /// Increases a god's score.
        /// </summary>
        /// <param name="toAdd">The amount of score to add.</param>
        /// <param name="god">The god to add the score to.</param>
        public void AddScore(int toAdd, GodID god)
        {
            scores[(int)god] += toAdd;
        }

        #region Debug
        private void OnGUI()
        {
            string scoreboard = "";
            for(int i = 0; i < scores.Length; i++)
            {
                scoreboard += (GodID)i + ": " + scores[i] + "\n";
            }
            GUI.Label(new Rect(20, 20, 400, 1000), scoreboard);
        }
        #endregion
    }
}
