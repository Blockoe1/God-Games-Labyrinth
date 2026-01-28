/*****************************************************************************
// File Name : Collectable.cs
// Author : Brandon Koederitz
// Creation Date : 1/28/2026
// Last Modified : 1/28/2026
//
// Brief Description : Allows an object to be collected by the players.
*****************************************************************************/
using GGL.Networking;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GGL.Scoring
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private int pointValue;
        [SerializeField, Tooltip("The amount of time after this collectable has been dropped before it can be " +
            "picked up again.")] 
        private float dropPickupDelay = 1;
        [SerializeField, Tooltip("The min and max force that can be applied to a dropped collectable.")] 
        private Vector2 scatterForce;
        [Header("Events")]
        [SerializeField] private UnityEvent OnCollect;
        [SerializeField] private UnityEvent OnDrop;

        private bool canBeCollected = true;

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] private Rigidbody2D rb;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        #endregion

        #region Properties
        public int PointValue => pointValue;
        public bool CanBeCollected => canBeCollected;
        #endregion

        /// <summary>
        /// Called when this object is collected.
        /// </summary>
        public void OnCollected(Collector collector)
        {
            gameObject.SetActive(false);
            OnCollect?.Invoke();
        }

        /// <summary>
        /// Called when this object is dropped.
        /// </summary>
        public void OnDropped(Collector collector)
        {
            // Snap the collected item to the dropped champion's position.
            rb.position = collector.transform.position;

            gameObject.SetActive(true);
            StartCoroutine(PauseCollection(dropPickupDelay));
            ApplyScatterForce();

            OnDrop?.Invoke();
        }

        /// <summary>
        /// Called when the gold is cashed as score by the champion returning to their base.
        /// </summary>
        public void OnCashed()
        {
            // Do Cleanup here.
            Destroy(gameObject);
        }

        /// <summary>
        /// Pauses collecting this colelctable for a time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public IEnumerator PauseCollection(float time)
        {
            if (!canBeCollected) { yield break; }
            canBeCollected = false;

            yield return new WaitForSeconds(time);

            canBeCollected = true;
        }

        /// <summary>
        /// Applies a randomized force to this colelctable so it scatters when dropped.
        /// </summary>
        private void ApplyScatterForce()
        {

        }
    }
}
