/*****************************************************************************
// File Name : ScoreboardTextUpdater.cs
// Author : Brandon Koederitz
// Creation Date : 2/26/2026
// Last Modified : 2/26/2026
//
// Brief Description : Displays a score on the scoreboard as text.
*****************************************************************************/
using GGL.Scoring;
using TMPro;
using UnityEngine;

namespace GGL.UI.Scoreboard
{
    public class ScoreboardTextUpdater : MonoBehaviour
    {
        #region CONSTS
        private const int DISPLAY_DIGITS = 6;
        #endregion

        [SerializeField] private TMP_Text textComponent;
        [SerializeField] private GodID team;

        /// <summary>
        /// Subscribe/Unsubscribe event references.
        /// </summary>
        private void Awake()
        {
            ScoreboardManager.OnScoreboardUpdate += UpdateText;
        }
        private void OnDestroy()
        {
            ScoreboardManager.OnScoreboardUpdate -= UpdateText;
        }

        /// <summary>
        /// Updates the text show on this object based on scoreboard scores.
        /// </summary>
        /// <param name="scoreboard"></param>
        private void UpdateText(int[] scoreboard)
        {
            textComponent.text = UIHelpers.ArcadeFormat(scoreboard[(int)team], DISPLAY_DIGITS);
        }
    }
}
