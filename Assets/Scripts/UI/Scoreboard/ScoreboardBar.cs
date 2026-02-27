/*****************************************************************************
// File Name : ScoreboardBar.cs
// Author : Brandon Koederitz
// Creation Date : 2/27/2026
// Last Modified : 2/27/2026
//
// Brief Description : Changes the width of a UI element based on the relative amount of score the designated team has.
*****************************************************************************/
using GGL.Scoring;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace GGL.UI.Scoreboard
{
    public class ScoreboardBar : MonoBehaviour
    {
        [SerializeField] private GodID team;
        [SerializeField] private float tweenTime;
        [SerializeField] private float minWidth;
        [SerializeField] private float maxWidth;

        private Coroutine animationCoroutine;

        private RectTransform rTrans => transform as RectTransform;

        /// <summary>
        /// Setup Event References.
        /// </summary>
        private void Awake()
        {
            ScoreboardManager.OnScoreboardUpdate += OnScoreboardUpdate;
        }
        private void OnDestroy()
        {
            ScoreboardManager.OnScoreboardUpdate -= OnScoreboardUpdate;
        }

        /// <summary>
        /// Updates this objects width based on new scores.
        /// </summary>
        /// <param name="scoreboard"></param>
        private void OnScoreboardUpdate(int[] scoreboard)
        {
            int totalScore = 0;
            foreach(int num in scoreboard)
            {
                totalScore += num;
            }
            int avgScore = totalScore / scoreboard.Length;
            float stdDev = MathHelpers.StandardDeviation(avgScore, scoreboard);

            // Calculate the proportion of the total score this team's sscore takes up.
            int score = scoreboard[(int)team];
            float sign = score > avgScore ? 1 : -1;
            Debug.Log(score + " " + avgScore + " " + MathHelpers.NormalDistribution01(score, avgScore, stdDev) + " " + stdDev);
            float normalizedWidth = 0.5f + (sign * (1 - MathHelpers.NormalDistribution01(score, avgScore, stdDev)));

            float width = Mathf.Lerp(minWidth, maxWidth, normalizedWidth);
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
            }
            animationCoroutine = StartCoroutine(ScoreboardAnimateRoutine(width, tweenTime));
        }

        /// <summary>
        /// Animates
        /// </summary>
        /// <param name="targetWidth"></param>
        /// <returns></returns>
        private IEnumerator ScoreboardAnimateRoutine(float targetWidth, float time)
        {
            float step = Mathf.Abs((rTrans.sizeDelta.x - targetWidth) / time);
            while (time > 0)
            {
                Vector2 size = rTrans.sizeDelta;
                size.x = Mathf.MoveTowards(size.x, targetWidth, step * Time.deltaTime);
                rTrans.sizeDelta = size;

                time -= Time.deltaTime;
                yield return null;
            }
            animationCoroutine = null;
        }

    }
}
