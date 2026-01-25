/*****************************************************************************
// File Name : NetworkReciever.cs
// Author : Brandon Koederitz
// Creation Date : 1/24/2026
// Last Modified : 1/24/2026
//
// Brief Description : Handles recieving messages from the network and broadcasting them locally.
*****************************************************************************/
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
 
        /// <summary>
        /// Setup Events to register message recieving.
        /// </summary>
        private void Start()
        {
            NetworkManager.Singleton.OnClientStarted += OnClientConnected;
            NetworkManager.Singleton.OnClientStopped += OnClientDisconnected;
        }
        private void OnDestroy()
        {
            NetworkManager.Singleton.OnClientStarted -= OnClientConnected;
            NetworkManager.Singleton.OnClientStopped -= OnClientDisconnected;
        }

        /// <summary>
        /// Register/Unregister the message reciever when the client connects/disconnects.
        /// </summary>
        private void OnClientConnected()
        {
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(MESSAGE_NAME, RecieveMessage);
        }
        private void OnClientDisconnected(bool hostMode)
        {
            NetworkManager.Singleton.CustomMessagingManager.UnregisterNamedMessageHandler(MESSAGE_NAME);
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
