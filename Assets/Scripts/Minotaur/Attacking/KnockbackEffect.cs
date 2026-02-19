/*****************************************************************************
// File Name : DropGoldEffect.cs
// Author : Brandon Koederitz
// Creation Date : 2/18/2026
// Last Modified : 2/18/2026
//
// Brief Description : Causes a minotaur attack to knock the hit object back.
*****************************************************************************/
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
            if (hitAttackable.TryGetComponent(out Rigidbody2D rb))
            {
                // Piggyback off the pathfinder movement GetDirection function to calculate the orthogonal direction.
                Vector2 knockbackDirection = PathfinderMovement.GetDirection(rb.position, attacker.transform.position);
                rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}
