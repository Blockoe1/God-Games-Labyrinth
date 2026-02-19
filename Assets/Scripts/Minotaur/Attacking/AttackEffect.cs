/*****************************************************************************
// File Name : AttackEffect.cs
// Author : Brandon Koederitz
// Creation Date : 2/18/2026
// Last Modified : 2/18/2026
//
// Brief Description : Base class that defines an effect of a minotaur's attack.
*****************************************************************************/
using UnityEngine;

namespace GGL.Minotaur
{
    [System.Serializable]
    public abstract class AttackEffect
    {
        public abstract void OnHit(Attackable hitAttackable);
    }
}
