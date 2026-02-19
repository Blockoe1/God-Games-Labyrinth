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
using UnityEngine;
using UnityEngine.Events;

namespace GGL.Scoring
{
    [RequireComponent(typeof(GodIdentifier))]
    public class Collector : MonoBehaviour
    {
        [SerializeField] private int goldCapacity;
        [SerializeField] private UnityEvent OnDropEvent;
        private readonly Queue<Collectable> heldCollectables = new();

        public bool DisableCollection { get; set; }

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
            // If a champion is disabled from just being stolen from, they can't recollect their dropped collectables.
            if (!DisableCollection && 
                collision.gameObject.TryGetComponent(out Collectable collectable) && collectable.IsCollectable)
            {
                ForceCollect(collectable);
            }

            // Handles cashing collectables at a GoldCashZone
            if (collision.gameObject.TryGetComponent(out CollectableCashZone cashZone) && cashZone.Team == id.Team)
            {
                cashZone.CashCollectables(heldCollectables);
                heldCollectables.Clear();
            }
        }

        /// <summary>
        /// Forces this collecter collect a collectable
        /// </summary>
        /// <param name="toCollect">The collectable to force collect.</param>
        public void ForceCollect(Collectable toCollect)
        {
            // Only allow collection if the champion's gold capacity hasn't been hit.
            if (goldCapacity <= 0 || heldCollectables.Count < goldCapacity)
            {
                heldCollectables.Enqueue(toCollect);
                toCollect.OnCollected(this);
            }
        }

        /// <summary>
        /// Causes this champion to drop all collectables.
        /// </summary>
        [ContextMenu("Debug: Drop Collectables")] // Debug
        public Collectable[] DropCollectables()
        {
            return DropCollectables(heldCollectables.Count);
        }
        /// <summary>
        /// Drops collectables based on a percentage of the collectables this collector is holding.
        /// </summary>
        /// <param name="proportion">A number clamped between 0-1 for the proportion of collectables to drop.</param>
        /// <returns></returns>
        public Collectable[] DropCollectables(float proportion)
        {
            int numToDrop = Mathf.RoundToInt(heldCollectables.Count * Mathf.Clamp01(proportion));
            return DropCollectables(numToDrop);
        }
        /// <summary>
        /// Causes this champion to drop a certain number of collectables.
        /// </summary>
        /// <param name="numToDrop">The number of collectables to drop.</param>
        public Collectable[] DropCollectables(int numToDrop)
        {
            List<Collectable> droppedCollectables = new List<Collectable>();
            for(int i = 0; i < numToDrop && heldCollectables.Count > 0; i++)
            {
                Collectable dropped = heldCollectables.Dequeue();
                droppedCollectables.Add(dropped);
                dropped.OnDropped(this);
            }

            OnDropEvent?.Invoke();

            return droppedCollectables.ToArray();
        }
    }
}
