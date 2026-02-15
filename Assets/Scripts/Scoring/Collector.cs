/*****************************************************************************
// File Name : GoldCollector.cs
// Author : Brandon Koederitz
// Creation Date : 1/28/2026
// Last Modified : 1/28/2026
//
// Brief Description : Allows a player to collect gold collectables and score points.
*****************************************************************************/
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace GGL.Scoring
{
    [RequireComponent(typeof(GodIdentifier))]
    public class Collector : MonoBehaviour
    {
        [SerializeField] private int goldCapacity;
        [SerializeField, Tooltip("The amount of time after being stolen from that this collector can't be stolen " +
            "from again.")] 
        private float dropIFrames;
        [SerializeField] private UnityEvent OnDropEvent;
        [SerializeField] private UnityEvent OnBecomeVulnerable;
        private readonly Queue<Collectable> heldCollectables = new();

        private bool isDisabled;

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

        #region Properties
        public bool DropDisabled => isDisabled;
        #endregion

        /// <summary>
        /// Check for gold collection when we enter a trigger.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Handles entering a collectable.
            // If a champion is disabled from just being stolen from, they can't recollect their dropped collectables.
            if (!isDisabled && 
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
            if (goldCapacity > 0 && heldCollectables.Count < goldCapacity)
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
            if (DropDisabled) { return null; }
            List<Collectable> droppedCollectables = new List<Collectable>();
            for(int i = 0; i < numToDrop && heldCollectables.Count > 0; i++)
            {
                Collectable dropped = heldCollectables.Dequeue();
                droppedCollectables.Add(dropped);
                dropped.OnDropped(this);
            }

            OnDropEvent?.Invoke();

            // Add IFrames to prevent dropping multiple times.
            StartCoroutine(DropFrames(dropIFrames));

            return droppedCollectables.ToArray();
        }

        /// <summary>
        /// Prevents the collector from dropping collectables again after they've been forced to drop collectables.
        /// </summary>
        /// <param name="seconds">The amount of invulnerability time the champion has.</param>
        /// <returns>cCoroutine</returns>
        private IEnumerator DropFrames(float seconds)
        {
            isDisabled = true;
            yield return new WaitForSeconds(seconds);
            isDisabled = false;
            OnBecomeVulnerable?.Invoke();
        }
    }
}
