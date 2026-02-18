/*****************************************************************************
// File Name : PatrolState.cs
// Author : Brandon Koederitz
// Creation Date : 2/17/2026
// Last Modified : 2/17/2026
//
// Brief Description : State for the minotaur patrolling around the maze.
*****************************************************************************/
using GGL.Scoring;
using System.Collections;
using UnityEngine;

namespace GGL.Minotaur
{
    public class PatrolState : MinotaurState
    {
        #region CONSTS
        private const float REQUIRED_PATH_DIST = 0.5f;
        #endregion

        private Vector2[] currentPath;
        private int currentPathNode;

        /// <summary>
        /// When the state is entered, set a starting path.
        /// </summary>
        /// <param name="controller"></param>
        public override void OnStateEnter()
        {
            SetNewPatrolPath(minotaur.pathfinder);
            // The minotaur is moving while in this state.
            minotaur.movement.IsMoving = true;
            base.OnStateEnter();

            // Setup the transition to the aggro state.
            minotaur.vision.OnChampionFound += OnDetectChampion;
        }

        /// <summary>
        /// Stop movement when exiting the patrol state.
        /// </summary>
        /// <param name="controller"></param>
        public override void OnStateExit()
        {
            base.OnStateExit();
            minotaur.movement.IsMoving = false;
            minotaur.vision.OnChampionFound -= OnDetectChampion;
        }

        #region Patrolling
        /// <summary>
        /// Continually check if the minotaur has reached the next point in their patrol path, and update 
        /// target direction.
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        protected override IEnumerator StateRoutine()
        {
            while (true)
            {
                // Pause the state if there is no valid paths.
                if (currentPath == null) { continue; }

                Pathfinder.DrawPath(currentPath);

                // If we've reached the next node in the path, update the target direction and move to the next node.
                if (Vector2.Distance(minotaur.movement.Rigidbody.position, currentPath[currentPathNode]) < REQUIRED_PATH_DIST)
                {
                    currentPathNode++;

                    // Get a new path if we've reached the end of this current path.
                    if (currentPathNode >= currentPath.Length)
                    {
                        SetNewPatrolPath(minotaur.pathfinder);
                        continue;
                    }

                    minotaur.movement.TargetDirection = Pathfinder.GetDirection(currentPath[currentPathNode], 
                        currentPath[currentPathNode - 1]);
                }

                // Check patrol on FixedUpdate as the game uses physics movement.
                yield return new WaitForFixedUpdate();
            }
        }

        /// <summary>
        /// Gets a patrol path to a randomized piece of gold.
        /// </summary>
        /// <returns>The path from the minotaur's current position to the gold's position.</returns>
        private void SetNewPatrolPath(Pathfinder pathfinder)
        {
            Vector2 destination = CollectableSpawner.Collectables[Random.Range(0, 
                CollectableSpawner.Collectables.Count)].transform.position;
            currentPath = pathfinder.FindPath(destination);
            currentPathNode = 0;
        }
        #endregion

        #region Transitions
        /// <summary>
        /// When the minotaur sees a champion while patrolling, switch to the aggroed state.
        /// </summary>
        /// <param name="seenChampion">The champion the minotaur saw.</param>
        private void OnDetectChampion(GameObject seenChampion)
        {
            AggroedState newState = parent.SetState<AggroedState>();
            newState.SetAggroTarget(seenChampion);
        }
        #endregion
    }
}
