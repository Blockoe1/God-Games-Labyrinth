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
using UnityEditor.EventSystems;
using UnityEngine;

namespace GGL.Minotaur
{
    public class ChargeState : MinotaurState
    {
        #region CONSTS
        private const float CRASH_CHECK_DISTANCE = 0.75f;
        #endregion

        [SerializeField] private float chargeSpeed;
        [SerializeField] private float crashDelay;
        [SerializeField] private float crashKnockback;
        [SerializeField] private ParticleSystem[] snortParticles;
        [SerializeField] private ParticleSystem chargeParticles;
        [SerializeField] private ParticleSystem crashParticles;

        private Vector2 chargeDirection;

        private float snortTime;

        private Rigidbody2D rb => minotaur.movement.Rigidbody;
        public override void OnStateEnter()
        {
            var main = snortParticles.FirstOrDefault().main;
            snortTime = main.duration + main.startLifetime.constant;
            minotaur.movement.Stop(true);
            base.OnStateEnter();
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
                rb.linearVelocity = chargeDirection * chargeSpeed;

                // Detect wall for crash.
                RaycastHit2D ray = Physics2D.Raycast(rb.position,  chargeDirection, CRASH_CHECK_DISTANCE,
                        GGLHelpers.MoveCheckMask | GGLHelpers.MazeMask);
                // If the raycast hit something, the minotaur hit a wall.
                if (ray)
                {
                    break;
                }
                yield return new WaitForFixedUpdate();
            }

            chargeParticles.Stop();
            minotaur.movement.enabled = true;
            minotaur.movement.ApplyKnockback(-chargeDirection, crashKnockback);
            crashParticles.Play();

            yield return new WaitForSeconds(crashDelay);

            // Return to chasing.
            parent.SetState<ChaseState>();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            chargeParticles.Stop();
            minotaur.movement.enabled = true;
        }
    }
}
