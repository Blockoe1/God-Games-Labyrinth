using UnityEngine;
using UnityEngine.UI;

namespace GGL
{
    public class ReadyUpSystem1 : MonoBehaviour
    {
        public bool ready;
        void OnDash()
        {
            if (!ready)
            {
                ready = true;
                GetComponent<UnityEngine.UI.Image>().color = new Vector4(1, 0.9254902f, 0.2980392f, 1);
            }
            else if (ready)
            {
                ready = false;
                GetComponent<UnityEngine.UI.Image>().color = new Vector4(1, 1, 1, 1);
            }
        }
    }
}
