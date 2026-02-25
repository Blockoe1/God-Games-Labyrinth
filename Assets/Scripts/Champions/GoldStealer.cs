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

namespace GGL.Champions
{
    [RequireComponent(typeof(Collector))]
    public class GoldStealer : ChampionBehavior
    {
        protected override string actionName => "Steal";

        [Header("Steal Settings")]
        [SerializeField] private StealProjectile projectilePrefab;
        [SerializeField] private float launchForce;
        [field: SerializeField] public float StealAmount { get; private set; }
        [field: SerializeField] public float ReturnVelocity { get; private set; }
        [field: SerializeField] public float ReturnAcceleration { get; private set; }
        [field: SerializeField] public float CollectableAttractionForce { get; private set; }
        [SerializeField, Tooltip("The amount of empty space that must be in front of the champion to use this" +
            " ability.")] 
        private float requiredLeeway = 2;

        private StealProjectile proj;
        private StealProjectile Projectile
        { 
            get
            {
                // Spawns a new projectile for this champion to use parented to this object's parent.
                if (proj == null)
                {
                    proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity, transform.parent);
                    proj.Initialize(this);
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
                Projectile.Launch(transform.position, Direction * launchForce);
            }
        }

        /// <summary>
        /// Called when the projectile returns to the champion.
        /// </summary>
        /// <param name="stoleCollectables">The collectibles this projectile stole.</param>
        public void OnReturn(Collectable[] stoleCollectables)
        {

        }
    }
}
