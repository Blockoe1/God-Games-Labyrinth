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
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGL.Champions
{
    [RequireComponent(typeof(EntityMover))]
    public class ChampionDash : ChampionMovementController
    {
        #region CONSTS
        private const string DASH_ACTION_NAME = "Dash";
        #endregion

        [Header("Dash Settings")]
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashDuration;

        private InputAction dashAction;
        private bool isDashing;

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] private EntityMover movement;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        protected override void Reset()
        {
            base.Reset();
            movement = GetComponent<EntityMover>();
        }
        #endregion

        /// <summary>
        /// Setup Input
        /// </summary>
        private void Awake()
        {
            dashAction = Input.actions.FindAction(DASH_ACTION_NAME);
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
            StartCoroutine(Dash(movement.Direction));
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
            movement.BlockMovement = false;
            isDashing = true;

            float timer = dashDuration;
            while (timer > 0)
            {
                Body.linearVelocity = direction * dashSpeed;

                timer -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            isDashing = false;
            movement.BlockMovement = true;
        }
    }
}
