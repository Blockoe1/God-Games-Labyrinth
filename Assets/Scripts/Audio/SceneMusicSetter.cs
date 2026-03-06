/*****************************************************************************
// File Name : MinotaurController.cs
// Author : Brandon Koederitz
// Creation Date : 3/5/2026
// Last Modified : 3/5/2026
//
// Brief Description : Sets the current music track for a scene.
*****************************************************************************/
using UnityEngine;

namespace GGL.Audio
{
    public class LevelMusicSetter : MonoBehaviour
    {
        [SerializeField] private MusicType sceneMusic;

        /// <summary>
        /// Sets the music to play on enable.
        /// </summary>
        private void Start()
        {
            Debug.Log(AudioManager.instance);
            if (AudioManager.instance != null && AudioManager.instance.MusicManager != null)
            {
                AudioManager.instance.MusicManager.SetMusic(sceneMusic);
            }    
        }
    }
}
