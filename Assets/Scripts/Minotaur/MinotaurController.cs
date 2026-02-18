/*****************************************************************************
// File Name : MinotaurController.cs
// Author : Brandon Koederitz
// Creation Date : 2/13/2026
// Last Modified : 2/15/2026
//
// Brief Description : Main control script for the minotaur that utilizes a state machine to swap between states.
*****************************************************************************/
using NaughtyAttributes;
using System;
using UnityEngine;

namespace GGL.Minotaur
{
    public class MinotaurController : MonoBehaviour, IStateHandler
    {
        [SerializeReference, ClassDropdown(typeof(MinotaurState))] private MinotaurState[] states;

        private MinotaurState currentState;

        #region Base Component References
        [field: SerializeReference, HideInInspector] internal Pathfinder pathfinder {  get; private set; }
        [field: SerializeReference, HideInInspector] internal EntityMovement movement { get; private set; }
        [field: SerializeReference, HideInInspector] internal MinotaurVision vision { get; private set; }
        #endregion

        /// <summary>
        /// Update all states with relevant values and references when this component is validated.
        /// </summary>
        private void OnValidate()
        {
            pathfinder = GetComponent<Pathfinder>();
            movement = GetComponent<EntityMovement>();
            vision = GetComponent<MinotaurVision>();
            foreach(MinotaurState state in states)
            {
                if (state == null) { continue; }
                state.OnValidate(this, this);
            }
        }

        /// <summary>
        /// Set the first state as the minotaur's starting state.
        /// </summary>
        private void Start()
        {
            SetState(states[0]);
        }

        /// <summary>
        /// Sets the current minotaur state to a state of type T.
        /// </summary>
        /// <typeparam name="T">The type of state to transition to</typeparam>
        /// <returns>The new state.</returns>
        public T SetState<T>() where T : MinotaurState
        {
            T state = (T)Array.Find(states, item => item.GetType() == typeof(T));
            SetState(state);
            return state;
        }

        /// <summary>
        /// Sets the current minotaur state.
        /// </summary>
        /// <param name="state">The state to set.</param>
        internal void SetState(MinotaurState state)
        {
            currentState?.OnStateExit();
            currentState = state;
            currentState?.OnStateEnter();
        }

        #region Debug
        [Button]
        private void SetDebug()
        {
            SetState<DebugState>();
        }
        [Button]
        private void SetComposite()
        {
            SetState<CompositeState>();
        }
        #endregion
    }
}
