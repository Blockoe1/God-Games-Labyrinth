/*****************************************************************************
// File Name : MinotaurState.cs
// Author : Brandon Koederitz
// Creation Date : 2/13/2026
// Last Modified : 2/13/2026
//
// Brief Description : Abstract base class for states that control the minotaur's behavior.
*****************************************************************************/
using UnityEngine;

namespace GGL.Minotaur
{
    [System.Serializable]
    public abstract class MinotaurState
    {
        protected IStateHandler parent { get; private set; }

        /// <summary>
        /// Update this state with relevant references 
        /// </summary>
        /// <param name="minotaur"></param>
        /// <param name="parent"></param>
        public virtual void OnValidate(MinotaurController minotaur, IStateHandler parent)
        {
            this.parent = parent;
            GetComponents(minotaur);
        }

        /// <summary>
        /// Called on validate to get service components on the minotaur.
        /// </summary>
        public virtual void GetComponents(MinotaurController minotaur) { }

        /// <summary>
        /// Called when the parent StateHandler enters this state.
        /// </summary>
        /// <param name="controller">The MinotaurController parent that this state belongs to.</param>
        public virtual void OnStateEnter(MinotaurController controller) { }

        /// <summary>
        /// Called when the parent StateHandler exits this state.
        /// </summary>
        /// <param name="controller">The MinotaurController parent that this state belongs to.</param>
        public virtual void OnStateExit(MinotaurController controller) { }
    }
}
