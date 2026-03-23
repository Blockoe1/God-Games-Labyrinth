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
    public class AggroState : CompositeState
    {
        [SerializeField, Tooltip("The amount of time that the minotaur remains aggored to a specific champion")] 
        private float aggroTime;
        [SerializeField, Range(0, 100)] 
        private int aggroChangeChance;
        [SerializeField] private GameObject aggroEffects;

        private GameObject aggroTarget;
        private float aggroTimer;

        #region Properties
        internal GameObject AggroTarget => aggroTarget;
        #endregion

        /// <summary>
        /// Setup so that the minotaur can change targets if a new champion enters it's vision.
        /// </summary>
        /// <param name="minotaur"></param>
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            minotaur.vision.OnChampionFound += OnDetectChampion;
            minotaur.attacker.OnHitObject += OnHit;
            aggroEffects.SetActive(true);
        }
        public override void OnStateExit()
        {
            base.OnStateExit();
            minotaur.vision.OnChampionFound -= OnDetectChampion;
            minotaur.attacker.OnHitObject -= OnHit;
            aggroEffects.SetActive(false);
        }

        /// <summary>
        /// De-aggro on a champion if the minotaur hits them in any capacity.
        /// </summary>
        /// <param name="hitObject"></param>
        private void OnHit(Attackable hitObject)
        {
            // Switch back to the patrol state if the minotaur hit it's target.
            if (hitObject.gameObject == aggroTarget)
            {
                parent.SetState<PatrolState>();
            }
        }

        /// <summary>
        /// When a new champion enters the minotaur's vision, there is a chance they will aggro on them instead.
        /// </summary>
        /// <param name="newChampion"></param>
        private void OnDetectChampion(GameObject newChampion)
        {
            // Prevent double aggro.
            if (newChampion == aggroTarget) { return; }
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
        protected override IEnumerator StateRoutine()
        {
            // Continually wait until the aggro timer expires.
            while (aggroTimer > 0)
            {
                aggroTimer -= Time.deltaTime;
                yield return null;
            }

            // Only allow leaving the aggro state while in the base state.
            if (SubStates.Length > 0)
            {
                yield return new WaitUntil(() => currentState == SubStates[0]);
            }
            // Transition back to the patrol state.
            parent.SetState<PatrolState>();
        }
    }
}
