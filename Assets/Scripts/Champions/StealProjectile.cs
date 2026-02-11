/*****************************************************************************
// File Name : StealProjectile.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Projectile fired from a champion to steal gold from another player.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGL.Champions
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class StealProjectile : MonoBehaviour
    {
        [SerializeField] private Transform returnTarget;
        [SerializeField] private float returnForce;

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
        public void Launch(Vector2 launchVector)
        {
            // Prevent duplicate launches.
            if (isLaunched) { return; }
            isLaunched = true;
            rb.AddForce(launchVector, ForceMode2D.Impulse);
        }

        /// <summary>
        /// Continually pulls this projectile back towards it's original shooter.
        /// </summary>
        private void FixedUpdate()
        {
            
        }
    }
}
