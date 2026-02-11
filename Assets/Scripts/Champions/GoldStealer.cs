/*****************************************************************************
// File Name : GoldStealer.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Fires a projectile that steals gold on contact with another player.
*****************************************************************************/
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGL.Champions
{
    public class GoldStealer : ChampionBehavior
    {
        protected override string actionName => "Steal";

        [Header("Steal Settings")]
        [SerializeField] private StealProjectile projectile;
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
                projectile.Launch(Direction * launchForce);
            }
        }
    }
}
