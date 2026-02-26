/*****************************************************************************
// File Name : NetworkReciever.cs
// Author : Brandon Koederitz
// Creation Date : 1/24/2026
// Last Modified : 1/24/2026
//
// Brief Description : Handles recieving messages from the network and broadcasting them locally.
*****************************************************************************/
using NaughtyAttributes;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace GGL.Networking
{
    public class NetworkReciever : MonoBehaviour
    {
        #region Consts
        internal const string MESSAGE_NAME = "ScoreboardMessage";
        #endregion

        [SerializeField] private UnityEvent<int[]> OnMessageRecieved;

        private NetworkManager networkManager;

        /// <summary>
        /// Setup event references to show and hide the UI element on network events.
        /// </summary>
        private void Awake()
        {
            NetworkManager.OnInstantiated += SubscribeEvents;
            NetworkManager.OnDestroying += UnsubscribeEvents;
            if (NetworkManager.Singleton != null)
            {
                SubscribeEvents(NetworkManager.Singleton);
            }
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
            networkManager = manager;
            manager.OnServerStarted += RegisterMessages;
            manager.OnPreShutdown += UnregisterMessages;
        }
        private void UnsubscribeEvents(NetworkManager manager)
        {
            manager.OnServerStarted -= RegisterMessages;
            manager.OnPreShutdown -= UnregisterMessages;
            networkManager = null;
        }

        /// <summary>
        /// Register/Unregister the message reciever when the client connects/disconnects.
        /// </summary>
        private void RegisterMessages()
        {
            networkManager.CustomMessagingManager.RegisterNamedMessageHandler(MESSAGE_NAME, RecieveMessage);
            Debug.Log("Registered the message reciever.");
        }
        private void UnregisterMessages()
        {
            if (networkManager.CustomMessagingManager != null)
            {
                networkManager.CustomMessagingManager.UnregisterNamedMessageHandler(MESSAGE_NAME);
                Debug.Log("Unregistered the message reciever.");
            }
        }

        /// <summary>
        /// Recieves a message from the network and broadcast it's contents out locally via a UnityEvent.
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="messagePayload"></param>
        private void RecieveMessage(ulong senderId, FastBufferReader messagePayload)
        {
            // Read the recieved int array.
            int[] recievedContents;
            messagePayload.ReadValueSafe(out recievedContents);

            // Broadcast the recieved message locally via UnityEvent
            OnMessageRecieved?.Invoke(recievedContents);
        }

        #region Debug
        public void PrintIntArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Debug.Log("Recieved value " + array[i] + " from the network at god value " + (GodID)i);
            }
        }
        #endregion
    }
}
