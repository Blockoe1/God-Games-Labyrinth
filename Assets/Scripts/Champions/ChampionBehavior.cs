/*****************************************************************************
// File Name : ChampionBehavior.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Abstract base class for actions that champions can perform by pressing certain buttons.
*****************************************************************************/
using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GGL.Champions
{
    [RequireComponent(typeof(ChampionMovement))]
    [RequireComponent(typeof(PlayerInput))]
    public abstract class ChampionBehavior : MonoBehaviour
    {
        [SerializeField] private float cooldown;
        [SerializeField] private UnityEvent OnCooldownExpire;

        private InputAction performAction;
        private bool isCooldown;

        public event Action<float> OnCooldownEvent;
        public event Action OnActionPerformedEvent;

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] private ChampionMovement movement;
        [SerializeReference, ReadOnly] private GodIdentifier id;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References: 0")]
        protected virtual void Reset()
        {
            movement = GetComponent<ChampionMovement>();
            id = GetComponent<GodIdentifier>();
        }
        #endregion

        #region Properties
        protected abstract string actionName { get; }
        protected GodID Team => id == null ? GodID.Zeus : id.Team;
        protected bool IsCooldown => isCooldown;
        protected Vector2 Direction => movement == null ? Vector2.zero : movement.Direction;
        protected Vector2 TargetDirection => movement == null ? Vector2.zero : movement.TargetDirection;
        #endregion

        /// <summary>
        /// Setup input.
        /// </summary>
        protected virtual void Awake()
        {
            if (TryGetComponent(out PlayerInput input))
            {
                performAction = input.currentActionMap.FindAction(actionName);

                performAction.performed += OnActionInput;
            }
        }
        protected virtual void OnDestroy()
        {
            if (performAction != null)
            {
                performAction.performed -= OnActionInput;
            }
        }

        /// <summary>
        /// Called when the corresponding action key is pressed and checks for cooldown before delegating to child classes.
        /// </summary>
        /// <param name="context"></param>
        private void OnActionInput(InputAction.CallbackContext context)
        {
            if (!IsCooldown)
            {
                OnActionPerformedEvent?.Invoke();
                OnActionPerformed();
            }
        }

        /// <summary>
        /// Abstract function to implement the specific action the child class enables.
        /// </summary>
        protected abstract void OnActionPerformed();

        protected void Cooldown()
        {
            Cooldown(cooldown);
        }
        /// <summary>
        /// Prevents the player from using this action for a time.
        /// </summary>
        /// <param name="cooldownTime">The amount of time the player must cool down.</param>
        /// <returns></returns>
        protected void Cooldown(float cooldownTime)
        {
            if (isCooldown) { return; }
            OnCooldownEvent?.Invoke(cooldownTime);
            StartCoroutine(CooldownRoutine(cooldownTime));
        }
        private IEnumerator CooldownRoutine(float cooldownTime)
        {
            isCooldown = true;
            yield return new WaitForSeconds(cooldownTime);
            isCooldown = false;
            OnCooldownExpire?.Invoke();
        }
    }
}
