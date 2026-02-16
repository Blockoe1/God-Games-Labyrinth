/*****************************************************************************
// File Name : CompositeState.cs
// Author : Brandon Koederitz
// Creation Date : 2/15/2026
// Last Modified : 2/15/2026
//
// Brief Description : A special type of state that can have a collection of sub-states.
*****************************************************************************/
using UnityEngine;

namespace GGL.Minotaur
{
    public class CompositeState : MinotaurState
    {
        [SerializeReference, ClassDropdown(typeof(MinotaurState))] private MinotaurState[] subStates;

        private MinotaurState currentState;

        /// <summary>
        /// Delegate to sub-states to get components.
        /// </summary>
        /// <param name="minotaurGo"></param>
        public override void GetComponents(GameObject minotaurGo)
        {
            foreach(MinotaurState state in subStates)
            {
                state.GetComponents(minotaurGo);
            }
        }
    }
}
