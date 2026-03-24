/*****************************************************************************
// File Name : JumpState.cs
// Author : Brandon Koederitz
// Creation Date : 3/22/2026
// Last Modified : 3/22/2026
//
// Brief Description : State for the minotaur jumping to a player if it patrols for too long to make it more menacing.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace GGL.Minotaur
{
    public class JumpState : MinotaurState
    {
        [SerializeField] private GameObject landingTelegraph;
        [SerializeField] private float jumpChargeTime = 2f;
        [SerializeField] private float postJumpDelay = 0.5f;
        [Header("Jump Settings")]
        [SerializeField] private float jumpSpeed;
        [SerializeField] private AnimationCurve jumpSizeCurve;

        private Vector2 jumpLocation;

        /// <summary>
        /// Initializes the jump state with a set of possible jump targets.
        /// </summary>
        /// <param name="jumpTargets">An array of elidgeable jump targets.</param>
        public void Initialize(Vector2 jumpLocation)
        {
            this.jumpLocation = jumpLocation;
        }

        /// <summary>
        /// Sets up the jump state telegraphing.
        /// </summary>
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            minotaur.movement.Stop();

            // Initalize the landing telegraph.
            landingTelegraph.SetActive(true);
            landingTelegraph.transform.position = jumpLocation;
        }

        /// <summary>
        /// Sequences the jump attack.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator StateRoutine()
        {            
            // Play snort particles
            foreach (var particle in minotaur.snortParticles)
            {
                particle.Play();
            }

            yield return new WaitForSeconds(jumpChargeTime);

            // Jump logic.
            // Disable minotaur collision.
            minotaur.movement.Rigidbody.excludeLayers = ~0;

            // Hitbox impact.
            landingTelegraph.SetActive(false);

            // Re-Enable minotaur collision.
            minotaur.movement.Rigidbody.excludeLayers = 0;

            yield return new WaitForSeconds(postJumpDelay);
        }

        /// <summary>
        /// Cleans up the jump state.
        /// </summary>
        public override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}
