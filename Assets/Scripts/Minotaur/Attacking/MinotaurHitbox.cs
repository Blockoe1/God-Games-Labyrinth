/*****************************************************************************
// File Name : HitboxRelay.cs
// Author : Brandon Koederitz
// Creation Date : 2/18/2026
// Last Modified : 2/18/2026
//
// Brief Description : Relays collision information to a parent MinotaurAttacker Component.
*****************************************************************************/
using GGL.Scoring;
using NaughtyAttributes;
using UnityEngine;

namespace GGL.Minotaur
{
    public class MinotaurHitbox : MonoBehaviour
    {
        [SerializeReference, ClassDropdown(typeof(AttackEffect))] private AttackEffect[] attackEffects;

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] protected MinotaurAttacker attacker;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        protected virtual void Reset()
        {
            attacker = GetComponentInParent<MinotaurAttacker>();
        }
        #endregion

        /// <summary>
        /// When a champion enters a hitbox, notify the parent MinotaurAttacker.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Attackable attackable))
            {
                attacker.HitObject(attackable, attackEffects);
            }
        }
    }
}
