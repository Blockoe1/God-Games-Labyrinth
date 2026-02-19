/*****************************************************************************
// File Name : ChampionMovementController.cs
// Author : Brandon Koederitz
// Creation Date : 1/26/2026
// Last Modified : 1/26/2026
//
// Brief Description : Controls player input for champion movement.
*****************************************************************************/
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGL.Champions
{
    [RequireComponent(typeof(PlayerInput))]
    public class ChampionMovement : EntityMovement
    {
        #region CONSTS
        private const string MOVE_ACTION_NAME = "Move";
        #endregion

        private InputAction moveAction;

        public event Action<bool> OnMove;

        #region Component References
        [SerializeReference, ReadOnly] private PlayerInput input;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References (Child)")]
        protected override void Reset()
        {
            base.Reset();
            input = GetComponent<PlayerInput>();
        }
        #endregion

        public override bool IsMoving 
        {
            set 
            {
                base.IsMoving = value;
                OnMove?.Invoke(value);
            }
        }

        /// <summary>
        /// Subscribe/Unsubscribe input.
        /// </summary>
        private void Awake()
        {
            moveAction = input.actions.FindAction(MOVE_ACTION_NAME);
            moveAction.performed += MoveAction_performed;
            moveAction.canceled += MoveAction_canceled;
        }
        private void OnDestroy()
        {
            moveAction.performed -= MoveAction_performed;
            moveAction.canceled -= MoveAction_canceled;
        }


        #region Input Functions
        /// <summary>
        /// Read player movement input.
        /// </summary>
        /// <param name="obj"></param>
        private void MoveAction_performed(InputAction.CallbackContext obj)
        {
            // Only take the X or Y Component for locked movement.
            Vector2Int input = MathHelpers.RoundVectorToInt(obj.ReadValue<Vector2>());
            TargetDirection = input;
            // Ignore diagonal movement
            //if (Mathf.Abs(input.x) != Mathf.Abs(input.y) || input == Vector2Int.zero)
            //{
            //    //Vector2Int inputDirection = Mathf.Abs(rawInput.y) > Mathf.Abs(rawInput.x) ? 
            //    //    Vector2Int.up * System.MathF.Sign(rawInput.y) : Vector2Int.right * System.MathF.Sign(rawInput.x);

            //    // Set the player's new direction and target speed.
            //    if (input != Vector2Int.zero)
            //    {
            //        Direction = input;
            //        IsMoving = true;
            //    }
            //    else
            //    {
            //        IsMoving = false;
            //    }
            //}
        }
        private void MoveAction_canceled(InputAction.CallbackContext obj)
        {
            IsMoving = false;
        }
        #endregion

        /// <summary>
        /// Forcibly rotates and applies speed to this champion to simulate knockback.
        /// </summary>
        /// <param name="direction">The direction to force the champion in.</param>
        /// <param name="force">The magnitude of the force.</param>
        public void ApplyKnockback(Vector2 direction, float force)
        {
            Direction = -direction;
            speed = -force;
        }
    }
}
