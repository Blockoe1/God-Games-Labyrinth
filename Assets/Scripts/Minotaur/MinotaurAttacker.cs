/*****************************************************************************
// File Name : MinotaurHitbox.cs
// Author : Brandon Koederitz
// Creation Date : 2/18/2026
// Last Modified : 2/18/2026
//
// Brief Description : Damages champions that this object collides with by making them drop gold.
*****************************************************************************/
using GGL.Scoring;
using System;
using UnityEngine;

namespace GGL.Minotaur
{
    public class MinotaurAttacker : MonoBehaviour
    {
        public event Action<Collector> OnHitChampion;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            
        }
    }
}
