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

        private AggroedState aggroState => parent as AggroedState;

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

            // While in the chase state, the minotaur is moving.
            minotaur.movement.IsMoving = true;
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

            // Reset the minotaur's movement state.
            minotaur.movement.IsMoving = false;
        }

        /// <summary>
        /// When the minotaur encounters a fork in the road, perform a pathfind to find the optimal path to the
        /// aggroed target.
        /// </summary>
        /// <param name="detectedDirection">The direction of new movement.</param>
        private void Movement_OnDetectDirection(Vector2 detectedDirection)
        {
            
            Vector2[] optimalPath = minotaur.pathfinder.FindPath(aggroState.AggroTarget.transform.position);
            // Do nothing if the optimal path isn't long enough.
            if (optimalPath.Length >= 2)
            {
                Vector2 optimalDirection = Pathfinder.GetDirection(optimalPath[1], optimalPath[0]);
                Pathfinder.DrawPath(optimalPath, 1f);
                //Debug.Log($"Direction {detectedDirection} found.  The optimal direction is {optimalDirection}.  The optimal path points are: {optimalPath[0]} {optimalPath[1]}");
                // Only allow turning if it's the found direction or backwards.
                if (optimalDirection == detectedDirection ||
                    optimalDirection == -minotaur.movement.Direction)
                {
                    minotaur.movement.TargetDirection = optimalDirection;
                }
            }
        }
    }
}
