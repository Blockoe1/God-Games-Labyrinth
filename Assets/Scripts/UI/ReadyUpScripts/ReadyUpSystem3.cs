using GGL.Audio;
using UnityEngine;

namespace GGL
{
    public class ReadyUpSystem3 : MonoBehaviour
    {
        public bool ready;
        [SerializeField] private GameObject notReadyObject;
        [SerializeField] private GameObject readyObject;
        [SerializeField] private GameObject eyes;
        [SerializeField] private Sprite closedEyes;
        [SerializeField] private Sprite openEyes;
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
                eyes.GetComponent<SpriteRenderer>().sprite = openEyes;
            }
            else if (ready)
            {
                ready = false;
                readyObject.SetActive(false);
                notReadyObject.SetActive(true);
                eyes.GetComponent<SpriteRenderer>().sprite = closedEyes;
            }
        }
    }
}
