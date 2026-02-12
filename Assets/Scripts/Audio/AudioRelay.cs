/*****************************************************************************
// File Name : AudioRelay.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Relay to allow objects to interface with the AudioManager and call sounds from UnityEvents.
*****************************************************************************/
using System;
using UnityEngine;

namespace GGL
{
    public class AudioRelay : MonoBehaviour
    {
        [SerializeField] private string defaultSoundName;

        public static Action<string> RelayPlayEvent;
        public static Action<string> RelayStopEvent;

        /// <summary>
        /// Plays a sound on the audio manager via an event.
        /// </summary>
        public void Play()
        {
            Play(defaultSoundName);
        }
        public void Play(string soundName)
        {
            RelayPlayEvent?.Invoke(soundName);
        }

        /// <summary>
        /// Stops a given sound on the audioManager by event.
        /// </summary>
        public void Stop()
        {
            Stop(defaultSoundName);
        }
        public void Stop(string soundName)
        {
            RelayStopEvent?.Invoke(soundName);
        }
    }
}
