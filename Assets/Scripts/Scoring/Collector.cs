/*****************************************************************************
// File Name : GoldCollector.cs
// Author : Brandon Koederitz
// Creation Date : 1/28/2026
// Last Modified : 1/28/2026
//
// Brief Description : Allows a player to collect gold collectables and score points.
*****************************************************************************/
using NaughtyAttributes;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GGL.Scoring
{
    [RequireComponent(typeof(GodIdentifier))]
    public class Collector : MonoBehaviour
    {
        private readonly Queue<Collectable> heldCollectables = new();

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] protected GodIdentifier id;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        protected virtual void Reset()
        {
            id = GetComponent<GodIdentifier>();
        }
        #endregion

        /// <summary>
        /// Check for gold collection when we enter a trigger.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Handles entering a collectable.
            if (collision.gameObject.TryGetComponent(out Collectable collectable) && collectable.CanBeCollected)
            {
                heldCollectables.Enqueue(collectable);
                collectable.OnCollected(this);
            }

            // Handles cashing collectables at a GoldCashZone
            if (collision.gameObject.TryGetComponent(out CollectableCashZone cashZone) && cashZone.Team == id.Team)
            {
                cashZone.CashCollectables(heldCollectables);
                heldCollectables.Clear();
            }
        }

        /// <summary>
        /// Causes this champion to drop all collectables.
        /// </summary>
        [ContextMenu("Debug: Drop Collectables")] // Debug
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
            for(int i = 0; i < numToDrop && heldCollectables.Count > 0; i++)
            {
                heldCollectables.Dequeue().OnDropped(this);
            }
        }
    }
}
