/*****************************************************************************
// File Name : JumpState.cs
// Author : Brandon Koederitz
// Creation Date : 3/22/2026
// Last Modified : 3/22/2026
//
// Brief Description : State for the minotaur jumping to a player if it patrols for too long to make it more menacing.
*****************************************************************************/
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GGL.Minotaur
{
    public class JumpState : MinotaurState
    {
        [Header("Jump Settings")]
        [SerializeField] private float jumpSpeed;
        [SerializeField] private AnimationCurve jumpSpeedCurve;
        [SerializeField] private AnimationCurve jumpSizeCurve;
        [SerializeField] private float postJumpDelay = 0.5f;

        private Vector2 jumpLocation;
        private int iterationNum;

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
            minotaur.movement.Stop();
            base.OnStateEnter();
        }

        /// <summary>
        /// Sequences the jump attack.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator StateRoutine()
        {
            minotaur.audioRelay.PlaySound(minotaur.snortSoundName);
            // Play snort particles
            foreach (var particle in minotaur.snortParticles)
            {
                particle.Play();
            }


            //yield return new WaitForSeconds(jumpChargeTime);

            //// Disable minotaur collision.
            //minotaur.movement.Rigidbody.excludeLayers = ~0;
            //minotaur.audioRelay.PlaySound(minotaur.dashSoundName);

            //// Jump logic.
            //Vector3 baseScale = minotaur.transform.localScale;
            Vector2 startingPosition = minotaur.movement.Rigidbody.position;
            float jumpTime = Vector2.Distance(startingPosition, jumpLocation) / jumpSpeed;
            yield return minotaur.jumper.PerformJump(jumpTime, jumpLocation, jumpSpeedCurve, jumpSizeCurve); 
            

            yield return new WaitForSeconds(postJumpDelay);

            // Return to the patrol with vision active state after jumping.
            PatrolState state = parent.SetState<PatrolState>();
            state.ToggleVision(true);
        }
    }
}
