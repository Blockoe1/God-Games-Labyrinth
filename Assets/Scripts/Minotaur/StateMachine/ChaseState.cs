/*****************************************************************************
// File Name : ChaseState.cs
// Author : Brandon Koederitz
// Creation Date : 2/17/2026
// Last Modified : 2/17/2026
//
// Brief Description : State for the minotaur chasing an aggroed player.
*****************************************************************************/
using UnityEngine;

namespace GGL.Minotaur
{
    public class ChaseState : MinotaurState
    {
        [SerializeField] private float chaseSpeed;

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
            Debug.Log(aggroState.AggroTarget);
            minotaur.movement.SetDestination(aggroState.AggroTarget.transform.position);
        }
    }
}
