/*****************************************************************************
// File Name : CollectionParticles.cs
// Author : Brandon Koederitz
// Creation Date : 2/22/2026
// Last Modified : 2/22/2026
//
// Brief Description : Displays increasing particle effects based on 
*****************************************************************************/
using GGL.Scoring;
using NaughtyAttributes;
using UnityEngine;

namespace GGL.Champions
{
    [RequireComponent(typeof(Collector))]
    public class GoldParticles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem goldParticles;
        [SerializeField] private float maxEmission = 5;
        //[SerializeField] private float maxLifetime = 2;

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] protected Collector collector;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        protected virtual void Reset()
        {
            collector = GetComponent<Collector>();
        }
        #endregion

        /// <summary>
        /// Setup event reference with collector.
        /// </summary>
        private void Awake()
        {
            collector.AddOnCollectListener(SetGoldCount);
        }
        private void OnDestroy()
        {
            collector.RemoveOnCollectListener(SetGoldCount);
        }

        /// <summary>
        /// Sets the displayed gold count.
        /// </summary>
        /// <param name="goldCount"></param>
        public void SetGoldCount(int goldCount)
        {
            // Calculate the emission count based on the max and the normalized gold amount.
            float normalizedGold = (float)goldCount / collector.GoldCapacity;

            // Set the emission rate.
            var emission = goldParticles.emission;
            emission.rateOverTime = Mathf.Lerp(0, maxEmission, normalizedGold);

            // Set the lifetime.
            //var main = goldParticles.main;
            //main.startLifetime = Mathf.Lerp(0, maxLifetime, normalizedGold);
        }
    }
}
