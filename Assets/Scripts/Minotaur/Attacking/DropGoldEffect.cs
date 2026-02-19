/*****************************************************************************
// File Name : DropGoldEffect.cs
// Author : Brandon Koederitz
// Creation Date : 2/18/2026
// Last Modified : 2/18/2026
//
// Brief Description : Causes a miinotaur attack to cause the hit object to drop some gold.
*****************************************************************************/
using GGL.Scoring;
using UnityEngine;

namespace GGL.Minotaur
{
    public class DropGoldEffect : AttackEffect
    {
        [SerializeField, Tooltip("The percentage of the hit champion's gold to force them to drop."), Range(0, 1)] 
        private float dropProportion;

        /// <summary>
        /// Causes a collectable on the hit object to drop gold.
        /// </summary>
        /// <param name="hitAttackable"></param>
        public override void OnHit(Attackable hitAttackable)
        {
            if (hitAttackable.TryGetComponent(out Collector collector))
            {
                collector.DropCollectables(dropProportion);
            }
        }
    }
}
