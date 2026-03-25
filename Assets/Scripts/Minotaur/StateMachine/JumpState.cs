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
        #region CONSTS
        private const float HITBOX_IMPACT_TIME = 0.25f;
        #endregion

        [Header("Timing")]
        [SerializeField] private float jumpChargeTime = 2f;
        [SerializeField] private float postJumpDelay = 0.5f;
        [Header("Visuals")]
        [SerializeField] private GameObject landingTelegraph;
        [SerializeField] private GameObject jumpHitbox;
        [SerializeField] private GameObject[] disabledObjects;
        [SerializeField] private ParticleSystem landParticles;
        [Header("Jump Settings")]
        [SerializeField] private float jumpSpeed;
        [SerializeField] private AnimationCurve jumpSpeedCurve;
        [SerializeField] private float maxJumpScaleMultiplier;
        [SerializeField] private AnimationCurve jumpSizeCurve;

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
            Debug.Log("Jumping");
            minotaur.movement.Stop();

            ToggleDisabledObjects(false);

            // Initalize the landing telegraph.
            landingTelegraph.SetActive(true);
            landingTelegraph.transform.position = jumpLocation;

            Vector2 toTarget = (jumpLocation - minotaur.movement.Rigidbody.position).normalized;
            minotaur.rotation.SetRotation(toTarget);
            base.OnStateEnter();
        }

        private void ToggleDisabledObjects(bool isEnabled)
        {
            foreach (var obj in disabledObjects)
            {
                obj.SetActive(isEnabled);
            }
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

            // Disable minotaur collision.
            minotaur.movement.Rigidbody.excludeLayers = ~0;

            // Jump logic.
            Vector3 baseScale = minotaur.transform.localScale;
            Vector2 startingPosition = minotaur.movement.Rigidbody.position;
            float jumpTime = Vector2.Distance(startingPosition, jumpLocation) / jumpSpeed;
            float timer = 0;
            while (timer < jumpTime)
            {
                float normalizedTime = timer / jumpTime;
                minotaur.movement.Rigidbody.MovePosition(Vector2.Lerp(startingPosition, jumpLocation, 
                    jumpSpeedCurve.Evaluate(normalizedTime)));
                minotaur.transform.localScale = baseScale + 
                    (baseScale * jumpSizeCurve.Evaluate(normalizedTime) * (maxJumpScaleMultiplier - 1));

                timer += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            // Re-Enable minotaur collision.
            minotaur.movement.Rigidbody.excludeLayers = 0;

            // Hitbox impact.
            landingTelegraph.SetActive(false);
            landParticles.Play();
            jumpHitbox.SetActive(true);
            yield return new WaitForSeconds(HITBOX_IMPACT_TIME);
            jumpHitbox.SetActive(false);

            yield return new WaitForSeconds(postJumpDelay);

            // Return to the patrol with vision active state after jumping.
            PatrolState state = parent.SetState<PatrolState>();
            state.ToggleVision(true);
        }

        /// <summary>
        /// Cleans up the jump state.
        /// </summary>
        public override void OnStateExit()
        {
            base.OnStateExit();
            ToggleDisabledObjects(true);
        }
    }
}
