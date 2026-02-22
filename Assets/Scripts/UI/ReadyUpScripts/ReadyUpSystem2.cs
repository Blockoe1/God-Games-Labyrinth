using UnityEngine;

namespace GGL
{
    public class ReadyUpSystem2 : MonoBehaviour
    {
        public bool ready;
        void OnDash()
        {
            if (!ready)
            {
                ready = true;
                GetComponent<UnityEngine.UI.Image>().color = new Vector4(0.4352941f, 0.8039216f, 1, 1);
            }
            else if (ready)
            {
                ready = false;
                GetComponent<UnityEngine.UI.Image>().color = new Vector4(1, 1, 1, 1);
            }
        }
    }
}
