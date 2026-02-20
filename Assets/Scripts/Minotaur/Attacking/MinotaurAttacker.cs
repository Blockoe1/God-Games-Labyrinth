/*****************************************************************************
// File Name : MinotaurHitbox.cs
// Author : Brandon Koederitz
// Creation Date : 2/18/2026
// Last Modified : 2/18/2026
//
// Brief Description : Damages champions that this object collides with by making them drop gold.
*****************************************************************************/
using System;
using UnityEngine;

namespace GGL.Minotaur
{
    public class MinotaurAttacker : MonoBehaviour
    {
        [SerializeReference, ClassDropdown(typeof(AttackEffect))] private AttackEffect[] defaultAttackEffects;

        public event Action<Attackable> OnHitObject;

        /// <summary>
        /// Cause the hit champion to drop some gold when hit.
        /// </summary>
        /// <param name="hitObject">The champion that was hit.</param>
        /// <param name="hitbox">The hitbox that hit them.</param>
        internal void HitObject(Attackable hitObject, AttackEffect[] addedAttackEffects)
        {
            // Perform minotaur attack logic here.
            AttackEffect[] totalAttackEffects = new AttackEffect[defaultAttackEffects.Length + addedAttackEffects.Length];
            defaultAttackEffects.CopyTo(totalAttackEffects, 0);
            addedAttackEffects.CopyTo(totalAttackEffects, defaultAttackEffects.Length);

            // Apply each attack effect.
            foreach(AttackEffect attackEffect in totalAttackEffects)
            {
                attackEffect.OnHit(hitObject, this);
            }

            // Notify the attackable that it was hit.
            hitObject.OnHit();

            OnHitObject?.Invoke(hitObject);
        }
    }
}
