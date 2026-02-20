/*****************************************************************************
// File Name : ConnectionButton.cs
// Author : Brandon Koederitz
// Creation Date : 2/20/2026
// Last Modified : 2/20/2026
//
// Brief Description : Connects a disconnected client or host when a button is pressed.
*****************************************************************************/
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGL.Networking
{
    public class ConnectionButton : MonoBehaviour
    {
        [SerializeField] private InputAction connectAction;
        [SerializeField] private bool isHost;

        /// <summary>
        /// Setup/Unsubscribe Input.
        /// </summary>
        private void Awake()
        {
            connectAction.performed += ConnectAction_performed;
        }
        private void OnDestroy()
        {
            connectAction.performed -= ConnectAction_performed;
        }

        /// <summary>
        /// Attempt a connection with either the host or client.
        /// </summary>
        /// <param name="obj"></param>
        private void ConnectAction_performed(InputAction.CallbackContext obj)
        {
            if (isHost)
            {
                Debug.Log("Connecting as host.");
                NetworkManager.Singleton.StartHost();
            }
            else
            {
                Debug.Log("Connecting as client.");
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}
