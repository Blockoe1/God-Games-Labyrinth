/*****************************************************************************
// File Name : StealProjectile.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Projectile fired from a champion to steal gold from another player.
*****************************************************************************/
using GGL.Scoring;
using NaughtyAttributes;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGL.Champions
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class StealProjectile : MonoBehaviour
    {
        [SerializeField] private Transform returnTarget;
        [SerializeField] private float returnForce;
        [SerializeField] private float collectableAttractionForce;

        private Action<Collider2D, StealProjectile> collisionLogic;
        private readonly List<Collectable> attractedCollectables = new List<Collectable>();
        private bool isLaunched; 

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] protected Rigidbody2D rb;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        protected virtual void Reset()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        #endregion

        #region Properties
        public bool IsLaunched => isLaunched;
        #endregion

        /// <summary>
        /// Launches this projectile outward with a given direction and strength.
        /// </summary>
        /// <param name="launchVector"></param>
        public void Launch(Vector2 launchVector, Action<Collider2D, StealProjectile> collisionLogic)
        {
            // Prevent duplicate launches.
            if (isLaunched) { return; }
            isLaunched = true;
            this.collisionLogic = collisionLogic;
            gameObject.SetActive(true);
            rb.AddForce(launchVector, ForceMode2D.Impulse);
        }

        /// <summary>
        /// Detect trigger collisions for projectile logic.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            collisionLogic?.Invoke(collision, this);
        }

        /// <summary>
        /// Resets the projectile back to a disabled state.
        /// </summary>
        public void ProjectileReset()
        {
            gameObject.SetActive(false);
            transform.position = returnTarget.transform.position;

            ClearAttractedCollectables();
        }

        /// <summary>
        /// Continually pulls this projectile back towards it's original shooter.
        /// </summary>
        private void FixedUpdate()
        {
            Vector2 toTarget = (Vector2)returnTarget.position - rb.position;
            rb.AddForce(toTarget.normalized * returnForce, ForceMode2D.Force);

            AttractCollectables();
        }

        #region Collectable Attraction
        /// <summary>
        /// Adds an array of collectables to be attracted to this projectile.
        /// </summary>
        /// <param name="collectablesToAttract">The collectables to attract to this projectile.</param>
        public void AddAttractedCollectables(Collectable[] collectablesToAttract, GodID team)
        {
            foreach(Collectable col in collectablesToAttract)
            {
                attractedCollectables.Add(col);
                // Disable collectable collision with everything except the target character.
                col.IgnoreMazeCollision(true);
                col.ApplyCollectMask(team);

                col.SubscribeCollectOneShot(() => { RemoveAttractedCollectable(col); });
            }
        }

        /// <summary>
        /// Removes a collectable from this projectile's attraction list.
        /// </summary>
        /// <param name="collectable">The collectable to remove from this projectile's attraction list.</param>
        private void RemoveAttractedCollectable(Collectable collectable)
        {
            if (attractedCollectables.Contains(collectable))
            {
                collectable.IgnoreMazeCollision(false);
                collectable.RemoveCollectMask();
                attractedCollectables.Remove(collectable);
            }
        }

        /// <summary>
        /// Clears all logic for attracted collectables.
        /// </summary>
        private void ClearAttractedCollectables()
        {
            foreach (Collectable col in attractedCollectables)
            {

            }
            attractedCollectables.Clear();
        }

        /// <summary>
        /// Applies force to all collectables to pull them towards this projectile.
        /// </summary>
        private void AttractCollectables()
        {
            foreach(Collectable collectable in attractedCollectables)
            {
                Vector2 forceDirection = rb.position - collectable.Rb.position;
                collectable.Rb.AddForce(forceDirection.normalized * collectableAttractionForce, ForceMode2D.Force);
            }
        }
        #endregion
    }
}
