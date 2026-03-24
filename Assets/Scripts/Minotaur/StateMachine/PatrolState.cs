/*****************************************************************************
// File Name : PatrolState.cs
// Author : Brandon Koederitz
// Creation Date : 2/17/2026
// Last Modified : 2/17/2026
//
// Brief Description : State for the minotaur patrolling around the maze.
*****************************************************************************/
using GGL.Scoring;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

namespace GGL.Minotaur
{
    public class PatrolState : MinotaurState
    {
        [SerializeField] private float visionDelay = 1f;
        [SerializeField] private float patrolTime = 15f;
        [SerializeField, Tooltip("If true, the minotaur will perform a jump attack instead of finding a new path " +
            "when the patrol timer expires.")]
        private bool jumpOnPatrolExpire;
        [SerializeField, ReadOnly, AllowNesting] private GameObject[] champions;

        private int patrolTarget;
        private float patrolTimer;

        /// <summary>
        /// Gets references to the champions to patrol to.
        /// </summary>
        public override void GetComponents()
        {
            champions = GameObject.FindGameObjectsWithTag("Player");
        }

        /// <summary>
        /// When the state is entered, set a starting path.
        /// </summary>
        /// <param name="controller"></param>
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            // When the minotaur finishes it's current path, get a new path.
            minotaur.movement.OnCompletePath += SetNewPatrolPath;

            // Set a starting patrol path.
            SetNewPatrolPath();
        }

        /// <summary>
        /// Stop movement when exiting the patrol state.
        /// </summary>
        /// <param name="controller"></param>
        public override void OnStateExit()
        {
            base.OnStateExit();
            minotaur.movement.Stop();
            minotaur.vision.OnChampionFound -= OnDetectChampion;
            // When the minotaur finishes it's current path, get a new path.
            minotaur.movement.OnCompletePath -= SetNewPatrolPath;
        }

        /// <summary>
        /// Only initialize vision after a delay.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator StateRoutine()
        {
            yield return new WaitForSeconds(visionDelay);
            // Setup the transition to the aggro state.
            minotaur.vision.OnChampionFound += OnDetectChampion;

            patrolTimer = 0;
            while(true)
            {
                // Continually re-find a path if we haven't finished the path to avoid bugs.
                while(patrolTimer <= patrolTime)
                {
                    patrolTimer += Time.deltaTime;
                    yield return null;
                }

                if(jumpOnPatrolExpire)
                {
                    JumpState jumpState = parent.GetState<JumpState>();
                    patrolTarget = (patrolTarget + 1) % champions.Length;
                    jumpState.Initialize(champions[patrolTarget].transform.position);
                    parent.SetState(jumpState);
                }
                else
                {
                    Debug.Log("Repathed");
                    SetNewPatrolPath();
                }
            }
        }

        #region Patrolling
        /// <summary>
        /// Gets a patrol path to a randomized piece of gold.
        /// </summary>
        /// <returns>The path from the minotaur's current position to the gold's position.</returns>
        private void SetNewPatrolPath()
        {
            //Vector2 destination = CollectableSpawner.Collectables[Random.Range(0, 
            //    CollectableSpawner.Collectables.Count)].transform.position;
            patrolTarget = (patrolTarget + 1) % champions.Length;
            Vector2 destination = champions[patrolTarget].transform.position;
            minotaur.movement.SetDestination(destination);
            // Only reset the timer if a jump attack is not set.
            if (!jumpOnPatrolExpire)
            {
                patrolTimer = 0;
            }
            // If no path was found, set a path for a random collectable instead.
            if (minotaur.movement.CurrentPath == null)
            {
                Collectable foundDest = CollectableSpawner.Collectables[Random.Range(0,
                CollectableSpawner.Collectables.Count)];
                if (foundDest != null)
                {
                    destination = foundDest.transform.position;
                }
                else
                {
                    destination = new Vector2(0, 3);
                }
                minotaur.movement.SetDestination(destination);
            }
        }
        #endregion

        #region Transitions
        /// <summary>
        /// When the minotaur sees a champion while patrolling, switch to the aggroed state.
        /// </summary>
        /// <param name="seenChampion">The champion the minotaur saw.</param>
        private void OnDetectChampion(GameObject seenChampion)
        {
            AggroState newState = parent.GetState<AggroState>();
            newState.SetAggroTarget(seenChampion);
            // Set the aggro target before setting the new state.
            parent.SetState(newState);
        }
        #endregion
    }
}
