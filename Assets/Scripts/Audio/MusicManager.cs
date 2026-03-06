/***********************************************************************
// File Name : MuiscManager.cs
// Author : Brandon Koederitz
// Creation Date : 3/5/2026
// Last Modified : 3/5/2026
//
// Brief Description : Manages the currently playing music.
*****************************************************************************/
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace GGL.Audio
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private EventReference musicEvent;

        private EventInstance musicEventInstance;

        #region Music Management
        public void InitializeMusic()
        {
            musicEventInstance = AudioManager.instance.CreateInstance(musicEvent);
            musicEventInstance.start();
        }

        public void SetMusic(MusicType type)
        {
            musicEventInstance.setParameterByName("music_type", (float)type);
        }
        #endregion

        /// <summary>
        /// Stop music on game end.
        /// </summary>
        private void OnDestroy()
        {
            musicEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }
}
