/*****************************************************************************
// File Name : GoldStealer.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Fires a projectile that steals gold on contact with another player.
*****************************************************************************/
using GGL.Scoring;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGL.Champions
{
    public class GoldStealer : ChampionBehavior
    {
        protected override string actionName => "Steal";

        [Header("Steal Settings")]
        [SerializeField] private StealProjectile projectilePrefab;
        [SerializeField] private float stealAmount;
        [SerializeField] private float launchForce;
        [SerializeField, Tooltip("The amount of empty space that must be in front of the champion to use this" +
            " ability.")] 
        private float requiredLeeway = 2;

        private bool allowReturn;

        private StealProjectile proj;
        private StealProjectile Projectile
        { 
            get
            {
                // Spawns a new projectile for this champion to use parented to this object's parent.
                if (proj == null)
                {
                    proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity, transform.parent);
                    proj.ReturnTarget = transform;
                }
                return proj;
            }
        }

        /// <summary>
        /// Fires the steal projectile when the player presses the correct button.
        /// </summary>
        protected override void OnActionPerformed()
        {
            // If the projectile is already launched, then it can't be launched again.
            if (Projectile.IsLaunched) { return; }  
            RaycastHit2D forwardCheck = Physics2D.Raycast(transform.position, Direction, requiredLeeway, GGLHelpers.MazeMask);
            if (!forwardCheck)
            {
                allowReturn = false;
                Debug.Log(Direction);
                Projectile.Launch(transform.position, Direction * launchForce, ProjectileCollision);
            }
        }

        /// <summary>
        /// Logic for when the steal projectile collides with an object.
        /// </summary>
        /// <param name="collider">The object the projectile collided with.</param>
        /// <param name="projectile">The projectile that the collision occured on.</param>
        private void ProjectileCollision(Collider2D collider, StealProjectile projectile, 
            StealProjectile.CollisionType collisionType)
        {
            switch(collisionType)
            {
                case StealProjectile.CollisionType.Enter:
                    // If the projectile collides with the shooter, then it resets.
                    if (allowReturn && 
                        collider.gameObject == gameObject)
                    {
                        Cooldown();
                        projectile.ProjectileReset();
                    }
                    // Make a collector drop held gold and then grab it with this projectile.
                    else if (collider.gameObject != gameObject && 
                        collider.TryGetComponent(out Collector collector) && 
                        !collector.DropDisabled)
                    {
                        Collectable[] droppedCollectables = collector.DropCollectables(stealAmount);

                        // Setup collectables to be attracted to the projectile until collected.
                        projectile.AddAttractedCollectables(droppedCollectables, Team);
                    }
                    break;
                // Only allow returning after the projectile has left this object.
                case StealProjectile.CollisionType.Exit:
                    if (collider.gameObject == gameObject)
                    {
                        allowReturn = true;
                    }
                    break;
            }
            
        }
    }
}
