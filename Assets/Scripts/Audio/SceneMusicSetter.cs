/*****************************************************************************
// File Name : MinotaurController.cs
// Author : Brandon Koederitz
// Creation Date : 3/5/2026
// Last Modified : 3/5/2026
//
// Brief Description : Sets the current music track for a scene.
*****************************************************************************/
using GGL.Scoring;
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
            if (AudioManager.instance != null && AudioManager.instance.MusicManager != null)
            {
                if (sceneMusic == MusicType.Victory)
                {
                    AudioManager.instance.MusicManager.SetVictor(GameplayScoreManager.Winner);
                }
                AudioManager.instance.MusicManager.SetMusic(sceneMusic);
            }    
        }
    }
}
