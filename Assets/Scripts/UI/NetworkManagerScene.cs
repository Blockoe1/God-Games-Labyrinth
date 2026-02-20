using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGL
{
    public class NetworkManagerScene : MonoBehaviour
    {
        void Update()
        {
            if (FindFirstObjectByType<NetworkManager>() != null)
            {
                SceneManager.LoadScene("StartScene");
            }
        }
    }
}
