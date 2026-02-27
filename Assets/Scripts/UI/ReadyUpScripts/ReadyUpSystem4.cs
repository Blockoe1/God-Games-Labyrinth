using UnityEngine;

namespace GGL
{
    public class ReadyUpSystem4 : MonoBehaviour
    {
        public bool ready;
        void OnDash()
        {
            if (!ready)
            {
                ready = true;
                GetComponent<UnityEngine.UI.Image>().color = new Vector4(0.9921569f, 0.6431373f, 0.9372549f, 1);
            }
            else if (ready)
            {
                ready = false;
                GetComponent<UnityEngine.UI.Image>().color = new Vector4(1, 1, 1, 1);
            }
        }
    }
}
