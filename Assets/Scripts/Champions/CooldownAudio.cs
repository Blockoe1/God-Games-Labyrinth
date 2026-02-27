/*****************************************************************************
// File Name : CooldownAudio.cs
// Author : Brandon Koederitz
// Creation Date : 2/25/2026
// Last Modified : 2/25/2026
//
// Brief Description : Plays a sound when a champion comes off cooldown.
*****************************************************************************/
using GGL.Audio;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

namespace GGL.Champions
{
    // Glue
    public class CooldownAudio : MonoBehaviour
    {
        [SerializeField] private ChampionBehavior cooldownBehavior;
        [SerializeField] private string soundName;

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] private AudioRelay audioRelay;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References: 1")]
        private void Reset()
        {
            audioRelay = GetComponent<AudioRelay>();
        }
        #endregion

        private void Awake()
        {
            cooldownBehavior.OnCooldownEvent += ScheduleCooldownSound;
        }
        private void OnDestroy()
        {
            cooldownBehavior.OnCooldownEvent -= ScheduleCooldownSound;
        }

        /// <summary>
        /// Schedules a sound to play when the cooldown is over.
        /// </summary>
        /// <param name="cooldown"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void ScheduleCooldownSound(float cooldown)
        {
            StartCoroutine(ScheduleCooldownRoutine(cooldown));

        }
        private IEnumerator ScheduleCooldownRoutine(float cooldown)
        {
            yield return new WaitForSeconds(cooldown);
            audioRelay.PlaySound(soundName);
        }
    }
}
