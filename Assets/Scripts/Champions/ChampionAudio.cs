/***********************************************************************
// File Name : ChampionAudio.cs
// Author : Brandon Koederitz
// Creation Date : 2/22/2026
// Last Modified : 2/22/2026
//
// Brief Description : Displays increasing particle effects based on 
*****************************************************************************/
using FMODUnity;
using GGL.Audio;
using System;
using System.Collections;
using UnityEngine;

namespace GGL.Champions
{
    public class ChampionAudio : AudioRelay
    {
        #region CONSTS
        private const string COOLDOWN_SOUND_NAME = "Cooldown";
        #endregion

        [SerializeField] private FmodEvents.Sound[] sounds;

        private ChampionBehavior[] cooldownBehaviours;

        /// <summary>
        /// Setup event references for cooldown sounds.
        /// </summary>
        private void Awake()
        {
            cooldownBehaviours = GetComponents<ChampionBehavior>();
            foreach (var behaviour in cooldownBehaviours)
            {
                behaviour.OnCooldownEvent += ScheduleCooldownSound;
            }

        }
        private void OnDestroy()
        {
            foreach (var behaviour in cooldownBehaviours)
            {
                behaviour.OnCooldownEvent -= ScheduleCooldownSound;
            }
        }

        /// <summary>
        /// Plays a one shot sound by name.
        /// </summary>
        /// <param name="soundName"></param>
        public override void PlaySound(string soundName)
        {
            // Only uses sounds stored locally for this champion, instead of the FMOD events singleton.
            if (AudioManager.instance != null)
            {

                AudioManager.instance.PlayOneShot(FindEvent(soundName), transform.position);
            }
        }

        /// <summary>
        /// Gets an FMOD event with a given name.
        /// </summary>
        /// <param name="name"></param>
        public EventReference FindEvent(string name)
        {
            //Debug.Log(sounds.Length);
            FmodEvents.Sound foundSound = Array.Find(sounds, item => item.name == name);
            if (foundSound == null && FmodEvents.instance != null)
            {
                return FmodEvents.instance.FindEvent(name);
            }
            return foundSound.eventRef;
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
            PlaySound(COOLDOWN_SOUND_NAME);
        }
    }
}
