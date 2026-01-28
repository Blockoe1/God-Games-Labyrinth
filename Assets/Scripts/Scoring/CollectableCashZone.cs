/*****************************************************************************
// File Name : GoldCashZone.cs
// Author : Brandon Koederitz
// Creation Date : 1/28/2026
// Last Modified : 1/28/2026
//
// Brief Description : Allows a player to collect gold collectables and score points.
*****************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGL.Scoring
{
    public class CollectableCashZone : MonoBehaviour
    {
        [SerializeField] private GodID team;
        [SerializeField] private UnityEvent<int, GodID> OnChampionScore;

        #region Properties
        public GodID Team => team;
        #endregion

        /// <summary>
        /// Cashes a champion's held collectables when they enter this trigger.
        /// </summary>
        /// <param name="collectables">The collectables to cash.</param>
        public void CashCollectables(List<Collectable> collectables)
        {
            int totalScore = 0;
            foreach (Collectable collectable in collectables)
            {
                totalScore += collectable.PointValue;
                collectable.OnCashed();
            }
            OnChampionScore?.Invoke(totalScore, team);
        }
    }
}
