/*****************************************************************************
// File Name : ChargeState.cs
// Author : Brandon Koederitz
// Creation Date : 3/10/2026
// Last Modified : 3/10/2026
//
// Brief Description : State for the minotaur charging at a player down a hallway.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace GGL.Minotaur
{
    public class ChargeState : MinotaurState
    {
        [SerializeField] private float chargeSpeed;
        [SerializeField] private ParticleSystem[] snortParticles;
        [SerializeField] private ParticleSystem chargeParticles;
        [SerializeField] private ParticleSystem crashParticles;

        private Vector2 chargeDirection;

        /// <summary>
        /// Controls the sequencing of the minotaur charging.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator StateRoutine()
        {
            chargeDirection = minotaur.movement.Direction;

            // Play snort particles


            // Set velocity for charge.

            // Detect wall for crash.
            throw new System.NotImplementedException();
        }
    }
}
