/*****************************************************************************
// File Name : AudioManager.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Interfaces with FMOD to play sounds.
*****************************************************************************/
using UnityEngine;

namespace GGL
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;

        private void Awake()
        {

            // If an AudioManager already exists, then we should destroy this AudioManager and prevent any setup.
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                // Make the AudioManager DontDestroyOnLoad so that sounds can persist across scenes.
                DontDestroyOnLoad(gameObject);
                instance = this;
            }

            AudioRelay.RelayPlayEvent = Play;
            AudioRelay.RelayStopEvent = Stop;
        }

        /// <summary>
        /// When the Audio Manager is destroyed, reset all relay actions.
        /// </summary>
        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
                AudioRelay.RelayPlayEvent = null;
                AudioRelay.RelayStopEvent = null;
            }
        }

        /// <summary>
        /// Plays a sound effect with a given ID.
        /// </summary>
        /// <param name="soundName">The ID of the sound to play.</param>
        public void Play(string soundName)
        {

        }

        /// <summary>
        /// Stops a sound effect with a given ID.
        /// </summary>
        /// <param name="soundName">The ID of the sound to stop.</param>
        public void Stop(string soundName)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopAll()
        {

        }
    }
}
