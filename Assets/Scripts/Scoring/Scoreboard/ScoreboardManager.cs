/*****************************************************************************
// File Name : ScoreboardManager.cs
// Author : Brandon Koederitz
// Creation Date : 1/28/2026
// Last Modified : 1/28/2026
//
// Brief Description : Controls managing the persistent god scoreboard.
*****************************************************************************/
using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace GGL.Scoring
{
    public class ScoreboardManager : MonoBehaviour
    {
        #region CONSTS
        private const string FILE_NAME = "ggl_scoreboard.json";
        #endregion

        public static event Action<int[]> OnScoreboardUpdate;

        private static int[] scoreboard;

        #region Nested
        [System.Serializable]
        private class ScoreboardWrapper
        {
            [SerializeField] private int[] scoreboard;

            internal int[] Scoreboard => scoreboard;

            internal ScoreboardWrapper(int[] scoreboard)
            {
                this.scoreboard = scoreboard;
            }
        }
        #endregion

        /// <summary>
        /// Load scores in start once event subscriptions are handled in awake.
        /// </summary>
        private void Start()
        {
            scoreboard = LoadScores();
            OnScoreboardUpdate?.Invoke(scoreboard);
        }

        /// <summary>
        /// Logs scores from a completed game.
        /// </summary>
        /// <param name="scores">The scores to log.</param>
        public void LogScores(int[] scores)
        {
            for(int i = 0; i < scores.Length; i++)
            {
                scoreboard[i] += scores[i];
            }
            OnScoreboardUpdate?.Invoke(scoreboard);
            SaveScores(scoreboard);
        }

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

        #region Score Saving/Loading
        /// <summary>
        /// Saves an array of god scores to a file in StreamingAssets.
        /// </summary>
        /// <param name="scores">The scores to save</param>
        private static void SaveScores(int[] scores)
        {
            // Saves the scores arrat as a JSON file.
            string path = Path.Combine(Application.streamingAssetsPath, FILE_NAME);
            string jsonData = JsonUtility.ToJson(new ScoreboardWrapper(scores));
            File.WriteAllText(path, jsonData);
        }
        /// <summary>
        /// Loads scores from the saved leaderboard file.
        /// </summary>
        /// <returns></returns>
        private static int[] LoadScores()
        {
            string path = Path.Combine(Application.streamingAssetsPath, FILE_NAME);
            if (File.Exists(path))
            {
                string jsonData = File.ReadAllText(path);
                ScoreboardWrapper wrapper = JsonUtility.FromJson<ScoreboardWrapper>(jsonData);
                int[] scoreboard;
                if (wrapper == null || wrapper.Scoreboard == null || wrapper.Scoreboard.Length == 0)
                {
                    scoreboard = GetScoreboardArray();
                    Debug.Log("Failed to load high scores from " + path + ".  No JSON data was detected.");
                }
                else
                {
                    scoreboard = wrapper.Scoreboard;
                }
                return scoreboard;
            }
            else
            {
                Debug.Log("Failed to load high scores from " + path + ".  No file exists at the specified path.");
                return GetScoreboardArray();
            }
        }
        #endregion
    }
}
