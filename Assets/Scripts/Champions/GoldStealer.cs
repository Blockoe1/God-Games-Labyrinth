/*****************************************************************************
// File Name : GoldStealer.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Fires a projectile that steals gold on contact with another player.
*****************************************************************************/
using GGL.Scoring;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGL.Champions
{
    public class GoldStealer : ChampionBehavior
    {
        protected override string actionName => "Steal";

        [Header("Steal Settings")]
        [SerializeField] private StealProjectile projectile;
        [SerializeField] private float stealAmount;
        [SerializeField] private float launchForce;
        [SerializeField, Tooltip("The amount of empty space that must be in front of the champion to use this" +
            " ability.")] 
        private float requiredLeeway = 2;

        /// <summary>
        /// Fires the steal projectile when the player presses the correct button.
        /// </summary>
        protected override void OnActionPerformed()
        {
            // If the projectile is already launched, then it can't be launched again.
            if (projectile.IsLaunched) { return; }  
            RaycastHit2D forwardCheck = Physics2D.Raycast(transform.position, Direction, requiredLeeway, GGLHelpers.MazeMask);
            if (!forwardCheck)
            {
                projectile.Launch(Direction * launchForce, ProjectileCollision);
            }
        }

        /// <summary>
        /// Logic for when the steal projectile collides with an object.
        /// </summary>
        /// <param name="collider">The object the projectile collided with.</param>
        /// <param name="projectile">The projectile that the collision occured on.</param>
        private void ProjectileCollision(Collider2D collider, StealProjectile projectile)
        {
            // If the projectile collides with the shooter, then it resets.
            if (collider.gameObject == gameObject)
            {
                projectile.ProjectileReset();
            }
            // Make a collector drop held gold and then grab it with this projectile.
            else if (collider.TryGetComponent(out Collector collector) && !collector.DropDisabled)
            {
                Collectable[] droppedCollectables = collector.DropCollectables(stealAmount);

                // Setup collectables to be attracted to the projectile until collected.
                projectile.AddAttractedCollectables(droppedCollectables, Team);
            }
        }
    }
}
