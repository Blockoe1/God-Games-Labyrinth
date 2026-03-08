using UnityEngine;

namespace GGL
{
    public class ReadyUpSystem2 : MonoBehaviour
    {
        public bool ready;
        [SerializeField] private GameObject notReadyObject;
        [SerializeField] private GameObject readyObject;

        void OnDash()
        {
            if (!ready)
            {
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
