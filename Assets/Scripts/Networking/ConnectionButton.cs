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
            connectAction.Enable();
            connectAction.performed += ConnectAction_performed;

            NetworkManager.OnInstantiated += OnNetworkManagerInstantiated;
            if (NetworkManager.Singleton != null)
            {
                OnNetworkManagerInstantiated(NetworkManager.Singleton);
                if (NetworkManager.Singleton.IsClient)
                {
                    
                }
            }
        }
        private void OnDestroy()
        {
            connectAction.performed -= ConnectAction_performed;
            NetworkManager.OnInstantiated -= OnNetworkManagerInstantiated;
        }

        /// <summary>
        /// Called by the NetworkManager when it's spawned/destroyed to subscribe/unsubscrive singleton events.
        /// </summary>
        private void OnNetworkManagerInstantiated(NetworkManager manager)
        {
            
        }

        /// <summary>
        /// Attempt a connection with either the host or client.
        /// </summary>
        /// <param name="obj"></param>
        private void ConnectAction_performed(InputAction.CallbackContext obj)
        {
            if (NetworkManager.Singleton == null) { return; }
            if (isHost)
            {
                if (!NetworkManager.Singleton.IsHost)
                {
                    Debug.Log("Connecting as host.");
                    NetworkManager.Singleton.StartHost();
                }
            }
            else
            {
                if (!NetworkManager.Singleton.IsClient)
                {
                    Debug.Log("Connecting as client.");
                    NetworkManager.Singleton.StartClient();
                }
            }
        }
    }
}
