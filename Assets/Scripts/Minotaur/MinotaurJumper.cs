/*****************************************************************************
// File Name : MinotaurJumper.cs
// Author : Brandon Koederitz
// Creation Date : 3/28/2026
// Last Modified : 3/28/2026
//
// Brief Description : Controls logic for the minotaur performing jump based attacks.
*****************************************************************************/
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

namespace GGL.Minotaur
{
    public class MinotaurJumper : MonoBehaviour
    {
        #region CONSTS
        private const float HITBOX_IMPACT_TIME = 0.25f;
        #endregion

        [SerializeField] private float maxJumpScaleMultiplier;
        [SerializeField] private float jumpChargeTime = 2f;
        [Header("Visuals")]
        [SerializeField] private GameObject landingTelegraph;
        [SerializeField] private GameObject jumpHitbox;
        [SerializeField] private GameObject[] disabledObjects;
        [SerializeField] private ParticleSystem landParticles;
        [SerializeField] private float screenShakeImpulse;

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] protected MinotaurController minotaur;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        protected virtual void Reset()
        {
            minotaur = GetComponent<MinotaurController>();
        }
        #endregion

        #region PerformJump Overloads
        public Coroutine PerformJump(float jumpTime, Vector2 targetPosition, AnimationCurve jumpPositionCurve, 
            AnimationCurve jumpSizeCurve)
        {
            return PerformJump(jumpTime, minotaur.movement.Rigidbody.position, targetPosition, jumpPositionCurve, 
                jumpSizeCurve, jumpChargeTime);
        }

        public Coroutine PerformJump(float jumpTime, Vector2 targetPosition, AnimationCurve jumpPositionCurve,
    AnimationCurve jumpSizeCurve, float jumpChargeTime)
        {
            return PerformJump(jumpTime, minotaur.movement.Rigidbody.position, targetPosition, jumpPositionCurve,
                jumpSizeCurve, jumpChargeTime);
        }

        public Coroutine PerformJump(float jumpTime, Vector2 startPosition, Vector2 targetPosition, AnimationCurve jumpPositionCurve,
AnimationCurve jumpSizeCurve)
        {
            return PerformJump(jumpTime, minotaur.movement.Rigidbody.position, targetPosition, jumpPositionCurve,
                jumpSizeCurve, jumpChargeTime);
        }
        #endregion

        /// <summary>
        /// Has the minotaur perform a jump attack
        /// </summary>
        /// <param name="jumpTime"></param>
        /// <param name="startPosition"></param>
        /// <param name="targetPosition"></param>
        /// <param name="jumpPositionCurve"></param>
        /// <param name="jumpSizeCurve"></param>
        /// <returns></returns>
        public Coroutine PerformJump(float jumpTime, Vector2 startPosition, Vector2 targetPosition, 
            AnimationCurve jumpPositionCurve, AnimationCurve jumpSizeCurve, float jumpChargeTime)
        {
            // Initalize the landing telegraph.
            if (landingTelegraph != null)
            {
                landingTelegraph.SetActive(true);
                landingTelegraph.transform.position = targetPosition;
            }

            // Rotate the minotaur.
            Vector2 toTarget = (targetPosition - minotaur.movement.Rigidbody.position).normalized;
            minotaur.rotation.SetRotation(toTarget);


            return StartCoroutine(JumpRoutine(jumpTime, startPosition, targetPosition, jumpPositionCurve, 
                jumpSizeCurve, jumpChargeTime));
        }

        private IEnumerator JumpRoutine(float jumpTime, Vector2 startPosition, 
            Vector2 targetPosition, AnimationCurve jumpPositionCurve, AnimationCurve jumpSizeCurve, float jumpChargeTime)
        {
            ToggleDisabledObjects(false);

            yield return new WaitForSeconds(jumpChargeTime);

            // Disable minotaur collision.
            minotaur.movement.Rigidbody.excludeLayers = ~0;
            minotaur.audioRelay.PlaySound(minotaur.dashSoundName);
            minotaur.movement.Stop();

            // Jump logic.
            Vector3 baseScale = minotaur.transform.localScale;
            float timer = 0;
            while (timer < jumpTime)
            {
                float normalizedTime = timer / jumpTime;
                minotaur.movement.Rigidbody.MovePosition(Vector2.Lerp(startPosition, targetPosition,
                    jumpPositionCurve.Evaluate(normalizedTime)));
                minotaur.transform.localScale = baseScale +
                    (baseScale * jumpSizeCurve.Evaluate(normalizedTime) * (maxJumpScaleMultiplier - 1));

                timer += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            minotaur.transform.localScale = baseScale;

            // Re-Enable minotaur collision.
            minotaur.movement.Rigidbody.excludeLayers = 0;

            // Hitbox impact.
            minotaur.screenShake.GenerateImpulse(screenShakeImpulse);
            if (landingTelegraph != null)
            {
                landingTelegraph.SetActive(false);
            }
            landParticles.Play();
            minotaur.audioRelay.PlaySound(minotaur.crashSoundName);
            jumpHitbox.SetActive(true);
            yield return new WaitForSeconds(HITBOX_IMPACT_TIME);
            jumpHitbox.SetActive(false);

            ToggleDisabledObjects(true);
        }

        private void ToggleDisabledObjects(bool isEnabled)
        {
            foreach (var obj in disabledObjects)
            {
                obj.SetActive(isEnabled);
            }
        }
    }
}
