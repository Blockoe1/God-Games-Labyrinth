/*****************************************************************************
// File Name : EntityMover.cs
// Author : Brandon Koederitz
// Creation Date : 1/26/2026
// Last Modified : 1/26/2026
//
// Brief Description : Base movement script for moving an entity through the maze.
*****************************************************************************/
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGL
{
    public class EntityMovement : MonoBehaviour
    {
        #region CONSTS
        private static readonly Vector2[] MOVEMENT_DIRECTIONS = new Vector2[]
        {
            Vector2.right,
            Vector2.up, 
            Vector2.down, 
            Vector2.left
        };
        #endregion

        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration;
        [SerializeField, Tooltip("The amount of empty space that the entity needs before it can turn in a given " +
    "direction.")]
        private float maxWallCheckDistance;
        [SerializeField] private bool positionSnap;
        [SerializeField] private UnityEvent<Vector2> OnDirectionChanged;

        // The actual direction that the object is facing.
        private Vector2 direction = Vector2.up;
        // THe direction that the ojbect is trying to move in.  Can be 0.
        private Vector2 targetDirection;

        private float speed;
        private bool markForSnap;

        private bool isMoving;

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] protected Rigidbody2D rb;

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
        public virtual bool IsMoving
        {
            get { return isMoving; }
            set { isMoving = value; }
        }
        public Vector2 TargetDirection
        {
            get { return targetDirection; }
            set 
            { 
                targetDirection = value;

                // If 0 is set as the target direction, then the objecct stops moving.
                if (targetDirection == Vector2.zero)
                {
                    IsMoving = false;
                }
                else
                {
                    IsMoving = true;
                }
            }
        }
        public Vector2 Direction
        { 
            get { return direction; }
            private set 
            {
                // Prevent assigning a direction of 0.
                if (value == Vector2.zero) { return; }

                Vector2 oldDirection = direction;
                direction = value;
                OnDirectionChanged?.Invoke(direction);

                //Snap the entity's position to the grid when they change direction.
                if (positionSnap && MathHelpers.V2Abs(oldDirection) == MathHelpers.V2Abs(Vector2.Perpendicular(direction)))
                {
                    markForSnap = true;
                }
            }
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
            if (IsMoving)
            {
                // Use a raycast to determine valid directions.
                foreach (var direction in MOVEMENT_DIRECTIONS)
                {
                    RaycastHit2D ray = Physics2D.Raycast(rb.position, direction, maxWallCheckDistance, GGLHelpers.MazeMask);
                    Debug.DrawRay(rb.position, direction * maxWallCheckDistance, Color.green);
                    // If the raycast hit nothing, this is a valid direction.
                    if (!ray && TargetDirection == direction)
                    {
                        // Change direction if the target direction is this valid direction.
                        Direction = direction;
                        break;
                    }
                }
            }

            speed = Mathf.MoveTowards(speed, IsMoving ? maxSpeed : 0, acceleration * Time.fixedDeltaTime);
            rb.linearVelocity = speed * Direction;

            // Snap the player's position tot he grid when they change direction.
            if (markForSnap)
            {
                Snap();
            }
        }

        /// <summary>
        /// Snaps the object to an integer grid.
        /// </summary>
        protected virtual void Snap()
        {
            rb.MovePosition((Vector2)MathHelpers.RoundVectorToInt(rb.position));
            markForSnap = false;
        }
    }
}
