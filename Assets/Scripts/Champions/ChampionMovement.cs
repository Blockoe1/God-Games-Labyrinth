/*****************************************************************************
// File Name : ChampionMovementController.cs
// Author : Brandon Koederitz
// Creation Date : 1/26/2026
// Last Modified : 1/26/2026
//
// Brief Description : Controls player input for champion movement.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGL.Champions
{
    [RequireComponent(typeof(PlayerInput))]
    public class ChampionMovement : EntityMover
    {
        #region CONSTS
        private const string MOVE_ACTION_NAME = "Move";
        #endregion

        [SerializeField] private Transform snapTarget;
        private InputAction moveAction;

        #region Component References
        [SerializeReference, ReadOnly] private PlayerInput input;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        protected override void Reset()
        {
            base.Reset();
            input = GetComponent<PlayerInput>();
        }
        #endregion

        #region Properties
        public Transform SnapTarget
        {
            get { return snapTarget; }
            set { snapTarget = value; }
        }
        #endregion

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
            // Ignore diagonal movement
            if (Mathf.Abs(input.x) != Mathf.Abs(input.y) || input == Vector2Int.zero)
            {
                //Vector2Int inputDirection = Mathf.Abs(rawInput.y) > Mathf.Abs(rawInput.x) ? 
                //    Vector2Int.up * System.MathF.Sign(rawInput.y) : Vector2Int.right * System.MathF.Sign(rawInput.x);

                // Set the player's new direction and target speed.
                if (input != Vector2Int.zero)
                {
                    Direction = input;
                    IsMoving = true;
                }
                else
                {
                    IsMoving = false;
                }
            }
        }
        private void MoveAction_canceled(InputAction.CallbackContext obj)
        {
            IsMoving = false;
        }
        #endregion

        /// <summary>
        /// Snaps the player to a snap target if it's set.
        /// </summary>
        protected override void Snap()
        {
            if (snapTarget != null)
            {
                rb.MovePosition(snapTarget.position);
            }
            base.Snap();                
        }
    }
}
