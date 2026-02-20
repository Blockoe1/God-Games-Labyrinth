/*****************************************************************************
// File Name : CooldownIndicator.cs
// Author : Brandon Koederitz
// Creation Date : 2/19/2026
// Last Modified : 2/19/2026
//
// Brief Description : visualizes a champion action's cooldown via an image fill.
*****************************************************************************/
using GGL.Champions;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GGL.UI.ChampionUI
{
    public class CooldownIndicator : ChampionUIService
    {
        [SerializeField] private string targetTypeName;
        [SerializeField] private Image fillImage;


        private ChampionBehavior targetBehaviour;

        /// <summary>
        /// Get the target behaviour of this cooldown.
        /// </summary>
        /// <param name="champion"></param>
        public override void Initialize(GodIdentifier champion)
        {
            targetBehaviour = champion.GetComponents<ChampionBehavior>()
                .Where(x => x.GetType().Name == targetTypeName).FirstOrDefault();
            targetBehaviour.OnCooldownEvent += StartCooldown;
            targetBehaviour.OnActionPerformedEvent += SetInactive;
        }

        /// <summary>
        /// Sets the fill image to 0 to show the ability is not useable.
        /// </summary>
        private void SetInactive()
        {
            fillImage.fillAmount = 0;
        }

        /// <summary>
        /// Starts a coroutine that animates the cooldown icon.
        /// </summary>
        /// <param name="cooldown">The time in seconds of the cooldown.</param>
        private void StartCooldown(float cooldown)
        {
            StartCoroutine(CooldownRoutine(cooldown));
        }
        private IEnumerator CooldownRoutine(float cooldown)
        {
            float timer = 0;
            while (timer <= cooldown)
            {
                float normalizedTime = timer / cooldown;
                fillImage.fillAmount = normalizedTime;

                timer += Time.deltaTime;
                yield return null;
            }
            // Fill the image completely at the end of the cooldown.
            fillImage.fillAmount = 1;
        }

        /// <summary>
        /// Clean up event references.
        /// </summary>
        private void OnDestroy()
        {
            targetBehaviour.OnCooldownEvent -= StartCooldown;
        }
    }
}
