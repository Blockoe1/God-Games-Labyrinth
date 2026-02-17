/*****************************************************************************
// File Name : PatrolState.cs
// Author : Brandon Koederitz
// Creation Date : 2/17/2026
// Last Modified : 2/17/2026
//
// Brief Description : State for the minotaur patrolling around the maze.
*****************************************************************************/
using UnityEngine;

namespace GGL.Minotaur
{
    public class PatrolState : MinotaurState
    {
        [SerializeReference, HideInInspector] private Pathfinder pathfinder;
        [SerializeReference, HideInInspector] private EntityMovement movement;

        private Vector2[] currentPath;

        /// <summary>
        /// Gets the components that this state requires.
        /// </summary>
        /// <param name="minotaur">The MinotaurController this state belongs to.</param>
        public override void GetComponents(MinotaurController minotaur)
        {
            base.GetComponents(minotaur);
            pathfinder = minotaur.GetComponent<Pathfinder>();
            movement = minotaur.GetComponent<EntityMovement>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public override void OnStateEnter(MinotaurController controller)
        {
            base.OnStateEnter(controller);

        }

        public override void OnStateExit(MinotaurController controller)
        {
            base.OnStateExit(controller);
        }

        private Vector2[] GetPatrolPath()
        {
            return null;
        }
    }
}
