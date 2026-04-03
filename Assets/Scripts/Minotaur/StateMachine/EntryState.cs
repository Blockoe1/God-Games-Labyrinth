/*****************************************************************************
// File Name : EntryState.cs
// Author : Brandon Koederitz
// Creation Date : 3/28/2026
// Last Modified : 3/28/2026
//
// Brief Description : Controls the minotaurs behaviour when it spawns.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace GGL.Minotaur
{
    public class EntryState : MinotaurState
    {
        [SerializeField, Tooltip("The amount of time the minotaur takes to fall into the maze.")] 
        private float fallTime;
        [SerializeField, Tooltip("The amount of time the red circle appears before the minotaur starts falling.")] 
        private float landingTelegraphTime;
        [SerializeField, Tooltip("Controls the animation of the minotaur's size and opacity during the fall ")] 
        private AnimationCurve landingCurve;
        [SerializeField] private float postJumpDelay = 0.5f;
        [SerializeField] private string roarSoundName;

        private SpriteRenderer minotaurSprite;

        /// <summary>
        /// Set the minotaur invisible when it spawns.
        /// </summary>
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            minotaurSprite = minotaur.GetComponent<SpriteRenderer>();
            minotaurSprite.color = Color.clear;
        }

        /// <summary>
        /// Has the minotaur perform a jump
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator StateRoutine()
        {
            // Start the jump.
            minotaur.jumper.PerformJump(fallTime, minotaur.movement.Rigidbody.position, 
                AnimationCurve.Constant(0, 1, 1), landingCurve, landingTelegraphTime, "");

            yield return new WaitForSeconds(landingTelegraphTime);
            minotaur.audioRelay.PlaySound(roarSoundName);

            // Start a parallel coroutine to update opacity based on time elapsed.
            float timer = 0;
            while (timer < fallTime)
            {
                float normalizedTime = timer / fallTime;
                
                minotaurSprite.color = Color.Lerp(DebugColor, Color.clear, landingCurve.Evaluate(normalizedTime));

                timer += Time.deltaTime;
                yield return null;
            }

            minotaurSprite.color = DebugColor;

            yield return new WaitForSeconds(postJumpDelay + MinotaurJumper.HITBOX_IMPACT_TIME);

            // Return to the patrol with vision active state after jumping.
            PatrolState state = parent.SetState<PatrolState>();
            state.ToggleVision(true);
        }
    }
}
