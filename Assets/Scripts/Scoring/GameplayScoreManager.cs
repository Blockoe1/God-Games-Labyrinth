/*****************************************************************************
// File Name : GameplayScoreManager.cs
// Author : Brandon Koederitz
// Creation Date : 1/28/2026
// Last Modified : 1/28/2026
//
// Brief Description : Tracks the score of each god during a play session.
*****************************************************************************/
using GGL.Networking;
using NaughtyAttributes;
using UnityEngine;

namespace GGL.Scoring
{
    [RequireComponent(typeof(NetworkMessenger))]
    public class GameplayScoreManager : MonoBehaviour
    {
        private int[] scores;

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] private NetworkMessenger networkMessenger;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            networkMessenger = GetComponent<NetworkMessenger>();
        }
        #endregion

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

        /// <summary>
        /// Has this score manager broadcast the scores for each god this round over the network.
        /// </summary>
        public void BroadcastScores()
        {
            networkMessenger.SendNetMessage(scores);
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
