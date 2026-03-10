using FMOD.Studio;
using FMODUnity;
using GGL.Audio;
using UnityEngine;

namespace GGL
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance { get; private set; }

        public MusicManager MusicManager { get; private set; }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Debug.LogWarning("Multiple AudioManagers found in the scene.");
                Destroy(gameObject);
                return;
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                MusicManager = GetComponent<MusicManager>();
                if (MusicManager != null)
                {
                    MusicManager.InitializeMusic();
                }
            }
        }


        public void PlayOneShot(EventReference sound, Vector3 worldPos)
        {
            RuntimeManager.PlayOneShot(sound, worldPos);
        }
        public EventInstance CreateInstance(EventReference eventReference)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            return eventInstance;
        }
    }
}