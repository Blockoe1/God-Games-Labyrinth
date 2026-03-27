using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGL
{
    public class NetworkManagerScene : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            if (FindFirstObjectByType<NetworkManager>() != null)
            {
                SceneManager.LoadScene("StartScene");
            }
        }
    }
}

