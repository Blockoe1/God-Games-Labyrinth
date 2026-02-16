/*****************************************************************************
// File Name : DebugState.cs
// Author : Brandon Koederitz
// Creation Date : 2/16/2026
// Last Modified : 2/16/2026
//
// Brief Description : State for debugging minotaur state transitions.
*****************************************************************************/
using UnityEngine;

namespace GGL.Minotaur
{
    public class DebugState : MinotaurState
    {
        public override void OnStateEnter(MinotaurController controller)
        {
            Debug.Log("Debug State Entered, childed to " + parent);
        }
        public override void OnStateExit(MinotaurController controller)
        {
            Debug.Log("Debug State Exited, childed to " + parent);
        }
    }
}
