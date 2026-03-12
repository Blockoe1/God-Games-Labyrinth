using GGL.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace GGL
{
    public class ReadyUpSystem1 : MonoBehaviour
    {
        public bool ready;
        [SerializeField] private GameObject notReadyObject;
        [SerializeField] private GameObject readyObject;
        [SerializeField] private AudioRelay relay;
        [SerializeField] private string readySoundName;

        void OnReady()
        {
            if (!ready)
            {
                relay.PlaySound(readySoundName);
                ready = true;
                notReadyObject.SetActive(false);
                readyObject.SetActive(true);
            }
            else if (ready)
            {
                ready = false;
                readyObject.SetActive(false);
                notReadyObject.SetActive(true);
            }
        }
    }
}
