/*****************************************************************************
// File Name : ChampionDash.cs
// Author : Brandon Koederitz
// Creation Date : 1/26/2026
// Last Modified : 1/26/2026
//
// Brief Description : Allows the champions to dash based on button press.
*****************************************************************************/
using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GGL.Champions
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ChampionDash : ChampionBehavior
    {
        [Header("Dash Settings")]
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashDuration;
        [SerializeField] private UnityEvent OnDashBegin;
        [SerializeField] private UnityEvent OnDashEnd;

        private bool isDashing;

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] private Rigidbody2D rb;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References: 1")]
        protected override void Reset()
        {
            base.Reset();
            rb = GetComponent<Rigidbody2D>();
        }
        #endregion

        /// <summary>
        /// Begin a dash when the player presses the dash button.
        /// </summary>
        protected override void OnActionPerformed()
        {
            StartCoroutine(Dash(Direction));
        }

        /// <summary>
        /// Dashes the player in a given direction.
        /// </summary>
        /// <param name="direction">The direction for the player to dash in.</param>
        private IEnumerator Dash(Vector2 direction)
        {
            // Prevent double dashing.
            if (isDashing) { yield break; }

            direction = direction.normalized;
            isDashing = true;
            OnDashBegin?.Invoke();

            float timer = dashDuration;
            while (timer > 0)
            {
                rb.linearVelocity = direction * dashSpeed;

                timer -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            OnDashEnd?.Invoke();
            Cooldown();
            isDashing = false;
        }
    }
}
