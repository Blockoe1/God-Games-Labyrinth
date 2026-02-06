using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace GGL
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance { get; private set; }

        private void Awake()
        {
            instance = this;
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
