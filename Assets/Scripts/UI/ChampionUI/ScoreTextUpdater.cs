/*****************************************************************************
// File Name : GameplayScoreManager.cs
// Author : Brandon Koederitz
// Creation Date : 1/28/2026
// Last Modified : 1/28/2026
//
// Brief Description : Tracks the score of each god during a play session.
*****************************************************************************/
using GGL.Scoring;
using TMPro;
using UnityEngine;

namespace GGL.UI
{
    public class ScoreTextUpdater : MonoBehaviour
    {
        #region CONST
        private const int SCORE_DISPLAY_DIGITS = 4;
        #endregion

        [SerializeField] private TMP_Text textComponent;
        [SerializeField] private GodID god;

        /// <summary>
        /// Subscribe/Unsubscribe to the score manager's OnScoreChanged function so this text updates whenever the
        /// corresponding god's score is upadated.
        /// </summary>
        private void Awake()
        {
            GameplayScoreManager.OnScoreUpdate += UpdateScoreText;
        }
        private void OnDestroy()
        {
            GameplayScoreManager.OnScoreUpdate -= UpdateScoreText;
        }

        /// <summary>
        /// Updates the score displayed by this text object
        /// </summary>
        /// <param name="score">The current score to display.</param>
        /// <param name="updatedGod">The GodID of the score that changed.</param>
        private void UpdateScoreText(int score, GodID updatedGod)
        {
            if(updatedGod == god)
            {
                textComponent.text = FormatPoints(score);
            }
        }

        /// <summary>
        /// Converts an iteger score into a string with zeros appended to the beginning to fill a certain 
        /// number of digits.
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public static string FormatPoints(int score)
        {
            return UIHelpers.ArcadeFormat(score, SCORE_DISPLAY_DIGITS);
        }
    }
}
