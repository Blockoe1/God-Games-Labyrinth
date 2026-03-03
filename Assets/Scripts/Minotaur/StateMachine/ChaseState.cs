/*****************************************************************************
// File Name : ChaseState.cs
// Author : Brandon Koederitz
// Creation Date : 2/17/2026
// Last Modified : 2/17/2026
//
// Brief Description : State for the minotaur chasing an aggroed player.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace GGL.Minotaur
{
    public class ChaseState : MinotaurState
    {
        [SerializeField] private float chaseSpeed;
        [SerializeField, Tooltip("If the minotaur fails to find a path to the chased player, wait this many " +
            "seconds before attempting to find a path again.")] 
        private float rePathDelay;

        private float baseSpeed;

        private AggroState aggroState => parent as AggroState;

        /// <summary>
        /// Setup event references when this state is entered.
        /// </summary>
        /// <param name="minotaur"></param>
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            // Set a custom movement speed for chases.
            baseSpeed = minotaur.movement.MaxSpeed;
            minotaur.movement.MaxSpeed = chaseSpeed;

            // When the minotaur reaches a new node along the path, auto-update the path so that it always
            // takes the most efficient route.
            minotaur.movement.OnReachNode += UpdatePath;
            // Set a default chase path.
            UpdatePath(Vector2.zero);
        }

        /// <summary>
        /// Unsubscribe event references.
        /// </summary>
        /// <param name="minotaur"></param>
        public override void OnStateExit()
        {
            base.OnStateExit();
            // Revert the minotaur's speed back to it's base.
            minotaur.movement.MaxSpeed = baseSpeed;
            minotaur.movement.OnReachNode -= UpdatePath;
        }

        /// <summary>
        /// Updates the minotaur's current path whenever a new node is reached.
        /// </summary>
        /// <param name="reachedNode">The new node that was reached.</param>
        private void UpdatePath(Vector2 reachedNode)
        {
            //Debug.Log(aggroState.AggroTarget);
            minotaur.movement.SetDestination(aggroState.AggroTarget.transform.position);
            // If a null path was found, delay for a bit and re-path;
            if (minotaur.movement.CurrentPath == null)
            {
                minotaur.StartCoroutine(RePathRoutine(rePathDelay));
            }
        }

        /// <summary>
        /// Delayed coroutine to check for a new path after a delay.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator RePathRoutine(float time)
        {
            yield return new WaitForSeconds(time);
            UpdatePath(Vector2.zero);
        }
    }
}
