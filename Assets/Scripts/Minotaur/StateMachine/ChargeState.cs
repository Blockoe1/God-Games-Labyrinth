/*****************************************************************************
// File Name : ChargeState.cs
// Author : Brandon Koederitz
// Creation Date : 3/10/2026
// Last Modified : 3/10/2026
//
// Brief Description : State for the minotaur charging at a player down a hallway.
*****************************************************************************/
using System.Collections;
using System.Linq;
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

        private float snortTime;
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            var main = snortParticles.FirstOrDefault().main;
            snortTime = main.duration + main.startLifetime.constant;
        }

        /// <summary>
        /// Controls the sequencing of the minotaur charging.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator StateRoutine()
        {
            chargeDirection = minotaur.movement.Direction;

            // Play snort particles
            foreach(var particle in snortParticles )
            {
                particle.Play();
            }
            yield return new WaitForSeconds(snortTime);


            // Set velocity for charge.
            chargeParticles.Play();
            minotaur.movement.enabled = false;

            while(true)
            {
                minotaur.movement.Rigidbody.linearVelocity = chargeDirection * chargeSpeed;

                // Detect wall for crash.
                RaycastHit2D ray = Physics2D.Raycast(minotaur.movement.Rigidbody.position, direction, maxWallCheckDistance,
                        GGLHelpers.MoveCheckMask | GGLHelpers.MazeMask);
                // If the raycast hit nothing, this is a valid direction.
                if (!ray)
                {
                    
                }
            }

        }
    }
}
