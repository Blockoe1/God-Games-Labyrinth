/*****************************************************************************
// File Name : HeldGoldCounter.cs
// Author : Brandon Koederitz
// Creation Date : 2/19/2026
// Last Modified : 2/19/2026
//
// Brief Description : Tracks the amount of gold held by a champion.
*****************************************************************************/
using GGL.Scoring;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GGL.UI.ChampionUI
{
    // Glue
    public class HeldGoldCounter : ChampionUIService
    {
        [SerializeField] private TieredSprite goldSprite;
        [SerializeField] private Transform depositParticleTarget;
        [SerializeField] private IndicatorParticles particles;
        [SerializeField] private Image fillImage;
        [SerializeField] private Image[] flashingImages;
        [SerializeField] private Color flashColor;
        [SerializeField] private float flashPeriod;
        [SerializeField] private float goldDecreaseTime = 1f;
        [SerializeField] private float goldDepositePeriod;
        [SerializeField] private UnityEvent OnGoldFull;
        //[SerializeField] private TMP_Text pointsText;

        private Collector collector;

        private Coroutine animateRoutine;

        private int lastHeld;
        private bool isFull;

        /// <summary>
        /// Get a reference to the collector on the found champion.
        /// </summary>
        /// <param name="champion"></param>
        public override void Initialize(GodIdentifier champion)
        {
            collector = champion.GetComponent<Collector>();
            collector.OnCollectEvent.AddListener(Collector_OnCollect);
            collector.OnDepositEvent.AddListener(Collector_OnDeposit);
            collector.OnDropEvent.AddListener(Collector_OnDrop);
        }

        /// <summary>
        /// When gold is collected, play a particle and updatea the collectables shown when it finishes.
        /// </summary>
        /// <param name="heldCount">The total amount of gold the champion is holding.</param>
        private void Collector_OnCollect(int heldCount)
        {
            //particles.PlayParticle(collector.transform.position, transform.position, () => UpdateCollectables(heldCount));
            UpdateCollectables(heldCount);
        }

        /// <summary>
        /// When gold is deposited, play an animation of the gold going down and play particles to the deposit
        /// location.
        /// </summary>
        /// <param name="heldCount"></param>
        private void Collector_OnDeposit(int heldCount)
        {
            // Animate the gold decreasing.
            if (animateRoutine != null)
            {
                StopCoroutine(animateRoutine);
                animateRoutine = null;
            }
            StartCoroutine(DepositRoutine(heldCount));
        }

        /// <summary>
        /// Animate the gold going down over time when dropped.
        /// </summary>
        /// <param name="heldCount"></param>
        private void Collector_OnDrop(int heldCount)
        {
            if (animateRoutine != null)
            {
                StopCoroutine(animateRoutine);
                animateRoutine = null;
            }
            animateRoutine = StartCoroutine(GoldAnimateWrapper(heldCount));
        }

        /// <summary>
        /// Animates gold being desposited in the champion's room
        /// </summary>
        /// <param name="heldCount">The new amount of gold held.</param>
        /// <returns></returns>
        private IEnumerator DepositRoutine(int heldCount)
        {
            animateRoutine = StartCoroutine(GoldAnimateWrapper(heldCount));
            // While the animate routine is happening, play periodic particles showing gold being deposited.
            while (animateRoutine != null)
            {
                particles.PlayParticle(transform.position, depositParticleTarget.position);
                yield return new WaitForSeconds(goldDepositePeriod);
            }
        }

        /// <summary>
        /// Wrapper coroutine that manages the AnimateRoutine reference for animating the held gold int.
        /// </summary>
        /// <param name="heldCount"></param>
        /// <returns></returns>
        private IEnumerator GoldAnimateWrapper(int heldCount)
        {
            yield return StartCoroutine(GGLHelpers.AnimateInt(UpdateCollectables, goldDecreaseTime, lastHeld, heldCount));
            animateRoutine = null;
        }

        /// <summary>
        /// Updates the displayed number and point value of held collectables.
        /// </summary>
        /// <param name="heldCount"></param>
        private void UpdateCollectables(int heldCount)
        {
            // Update the gold sprite based on the proportion of the champion's held amount and capacity.
            float normalizedGold = collector.GoldCapacity > 0 ? (float)heldCount / collector.GoldCapacity : 0;
            if (fillImage != null)
            {
                fillImage.fillAmount = normalizedGold;
            }
            if (goldSprite != null)
            {
                goldSprite.SpriteAmount = normalizedGold;
            }
            if (normalizedGold == 1)
            {
                if (!isFull)
                {
                    OnGoldFull?.Invoke();

                    if (flashingImages != null)
                    {
                        foreach (var image in flashingImages)
                        {
                            StartCoroutine(FlashRoutine(image));
                        }
                    }
                }
            }
            else
            {
                isFull = false;
            }
            lastHeld = heldCount;
        }

        /// <summary>
        /// Makes the outline around the gold purse flash white.
        /// </summary>
        /// <param name="flashingImage"></param>
        /// <returns></returns>
        private IEnumerator FlashRoutine(Image flashingImage)
        {
            Color baseColor = flashingImage.color;
            isFull = true;
            while(isFull)
            {
                flashingImage.color = flashColor;
                yield return new WaitForSeconds(flashPeriod);
                flashingImage.color = baseColor;
                yield return new WaitForSeconds(flashPeriod);
            }
            flashingImage.color = baseColor;
        }

        /// <summary>
        /// Cleanup events.
        /// </summary>
        private void OnDestroy()
        {
            collector.OnCollectEvent.RemoveListener(Collector_OnCollect);
            collector.OnDepositEvent.RemoveListener(Collector_OnDeposit);
            collector.OnDropEvent.RemoveListener(Collector_OnDrop);
        }
    }
}
