/*****************************************************************************
// File Name : AggroedState.cs
// Author : Brandon Koederitz
// Creation Date : 2/17/2026
// Last Modified : 2/17/2026
//
// Brief Description : Composite state that manages all minotaur behaviours that involve chasing an aggroed player.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace GGL.Minotaur
{
    public class AggroedState : CompositeState
    {
        [SerializeField, Tooltip("The amount of time that the minotaur remains aggored to a specific champion")] 
        private float aggroTime;
        [SerializeField, Range(0, 100)] 
        private int aggroChangeChance;

        private GameObject aggroTarget;
        private float aggroTimer;

        /// <summary>
        /// Setup so that the minotaur can change targets if a new champion enters it's vision.
        /// </summary>
        /// <param name="minotaur"></param>
        public override void OnStateEnter(MinotaurController minotaur)
        {
            base.OnStateEnter(minotaur);
            minotaur.vision.OnChampionFound += OnDetectChampion;
        }
        public override void OnStateExit(MinotaurController minotaur)
        {
            base.OnStateExit(minotaur);
            minotaur.vision.OnChampionFound -= OnDetectChampion;
        }

        /// <summary>
        /// When a new champion enters the minotaur's vision, there is a chance they will aggro on them instead.
        /// </summary>
        /// <param name="newChampion"></param>
        private void OnDetectChampion(GameObject newChampion)
        {
            if (Random.Range(0, 100) > aggroChangeChance)
            {
                SetAggroTarget(newChampion);
            }
        }

        /// <summary>
        /// Sets a certain champion as the aggro target of the minotaur.
        /// </summary>
        /// <param name="champion"></param>
        public void SetAggroTarget(GameObject champion)
        {
            aggroTarget = champion;
            aggroTimer = aggroTime;
        }

        /// <summary>
        /// The aggro state continually counts a timer down and transitions back to the patrol state once the timer
        /// expires.
        /// </summary>
        /// <param name="minotaur"></param>
        /// <returns></returns>
        protected override IEnumerator StateRoutine(MinotaurController minotaur)
        {
            // Continually wait until the aggro timer expires.
            yield return new WaitUntil(() => aggroTimer < 0);

            // Transition back to the patrol state.
            parent.SetState<PatrolState>();
        }
    }
}
