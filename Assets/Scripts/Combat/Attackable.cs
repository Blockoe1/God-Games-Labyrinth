/*****************************************************************************
// File Name : MinotaurAttackable.cs
// Author : Brandon Koederitz
// Creation Date : 2/18/2026
// Last Modified : 2/18/2026
//
// Brief Description : Allows another object to be damaged by various attacks.
*****************************************************************************/
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GGL
{
    public class Attackable : MonoBehaviour
    {
        [SerializeField, Tooltip("The amount of time in seconds that this object cannot be attacked for after " +
            "being hit.")] 
        private float iFrames; 
        [SerializeField] private UnityEvent OnHitEvent;
        [SerializeField] private UnityEvent OnIFrameExpire;

        private bool isInvincible;

        #region Properties
        public bool IsInvincible => isInvincible;
        #endregion

        /// <summary>
        /// Called by the MinotaurAttacker when this object is hit by the minotaur's hitbox to handle universal
        /// behaviour that happens when the object is hit.
        /// </summary>
        internal void OnHit()
        {
            // Only allow hits if the attack target isn't invincible.
            if (IsInvincible) { return; }
            OnHitEvent?.Invoke();

            // Give the attacked object IFrames.
            if (iFrames > 0)
            {
                StartCoroutine(IFrames(iFrames));
            }
        }

        /// <summary>
        /// Prevents the collector from dropping collectables again after they've been forced to drop collectables.
        /// </summary>
        /// <param name="seconds">The amount of invulnerability time the champion has.</param>
        /// <returns>cCoroutine</returns>
        private IEnumerator IFrames(float seconds)
        {
            isInvincible = true;
            yield return new WaitForSeconds(seconds);
            isInvincible = false;
            OnIFrameExpire?.Invoke();
        }
    }
}
