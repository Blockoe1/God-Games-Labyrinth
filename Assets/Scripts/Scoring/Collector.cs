/*****************************************************************************
// File Name : GoldCollector.cs
// Author : Brandon Koederitz
// Creation Date : 1/28/2026
// Last Modified : 1/28/2026
//
// Brief Description : Allows a player to collect gold collectables and score points.
*****************************************************************************/
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GGL.Scoring
{
    public class Collector : MonoBehaviour
    {
        [SerializeField] private GodID team;

        private List<Collectable> heldCollectables = new();

        /// <summary>
        /// Check for gold collection when we enter a trigger.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Handles entering a collectable.
            if (collision.gameObject.TryGetComponent(out Collectable collectable))
            {

            }

            // Handles cashing collectables at a GoldCashZone
            if (collision.gameObject.TryGetComponent(out CollectableCashZone cashZone))
            {

            }
        }

        /// <summary>
        /// Causes this champion to drop a all collectables.
        /// </summary>
        public void DropCollectables()
        {
            DropCollectables(heldCollectables.Count);
        }
        /// <summary>
        /// Causes this champion to drop a certain number of collectables.
        /// </summary>
        /// <param name="numToDrop">The number of collectables to drop.</param>
        public void DropCollectables(int numToDrop)
        {

        }
    }
}
