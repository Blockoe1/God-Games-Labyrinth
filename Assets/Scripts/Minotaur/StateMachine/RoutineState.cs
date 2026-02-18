/*****************************************************************************
// File Name : RoutineState.cs
// Author : Brandon Koederitz
// Creation Date : 2/17/2026
// Last Modified : 2/17/2026
//
// Brief Description : Abstract class for a state that utilizes a couroutine running throughout the state's duration.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace GGL.Minotaur
{
    public abstract class RoutineState : MinotaurState
    {
        private Coroutine stateRoutine;

        /// <summary>
        /// Start/Stop the state coroutine.
        /// </summary>
        /// <param name="controller"></param>
        public override void OnStateEnter(MinotaurController controller)
        {
            base.OnStateEnter(controller);
            stateRoutine = controller.StartCoroutine(StateRoutine(controller));
        }
        public override void OnStateExit(MinotaurController controller)
        {
            base.OnStateExit(controller);
            if (stateRoutine != null)
            {
                controller.StopCoroutine(stateRoutine);
                stateRoutine = null;
            }
        }

        /// <summary>
        /// Continual coroutine that runs while this state is active.
        /// </summary>
        /// <param name="controller">The MinotaurController this state is managing.</param>
        /// <returns>Coroutine</returns>
        protected abstract IEnumerator StateRoutine(MinotaurController controller);
    }
}
