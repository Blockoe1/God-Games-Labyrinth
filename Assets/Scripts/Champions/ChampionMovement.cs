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
using System.Collections;
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

        [SerializeField] private float knockbackTime;
        [SerializeField] private bool autoTurn;

        private InputAction moveAction;

        public event Action<bool> OnMove;
        private bool suspendInput;

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
            if (!suspendInput)
            {
                Vector2Int input = MathHelpers.RoundVectorToInt(moveAction.ReadValue<Vector2>());
                TargetDirection = input;
            }
        }
        private void MoveAction_canceled(InputAction.CallbackContext obj)
        {
            IsMoving = false;
        }
        #endregion
        /// <summary>
        /// Perform additional checks for auto-turning.
        /// </summary>
        protected override void OnFixedUpdate()
        {
            if (autoTurn && IsMoving && TargetDirection == Direction)
            {
                // Check for if the champion is running into a wall.
                RaycastHit2D isFacingWall = Physics2D.Raycast(rb.position, Direction, MaxWallCheckDistance, GGLHelpers.MazeMask);
                if (isFacingWall)
                {
                    // Check for valid directions to auto-turn.
                    Vector2 perpVector = new Vector2(Direction.y, -Direction.x);
                    for (int i = 1; i >= -1; i -= 2)
                    {
                        // Check both perpendicular directions
                        perpVector = perpVector * i;

                        RaycastHit2D isWallCheck = Physics2D.Raycast(rb.position, perpVector,
                            MaxWallCheckDistance, GGLHelpers.MazeMask);
                        if (!isWallCheck)
                        {
                            RaycastHit2D isHallCheck = Physics2D.Raycast(rb.position + (perpVector), TargetDirection,
                                MaxWallCheckDistance, GGLHelpers.MazeMask);
                            Debug.DrawRay(rb.position + (perpVector), Direction * MaxWallCheckDistance, Color.green);
                            // If this direction is available, auto-turn in that direction.
                            if (!isHallCheck)
                            {
                                Debug.Log($"Found {perpVector} direction to auto-turn");
                                Direction = perpVector;
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Forcibly rotates and applies speed to this champion to simulate knockback.
        /// </summary>
        /// <param name="direction">The direction to force the champion in.</param>
        /// <param name="force">The magnitude of the force.</param>
        public void ApplyKnockback(Vector2 direction, float force)
        {
            Direction = -direction;
            speed = -force;
            StartCoroutine(SuspendInput(knockbackTime));
        }

        private IEnumerator SuspendInput(float time)
        {
            if (suspendInput) { yield break; }
            suspendInput = true;
            yield return new WaitForSeconds(time);
            suspendInput = false;
            // Check input after suspension.
            MoveAction_performed(new InputAction.CallbackContext());
        }
    }
}
