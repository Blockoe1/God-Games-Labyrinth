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
using System;
using System.Collections;
using Unity.VisualScripting;
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

        public Action<Collectable> OnCashedCallback {  get; set; }

        private bool collectCooldown;
        public bool CollectDisabled { get; set; }

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

        #region Nested
        private class OneShotWrapper
        {
            internal UnityAction handledAction;
            internal readonly Action triggeredAction;

            internal OneShotWrapper(Action triggerAction)
            {
                this.triggeredAction = triggerAction;
            }
        }
        #endregion

        #region Properties
        public int PointValue => pointValue;
        public bool IsCollectable => !collectCooldown && !CollectDisabled;
        public Rigidbody2D Rb => rb;
        #endregion

        /// <summary>
        /// Called when this object is collected.
        /// </summary>
        public void OnCollected(Collector collector)
        {
            gameObject.SetActive(false);
            StopAllCoroutines();
            collectCooldown = false;
            CollectDisabled = false;
            OnCollect?.Invoke();
        }

        /// <summary>
        /// Called when this object is dropped.
        /// </summary>
        public void OnDropped(Collector collector)
        {
            collectCooldown = true;
            transform.position = collector.transform.position;
            gameObject.SetActive(true);
            StartCoroutine(PauseCollection(dropPickupDelay));
            // Snap the collected item to the dropped champion's position.
            ApplyScatterForce(UnityEngine.Random.Range(scatterForce.x, scatterForce.y));

            OnDrop?.Invoke();
        }

        /// <summary>
        /// Called when the gold is cashed as score by the champion returning to their base.
        /// </summary>
        public void OnCashed()
        {
            // Callback to return the spawned collectable to the object pool.
            OnCashedCallback?.Invoke(this);
        }

        /// <summary>
        /// Pauses collecting this colelctable for a time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public IEnumerator PauseCollection(float time)
        {
            collectCooldown = true;

            yield return new WaitForSeconds(time);

            collectCooldown = false;
        }

        /// <summary>
        /// Applies a randomized force to this colelctable so it scatters when dropped.
        /// </summary>
        public void ApplyScatterForce(float scatterForce)
        {
            int randomAngle = UnityEngine.Random.Range(0, 360);

            Vector2 forceVector = MathHelpers.DegAngleToUnitVector(randomAngle);
            rb.AddForce(forceVector * scatterForce, ForceMode2D.Impulse);
        }

        /// <summary>
        /// Has this collectable ignore physics collisions with the maze.
        /// </summary>
        /// <param name="ignore">True if collisions should be ignored, false if not.</param>
        public void IgnoreMazeCollision(bool ignore)
        {
            LayerMask mazeMask = GGLHelpers.MazeMask | GGLHelpers.MoveCheckMask;
            rb.excludeLayers = ignore ? rb.excludeLayers |  mazeMask: 
                rb.excludeLayers & ~mazeMask;
        }

        #region Event Subscriptions
        /// <summary>
        /// Adds a subscriber to the collectable's OnCollect event.
        /// </summary>
        /// <param name="onCollectAction"></param>
        public void SubscribeCollectEvent(UnityAction onCollectAction)
        {
            OnCollect.AddListener(onCollectAction);
        }
        /// <summary>
        /// Removes a subscriber from the OnCollect Event.
        /// </summary>
        /// <param name="onCollectAction"></param>
        public void UnsubscribeCollectEvent(UnityAction onCollectAction)
        {
            OnCollect.RemoveListener(onCollectAction);
        }

        /// <summary>
        /// Adds a subscriber to the OnCollect event that only gets called during the next collection.
        /// </summary>
        /// <param name="oneShot">The action to call when the one shot occurs.</param>
        public void SubscribeCollectOneShot(Action oneShot)
        {
            OneShotWrapper osw = new OneShotWrapper(oneShot);
            void HandledAction() { HandleCollectOneShot(osw); }
            osw.handledAction = HandledAction;
            SubscribeCollectEvent(osw.handledAction);
        }

        /// <summary>
        /// Called by the OnCollect event when a one shot is called to automatically unsubscribe the action.
        /// </summary>
        /// <param name="osw">The wrapper class that contains the UnityAction to unsubscribe.</param>
        private void HandleCollectOneShot(OneShotWrapper osw)
        {
            // Unsubscribe this action.
            UnsubscribeCollectEvent(osw.handledAction);
            osw.triggeredAction();
        }
        #endregion
    }
}
