/*****************************************************************************
// File Name : NetworkConnectionIndicator.cs
// Author : Brandon Koederitz
// Creation Date : 2/20/2026
// Last Modified : 2/20/2026
//
// Brief Description : Displays a UI element when the network connection disconnects.
*****************************************************************************/
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace GGL.UI
{
    public class NetworkConnectionIndicator : MonoBehaviour
    {
        [SerializeField] private Image disconnectedImage;

        /// <summary>
        /// Setup event references to show and hide the UI element on network events.
        /// </summary>
        private void Awake()
        {
            NetworkManager.OnInstantiated += SubscribeEvents;
            NetworkManager.OnDestroying += UnsubscribeEvents;
        }
        private void OnDestroy()
        {
            NetworkManager.OnInstantiated -= SubscribeEvents;
            NetworkManager.OnDestroying -= UnsubscribeEvents;
        }

        /// <summary>
        /// Called by the NetworkManager when it's spawned/destroyed to subscribe/unsubscrive singleton events.
        /// </summary>
        private void SubscribeEvents(NetworkManager manager)
        {
            manager.OnClientStarted += OnClientStarted;
            manager.OnClientStopped += OnClientStopped;
        }
        private void UnsubscribeEvents(NetworkManager manager)
        {
            manager.OnClientStarted -= OnClientStarted;
            manager.OnClientStopped -= OnClientStopped;
        }

        /// <summary>
        /// Removes the disconnect image when the client is connected.
        /// </summary>
        private void OnClientStarted()
        {
            disconnectedImage.enabled = false;
        }

        /// <summary>
        /// Show the disconnect image when the client is disconnected.
        /// </summary>
        /// <param name="isHost">If this client was run as a host.</param>
        private void OnClientStopped(bool isHost)
        {
            disconnectedImage.enabled = true;
        }
    }
}
