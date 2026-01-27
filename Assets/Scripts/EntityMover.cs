/*****************************************************************************
// File Name : EntityMover.cs
// Author : Brandon Koederitz
// Creation Date : 1/26/2026
// Last Modified : 1/26/2026
//
// Brief Description : Base movement script for moving an entity through the maze.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GGL
{
    public class EntityMover : MonoBehaviour
    {
        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private bool positionSnap;
        [SerializeField] private UnityEvent<Vector2> OnDirectionChanged;

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
                Vector2 oldDirection = direction;
                direction = value;
                OnDirectionChanged?.Invoke(direction);

                //Snap the entity's position to the grid when they change direction.
                if (positionSnap && Direction != oldDirection)
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
            speed = Mathf.MoveTowards(speed, isMoving ? maxSpeed : 0, acceleration * Time.fixedDeltaTime);
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
