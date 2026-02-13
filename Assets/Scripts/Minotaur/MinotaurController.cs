/*****************************************************************************
// File Name : MinotaurController.cs
// Author : Brandon Koederitz
// Creation Date : 2/13/2026
// Last Modified : 2/13/2026
//
// Brief Description :Main control script for the minotaur that utilizes a state machine to swap between states.
*****************************************************************************/
using GGL.Champions;
using NaughtyAttributes;
using System.Collections.Specialized;
using UnityEngine;

namespace GGL.Minotaur
{
    public class MinotaurController : MonoBehaviour
    {
        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] private EntityMovement movement;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References: 0")]
        protected virtual void Reset()
        {
            movement = GetComponent<EntityMovement>();
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
