using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace GGL
{
    public class AudioManager : MonoBehaviour
    {
        private EventInstance musicEventInstance;

        public static AudioManager instance { get; private set; }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Debug.LogWarning("Multiple AudioManagers found in the scene.");
                return;
            }
            else
            {
                instance = this;
            }
        }

        private void Start()
        {
            InitializeMusic(FmodEvents.instance.music);
        }

        private void InitializeMusic(EventReference musicEventReference)
        {
            musicEventInstance = CreateInstance(musicEventReference);
            musicEventInstance.start();
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