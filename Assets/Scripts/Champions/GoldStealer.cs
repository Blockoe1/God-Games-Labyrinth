/*****************************************************************************
// File Name : GoldStealer.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Fires a projectile that steals gold on contact with another player.
*****************************************************************************/
using GGL.Scoring;
using NaughtyAttributes;
using UnityEngine;

namespace GGL.Champions
{
    [RequireComponent(typeof(Collector))]
    public class GoldStealer : ChampionBehavior
    {
        protected override string actionName => "Steal";

        [Header("Steal Settings")]
        [SerializeField] private StealProjectile projectilePrefab;
        [SerializeField, Tooltip("The speed at which the projectile is initially shot at.")] 
        private float launchForce;
        [field: SerializeField, Range(0, 1f), Tooltip("The proportion of gold that is stolen from hit champions.")] 
        public float StealAmount { get; private set; }
        [field: SerializeField, Tooltip("The max speed that the projectile returns at.")] 
        public float ReturnVelocity { get; private set; }
        [field: SerializeField, Tooltip("How quickly the projectile turns around to return to the shooting champion.")]
        public float ReturnAcceleration { get; private set; }
        [field: SerializeField, Tooltip("How strongly stolen gold is pulled to the projectile.")] 
        public float CollectableAttractionForce { get; private set; }
        [SerializeField, Tooltip("The amount of empty space that must be in front of the champion to use this" +
            " ability.")] 
        private float requiredLeeway = 2;

        private StealProjectile proj;

        #region Properties
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
        #endregion

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] protected Collector collector;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References: 1")]
        protected override void Reset()
        {
            base.Reset();
            collector = GetComponent<Collector>();
        }
        #endregion

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
            // Force-Collect all the stolen collectables.
            foreach(Collectable collectable in stoleCollectables)
            {
                collector.ForceCollect(collectable);
            }
        }
    }
}
