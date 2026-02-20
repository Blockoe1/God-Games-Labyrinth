using UnityEngine;
using FMODUnity;
using System.ComponentModel;
using System;

namespace GGL
{
    public class FmodEvents : MonoBehaviour
    {
        [SerializeField] private Sound[] sounds;
        [field: Header("Music")]
        [field:SerializeField] public EventReference music { get; private set; }

        public static FmodEvents instance {  get; private set; }

        #region Nested
        [System.Serializable]
        private class Sound
        {
            [SerializeField] internal string name;
            [SerializeField] internal EventReference eventRef;
        }
        #endregion

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Debug.LogWarning("Multiple FMODEvents found in the scene.");
                return;
            }
            else
            {
                instance = this;
            }
        }

        /// <summary>
        /// Gets an FMOD event with a given name.
        /// </summary>
        /// <param name="name"></param>
        public EventReference FindEvent(string name)
        {
            //Debug.Log(sounds.Length);
            return Array.Find(sounds, item => item.name == name).eventRef;
        }
    }
}
