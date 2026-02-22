/*****************************************************************************
// File Name : ChampionMovementController.cs
// Author : Brandon Koederitz
// Creation Date : 2/22/2026
// Last Modified : 2/22/2026
//
// Brief Description : Animates a particle gameObject between two target points.
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace GGL.UI
{
    public class IndicatorParticles : MonoBehaviour
    {
        [Header("Pertcle Settings")]
        [SerializeField] private GameObject particlePrefab;
        [SerializeField, Tooltip("The amount of time the particle takes to reach it's destination.")]
        private float animationTime;
        [SerializeField] private float offsetAmplitude;
        [SerializeField, Tooltip("Controls the particle's motion towards it's target.  Adjust the curve to make " +
            "the particle move at different speeds as it moves towards the target.")] 
        private AnimationCurve toTargetCurve;
        [SerializeField, Tooltip("Controls the offset of the particle from it's path to it's target position.  " +
            "Adjusting the curve changes how the particle moves away.")] 
        private AnimationCurve offseCurve;

        private readonly Queue<GameObject> particlePool = new Queue<GameObject>();


        /// <summary>
        /// Plays a particle that animates bewteen the start and end positions.
        /// </summary>
        /// <param name="startPos">The starting position of the particle.</param>
        /// <param name="endPos">The ending position of the particle.</param>
        /// <param name="time">The time the animation takes.</param>
        /// <param name="callback">A callback to perform after the particle's animation is complete.</param>
        public void PlayParticle(Vector2 startPos, Vector2 endPos, float time, Action callback = null)
        {
            StartCoroutine(ParticleRoutine(startPos, endPos, time, callback));
        }

        public void PlayParticle(Vector2 startPos, Vector2 endPos, Action callback = null)
        {
            PlayParticle(startPos, endPos, animationTime, callback);
        }

        /// <summary>
        /// Animates the particle.
        /// </summary>
        /// <param name="startPos">The starting position of the particle.</param>
        /// <param name="endPos">The ending position of the particle.</param>
        /// <param name="time">The time the animation takes.</param>
        /// <param name="callback">A callback to perform after the particle's animation is complete.</param>
        /// <returns></returns>
        private IEnumerator ParticleRoutine(Vector2 startPos, Vector2 endPos, float time, Action callback = null)
        {
            // Prevent 0 time particles.
            if (time == 0)
            {
                callback?.Invoke();
                yield break;
            }

            GameObject particleGo = GetParticle();
            particleGo.SetActive(false);
            float timer = 0;
            float ampScale = UnityEngine.Random.Range(-1, 1);
            particleGo.transform.position = startPos;
            particleGo.SetActive(true);

            while (timer < time)
            {
                float normalizedTime = timer / time;
                Vector2 toVector = endPos - (Vector2)particleGo.transform.position;
                Vector2 perpVector = new Vector2(toVector.y, -toVector.x);
                Debug.Log(perpVector);

                Vector2 currentPos = Vector2.Lerp(startPos, endPos, toTargetCurve.Evaluate(normalizedTime));
                // Offsets the particle based on a random amount and the offset curve.
                currentPos = currentPos + ampScale * offsetAmplitude * offseCurve.Evaluate(normalizedTime) * perpVector.normalized;
                particleGo.transform.position = currentPos;

                timer += Time.deltaTime;
                yield return null;
            }

            callback?.Invoke();
            ReturnParticle(particleGo);
        }

        #region Particle Object Pooling
        /// <summary>
        /// Gets an unused particle from the particles pool.
        /// </summary>
        /// <returns>The queued particle.</returns>
        private GameObject GetParticle()
        {
            GameObject toReturn = particlePool.Count > 0 ? particlePool.Dequeue() :
                Instantiate(particlePrefab, transform);
            //toReturn.SetActive(false);
            return toReturn;
        }

        /// <summary>
        /// returns a particle to the particles object pool.
        /// </summary>
        /// <param name="particle"></param>
        private void ReturnParticle(GameObject particle)
        {
            particlePool.Enqueue(particle);
            //particle.SetActive(false);
        }
        #endregion
    }
}
