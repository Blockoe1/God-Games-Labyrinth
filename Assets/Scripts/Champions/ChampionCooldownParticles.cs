/*****************************************************************************
// File Name : ChampionCooldownParticles.cs
// Author : Brandon Koederitz
// Creation Date : 3/29/2026
// Last Modified : 3/29/2026
//
// Brief Description : Plays a particle effect when a champion's abilitiy comes off cooldown
*****************************************************************************/
using UnityEngine;

namespace GGL.Champions
{
    public class ChampionCooldownParticles : MonoBehaviour
    {
        [SerializeField] private ChampionBehavior ability;
        [SerializeField] private ParticleSystem particles;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
