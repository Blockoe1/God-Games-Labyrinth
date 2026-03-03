/*****************************************************************************
// File Name : TimerFill.cs
// Author : Brandon Koederitz
// Creation Date : 3/3/2026
// Last Modified : 3/3/2026
//
// Brief Description : Displays the current value of the timer on the champion UI.
*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;

namespace GGL.UI.ChampionUI
{
    public class TimerDisplay : ChampionUIService
    {
        [SerializeField] private Image fillImage;

        /// <summary>
        /// Subscribe/Unsubscribe event references to update the dispalyed time.
        /// </summary>
        /// <param name="champion"></param>
        public override void Initialize(GodIdentifier champion)
        {
            TimerScript.OnTimerUpdate += UpdateTimer;
        }

        private void OnDestroy()
        {
            TimerScript.OnTimerUpdate -= UpdateTimer;
        }

        /// <summary>
        /// Updates the fill of an image based on the current time.
        /// </summary>
        /// <param name="time">The Current time.</param>
        /// <param name="maxTime">The max time displayed by the timer.</param>
        private void UpdateTimer(float time, float maxTime)
        {
            if (fillImage != null)
            {
                Debug.Log(maxTime == 0 ? 0 : (time / maxTime));
                fillImage.fillAmount = maxTime == 0 ? 0 : (time / maxTime);
            }
        }
    }
}
