/*****************************************************************************
// File Name : MinotaurController.cs
// Author : Brandon Koederitz
// Creation Date : 2/13/2026
// Last Modified : 2/15/2026
//
// Brief Description : Main control script for the minotaur that utilizes a state machine to swap between states.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;

namespace GGL.Minotaur
{
    public class MinotaurController : MonoBehaviour
    {
        [SerializeReference, ClassDropdown(typeof(MinotaurState))] private MinotaurState[] states;

        private MinotaurState currentState;

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] private EntityMovement movement;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        protected virtual void Reset()
        {
            movement = GetComponent<EntityMovement>();

            // Have sub states get components on this object.
            foreach(MinotaurState state in states)
            {
                state.GetComponents(gameObject);
            }
        }
        #endregion

        /// <summary>
        /// Initialize the minotaur.
        /// </summary>
        private void Awake()
        {
            movement.OnDetectDirection += ProcessDirection;
        }

        /// <summary>
        /// Unsubscribe events.
        /// </summary>
        private void OnDestroy()
        {
            movement.OnDetectDirection -= ProcessDirection;
        }

        /// <summary>
        /// Set the first state as the minotaur's starting state.
        /// </summary>
        private void Start()
        {
            currentState = states[0];
        }

        /// <summary>
        /// Handles logic when a new possible direction to move in is detected.
        /// </summary>
        /// <param name="direction">The direction to move in.</param>
        private void ProcessDirection(Vector2 direction)
        {
            // Check if the found direction is perpendicular to the current direction.
            if (direction != movement.Direction && direction != -movement.Direction)
            {

            }
        }
    }
}
