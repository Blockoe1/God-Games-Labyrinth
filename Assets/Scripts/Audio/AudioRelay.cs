/*****************************************************************************
// File Name : AudioRelay.cs
// Author : Brandon Koederitz
// Creation Date : 2/13/2026
// Last Modified : 2/13/2026
//
// Brief Description : Interfaces with the audio manager and FMOD events to play sounds 
*****************************************************************************/
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace GGL.Audio
{
    public class AudioRelay : MonoBehaviour
    {
        /// <summary>
        /// Plays a sound with a given name.
        /// </summary>
        /// <remarks>Done this way so it can be called with UnityEvents.</remarks>
        /// <param name="soundName"></param>
        public virtual void PlaySound(string soundName)
        {
            //Debug.Log(FmodEvents.instance + "-" + AudioManager.instance);
            if (FmodEvents.instance != null && AudioManager.instance != null)
            {
                EventReference eventInst = FmodEvents.instance.FindEvent(soundName);
                AudioManager.instance.PlayOneShot(eventInst, transform.position);
            }
        }
    }
}
