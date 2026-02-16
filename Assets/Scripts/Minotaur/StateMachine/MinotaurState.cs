/*****************************************************************************
// File Name : MinotaurState.cs
// Author : Brandon Koederitz
// Creation Date : 2/13/2026
// Last Modified : 2/13/2026
//
// Brief Description : Abstract base class for states that control the minotaur's behavior.
*****************************************************************************/
using UnityEngine;

namespace GGL.Minotaur
{
    [System.Serializable]
    public abstract class MinotaurState
    {
        /// <summary>
        /// Called on component reset to automatically get components on the 
        /// </summary>
        public virtual void GetComponents(GameObject minotaurGo) { }

        public virtual void OnStateEnter() { }

        public virtual void OnStateExit() { }
    }
}
