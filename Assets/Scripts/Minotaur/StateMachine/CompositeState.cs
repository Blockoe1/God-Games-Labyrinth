/*****************************************************************************
// File Name : CompositeState.cs
// Author : Brandon Koederitz
// Creation Date : 2/15/2026
// Last Modified : 2/15/2026
//
// Brief Description : A special type of state that can have a collection of sub-states.
*****************************************************************************/
using NaughtyAttributes;
using System;
using UnityEngine;

namespace GGL.Minotaur
{
    [DropdownGroup("Composite States")]
    public class CompositeState : MinotaurState, IStateHandler
    {
        [SerializeReference, ClassDropdown(typeof(MinotaurState))] private MinotaurState[] subStates;

        private MinotaurState currentState;

        /// <summary>
        /// When the minotaur is validated, validate this state and all sub-states, with this composite state as the
        /// parent.
        /// </summary>
        /// <param name="minotaur">The MinotaurController component to get components from.</param>
        /// <param name="parent">The parent state handler of this state.</param>
        public sealed override void OnValidate(MinotaurController minotaur, IStateHandler parent)
        {
            base.OnValidate(minotaur, parent);
            foreach (MinotaurState state in subStates)
            {
                if (state == null) { continue; }
                state.OnValidate(minotaur, this);
            }
        }

        /// <summary>
        /// When this state is entered, act like minotaur enter state.
        /// </summary>
        /// <param name="controller"></param>
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            if (subStates.Length > 0)
            {
                SetState(subStates[0]);
            }
        }
        public override void OnStateExit()
        {
            base.OnStateExit();
            SetState(null);
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

        /// <summary>
        /// Sets the current minotaur state to a state of type T.
        /// </summary>
        /// <typeparam name="T">The type of state to transition to</typeparam>
        /// <returns>The new state.</returns>
        public T SetState<T>() where T : MinotaurState
        {
            // Only match exact types.
            T state = (T)Array.Find(subStates, item => item.GetType() == typeof(T));
            if (state == null)
            {
                Debug.LogWarning($"The StateHandler {this} does not have a state of type {typeof(T)}");
                return null;
            }
            SetState(state);
            return state;
        }
    }
}
