/*****************************************************************************
// File Name : HeldGoldCounter.cs
// Author : Brandon Koederitz
// Creation Date : 2/19/2026
// Last Modified : 2/19/2026
//
// Brief Description : Tracks the amount of gold held by a champion.
*****************************************************************************/
using GGL.Scoring;
using TMPro;
using UnityEngine;

namespace GGL.UI.ChampionUI
{
    public class HeldGoldCounter : ChampionUIService
    {
        [SerializeField] private TieredSprite goldSprite;
        //[SerializeField] private TMP_Text pointsText;

        private Collector collector;

        /// <summary>
        /// Get a reference to the collector on the found champion.
        /// </summary>
        /// <param name="champion"></param>
        public override void Initialize(GodIdentifier champion)
        {
            collector = champion.GetComponent<Collector>();
            collector.OnCollectablesChanged += UpdateCollectables;
        }

        /// <summary>
        /// Updates the displayed number and point value of held collectables.
        /// </summary>
        /// <param name="heldCount"></param>
        /// <param name="heldValue"></param>
        private void UpdateCollectables(int heldCount, int heldValue)
        {
            // Update the text to show the total held value.
            //pointsText.text = FormatPoints(heldValue);

            // Update the gold sprite based on the proportion of the champion's held amount and capacity.
            goldSprite.SpriteAmount = collector.GoldCapacity > 0 ? (float)heldCount / collector.GoldCapacity : 0;
        }

        /// <summary>
        /// Cleanup events.
        /// </summary>
        private void OnDestroy()
        {
            collector.OnCollectablesChanged -= UpdateCollectables;
        }
    }
}
