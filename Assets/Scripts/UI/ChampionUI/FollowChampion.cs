/*****************************************************************************
// File Name : FollowChampion.cs
// Author : Brandon Koederitz
// Creation Date : 2/25/2026
// Last Modified : 2/25/2026
//
// Brief Description : Makes a UI object follow the champion's position.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace GGL.UI.ChampionUI
{
    public class FollowChampion : ChampionUIService
    {
        private Transform trackedChampion;
        private bool isFollowing;

        /// <summary>
        /// Sets the tracked champion of this object.
        /// </summary>
        /// <param name="champion"></param>
        public override void Initialize(GodIdentifier champion)
        {
            trackedChampion = champion.transform;
            isFollowing = true;
            StartCoroutine(FollowRoutine());
        }

        /// <summary>
        /// Continually sets this UI object to the champion's posiiton.
        /// </summary>
        /// <remarks>Assumes a camera space canvas.</remarks>
        /// <returns></returns>
        private IEnumerator FollowRoutine()
        {
            while (isFollowing)
            {
                transform.position = trackedChampion.position;
                yield return null;
            }
        }
    }
}
