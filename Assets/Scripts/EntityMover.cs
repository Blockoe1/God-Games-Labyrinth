/*****************************************************************************
// File Name : ChampionMovement.cs
// Author : Brandon Koederitz
// Creation Date : 1/26/2026
// Last Modified : 1/26/2026
//
// Brief Description : Handles base champion movement.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GGL
{
    public class EntityMover : MonoBehaviour
    {
        #region CONSTS
        private const string MOVE_ACTION_NAME = "Move";
        #endregion

        [Header("Movement settings")]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private bool positionSnap;
        [SerializeField] private UnityEvent<Vector2> OnDirectionChanged;

        private float targetSpeed;
        private float speed;
        private Vector2 direction = Vector2.up;
        private bool markForSnap;

        private bool isMoving;

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] private Rigidbody2D rb;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        protected virtual void Reset()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        #endregion

        #region Properties
        public bool IsMoving
        {
            get { return isMoving; }
            set { isMoving = value; }
        }

        public Vector2 Direction
        { 
            get { return direction; }
            set 
            { 
                direction = value;
                OnDirectionChanged?.Invoke(direction);
            }
        }
        #endregion

        #region Input Functions
        /// <summary>
        /// Read player movement input.
        /// </summary>
        /// <param name="obj"></param>
        private void MoveAction_performed(InputAction.CallbackContext obj)
        {
            // Only take the X or Y Component for locked movement.
            Vector2 rawInput = obj.ReadValue<Vector2>().normalized;
            Vector2 oldDirection = direction;
            Vector2 inputDirection = Mathf.Abs(rawInput.y) > Mathf.Abs(rawInput.x) ? Vector2.up * System.MathF.Sign(rawInput.y) :
                Vector2.right * System.MathF.Sign(rawInput.x);

            // Set the player's new direction and target speed.
            if (inputDirection != Vector2.zero)
            {
                direction = inputDirection;
                targetSpeed = maxSpeed;
            }
            else
            {
                targetSpeed = 0;
            }

            //Snap the player's position to the grid when they change direction.
            if (positionSnap && direction != oldDirection)
            {
                markForSnap = true;
            }

        }
        private void MoveAction_canceled(InputAction.CallbackContext obj)
        {
            targetSpeed = 0;
        }
        #endregion

        /// <summary>
        /// Control movement in FixedUpdate
        /// </summary>
        /// <remarks>
        /// Using FixedUpdate instead of a corutine because FixedUpdate happens before internal physics updates, while
        /// WaitForFixedUpdate happens after.
        /// </remarks>
        private void FixedUpdate()
        {
            speed = Mathf.MoveTowards(speed, targetSpeed, acceleration * Time.fixedDeltaTime);
            rb.linearVelocity = speed * direction;

            // Snap the player's position tot he grid when they change direction.
            if (markForSnap)
            {
                rb.MovePosition(MathHelpers.RoundVectorToInt(rb.position));
                markForSnap = false;
            }
        }
    }
}
