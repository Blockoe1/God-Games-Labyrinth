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
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGL.Champions
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class StealProjectile : MonoBehaviour
    {
        private GoldStealer shooter;

        private readonly List<Collectable> attractedCollectables = new List<Collectable>();

        private bool isLaunched;
        private bool allowReturn;

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

        #region Nested
        public enum CollisionType
        { 
            Enter,
            Exit,
            Stay
        }
        #endregion

        /// <summary>
        /// Initializes this steal projectile with a reference to the champion that shoots it.
        /// </summary>
        /// <param name="shooter"></param>
        public void Initialize(GoldStealer shooter)
        {
            this.shooter = shooter;
        }

        /// <summary>
        /// Launches this projectile outward with a given direction and strength.
        /// </summary>
        /// <param name="launchVector"></param>
        public void Launch(Vector2 launchPosition, Vector2 launchVector)
        {
            // Prevent duplicate launches.
            if (isLaunched) { return; }
            transform.position = launchPosition;
            isLaunched = true;
            gameObject.SetActive(true);
            rb.AddForce(launchVector, ForceMode2D.Impulse);
        }

        /// <summary>
        /// Detect trigger collisions for projectile logic.
        /// </summary>
        /// <param name="collider"></param>
        private void OnTriggerEnter2D(Collider2D collider)
        {
            // If the projectile collides with the shooter, then it resets.
            if (allowReturn &&
                base.GetComponent<Collider>().gameObject == gameObject)
            {
                Cooldown();
                projectile.ProjectileReset();
            }
            // Make a collector drop held gold and then grab it with this projectile.
            else if (base.GetComponent<Collider>().gameObject != gameObject &&
                base.GetComponent<Collider>().TryGetComponent(out Attackable attackable) &&
                !attackable.IsInvincible)
            {
                OnHitAttackable(attackable, projectile);
            }
        }

        /// <summary>
        /// Only allow returning if we've left a champion hitbox already.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (GetComponent<Collider>().gameObject == gameObject)
            {
                allowReturn = true;
            }
        }

        /// <summary>
        /// Resets the projectile back to a disabled state.
        /// </summary>
        public void ProjectileReset()
        {
            gameObject.SetActive(false);
            transform.position = ReturnTarget.transform.position;
            isLaunched = false;

            CollectAllCollectables();
        }

        /// <summary>
        /// Continually pulls this projectile back towards it's original shooter.
        /// </summary>
        private void FixedUpdate()
        {
            Vector2 toTarget = (Vector2)ReturnTarget.transform.position - rb.position;
            //rb.AddForce(toTarget.normalized * returnForce, ForceMode2D.Force);
            //rb.MovePosition(Vector2.MoveTowards(ReturnTarget.transform.position, rb.position, returnForce));

            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, toTarget.normalized * returnVelocity, 
                returnAcceleration * Time.fixedDeltaTime);

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
                col.CollectDisabled = true;

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
                collectable.CollectDisabled = false;
                attractedCollectables.Remove(collectable);
            }
        }

        /// <summary>
        /// Collects all attracted collectables.
        /// </summary>
        private void CollectAllCollectables()
        {
            Collectable[] toCollect = new Collectable[attractedCollectables.Count];
            attractedCollectables.CopyTo(toCollect);

            // Collectables should clean up and remove themselves automatically when collected.
            foreach (Collectable col in toCollect)
            {
                // Have the collector we're returning to force-collect the collectables.
                RemoveAttractedCollectable(col);
                ReturnTarget.ForceCollect(col);
            }
        }

        /// <summary>
        /// Applies force to all collectables to pull them towards this projectile.
        /// </summary>
        private void AttractCollectables()
        {
            foreach(Collectable collectable in attractedCollectables)
            {
                //Vector2 forceDirection = rb.position - collectable.Rb.position;
                collectable.Rb.MovePosition(Vector2.MoveTowards(collectable.Rb.position, rb.position,
                    collectableAttractionForce));
            }
        }
        #endregion

        /// <summary>
        /// Controls what happens when the projectile hits a valid target.
        /// </summary>
        /// <param name="attackable"></param>
        /// <param name="projectile"></param>
        private void OnHitAttackable(Attackable attackable, StealProjectile projectile)
        {
            // If the hit object collects gold, steal it.
            if (attackable.TryGetComponent(out Collector collector))
            {
                Collectable[] droppedCollectables = collector.DropCollectables(stealAmount);

                // Setup collectables to be attracted to the projectile until collected.
                projectile.AddAttractedCollectables(droppedCollectables, Team);
            }

            // Notify the attackable that it was hit.
            attackable.OnHit();
        }
    }
}
