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
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class ChampionDash : MonoBehaviour
    {
        #region CONSTS
        private const string DASH_ACTION_NAME = "Dash";
        #endregion

        [Header("Dash Settings")]
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashDuration;
        [SerializeField] private float dashCooldown;
        [SerializeField] private UnityEvent OnDashBegin;
        [SerializeField] private UnityEvent<float> OnDashEnd;

        private InputAction dashAction;
        private bool isDashing;
        private bool isCooldown;

        private Vector2 dashDirection;

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] private Rigidbody2D rb;
        [SerializeReference, ReadOnly] private PlayerInput input;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            rb = GetComponent<Rigidbody2D>();
            input = GetComponent<PlayerInput>();
        }
        #endregion

        #region Properties
        public Vector2 DashDirection
        {
            get { return dashDirection; }
            set { dashDirection = value; }
        }
        #endregion

        /// <summary>
        /// Setup Input
        /// </summary>
        private void Awake()
        {
            dashAction = input.actions.FindAction(DASH_ACTION_NAME);
            dashAction.performed += DashAction_performed;
        }
        private void OnDestroy()
        {
            dashAction.performed -= DashAction_performed;
        }

        /// <summary>
        /// Read player dash input.
        /// </summary>
        /// <param name="obj"></param>
        private void DashAction_performed(InputAction.CallbackContext obj)
        {
            StartCoroutine(Dash(dashDirection));
        }

        /// <summary>
        /// Dashes the player in a given direction.
        /// </summary>
        /// <param name="direction">The direction for the player to dash in.</param>
        private IEnumerator Dash(Vector2 direction)
        {
            // Prevent double dashing and dashing during cooldown.
            if (isDashing || isCooldown) { yield break; }

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

            OnDashEnd?.Invoke(dashCooldown);
            StartCoroutine(DashCooldown(dashCooldown));
            isDashing = false;
        }

        /// <summary>
        /// Prevents the player from dashing for a time.
        /// </summary>
        /// <param name="dashCooldown">The amount of time the player must cool down.</param>
        /// <returns></returns>
        private IEnumerator DashCooldown(float dashCooldown)
        {
            if (isCooldown) { yield break; }
            isCooldown = true;
            yield return new WaitForSeconds(dashCooldown);
            isCooldown = false;
        }
    }
}
