/*****************************************************************************
// File Name : JumpState.cs
// Author : Brandon Koederitz
// Creation Date : 3/22/2026
// Last Modified : 3/22/2026
//
// Brief Description : State for the minotaur jumping to a player if it patrols for too long to make it more menacing.
*****************************************************************************/
using UnityEngine;

namespace GGL.Minotaur
{
    public class JumpState : MinotaurState
    {
        [SerializeField] private GameObject landingTelegraph;

        private GameObject[] targets;

        /// <summary>
        /// Initializes the jump state with a set of possible jump targets.
        /// </summary>
        /// <param name="jumpTargets">An array of elidgeable jump targets.</param>
        public void Initialize(GameObject[] jumpTargets)
        {
            targets = jumpTargets;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}
