/*****************************************************************************
// File Name : DropGoldEffect.cs
// Author : Brandon Koederitz
// Creation Date : 2/18/2026
// Last Modified : 2/18/2026
//
// Brief Description : Causes a minotaur attack to knock the hit object back.
*****************************************************************************/
using GGL.Champions;
using UnityEngine;

namespace GGL.Minotaur
{
    public class KnockbackEffect : AttackEffect
    {
        [SerializeField] private float knockbackForce;

        /// <summary>
        /// Applies orthogonal knockback to a hit object.
        /// </summary>
        /// <param name="hitAttackable"></param>
        public override void OnHit(Attackable hitAttackable, MinotaurAttacker attacker)
        {
            if (hitAttackable.TryGetComponent(out ChampionMovement champMove))
            {
                // Piggyback off the pathfinder movement GetDirection function to calculate the orthogonal direction.
                Vector2 knockbackDirection = PathfinderMovement.GetDirection(champMove.Rigidbody.position, 
                    attacker.transform.position);
                // Cant use rigidbody knockback since EntityMovement strictly manages speed.  Implementation must
                // go through the mover.
                champMove.ApplyKnockback(knockbackDirection, knockbackForce);
            }
        }
    }
}
