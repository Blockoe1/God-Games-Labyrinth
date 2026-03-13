/*****************************************************************************
// File Name : MinotaurState.cs
// Author : Brandon Koederitz
// Creation Date : 2/13/2026
// Last Modified : 2/13/2026
//
// Brief Description : Abstract base class for states that control the minotaur's behavior.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace GGL.Minotaur
{
    [System.Serializable]
    public abstract class MinotaurState
    {
        [SerializeField] private Color debugColor;

        private Coroutine stateRoutine;
        [SerializeReference, HideInInspector] private IStateHandler parentState;

        protected bool isState { get; private set; }
        [field: SerializeField, HideInInspector] protected MinotaurController minotaur { get; private set; }

        #region Properties
        protected IStateHandler parent => parentState == null ? minotaur : parentState;
        #endregion

        /// <summary>
        /// Update this state with relevant references 
        /// </summary>
        /// <param name="minotaur"></param>
        /// <param name="parent"></param>
        public virtual void OnValidate(MinotaurController minotaur, IStateHandler parent)
        {
            this.parentState = parent;
            this.minotaur = minotaur;
            GetComponents();
        }

        /// <summary>
        /// Called on validate to get service components on the minotaur.
        /// </summary>
        public virtual void GetComponents() { }

        /// <summary>
        /// Called when the parent StateHandler enters this state.
        /// </summary>
        /// <param name="controller">The MinotaurController parent that this state belongs to.</param>
        public virtual void OnStateEnter() 
        { 
            isState = true;
            // Debug to visualize colors.
            minotaur.GetComponent<SpriteRenderer>().color = debugColor;
            // Start the coroutine for this state.
            stateRoutine = minotaur.StartCoroutine(StateRoutine());
        }

        /// <summary>
        /// Called when the parent StateHandler exits this state.
        /// </summary>
        /// <param name="controller">The MinotaurController parent that this state belongs to.</param>
        public virtual void OnStateExit() 
        {
            isState = false;
            if (stateRoutine != null)
            {
                minotaur.StopCoroutine(stateRoutine);
                stateRoutine = null;
            }
        }

        /// <summary>
        /// Continual coroutine that runs while this state is active.
        /// </summary>
        /// <param name="controller">The MinotaurController this state is managing.</param>
        /// <returns>Coroutine</returns>
        protected virtual IEnumerator StateRoutine()
        {
            // By default, the state routine immediately ends.
            stateRoutine = null;
            yield break;
        }
    }
}
