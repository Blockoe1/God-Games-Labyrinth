/*****************************************************************************
// File Name : NetworkMessenger.cs
// Author : Brandon Koederitz
// Creation Date : 1/24/2026
// Last Modified : 1/24/2026
//
// Brief Description : Handles sending scores across the network.
*****************************************************************************/
using NUnit.Framework.Constraints;
using Unity.Netcode;
using UnityEngine;

namespace GGL.Networking
{
    public class NetworkMessenger : MonoBehaviour
    {
        /// <summary>
        /// Broadcasts the change in score across the network.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void SendNetMessage(int[] message)
        {
            if (NetworkManager.Singleton.CustomMessagingManager == null)
            {
                throw new System.NotSupportedException
                    ("Cannot send a message over the network as the client is not connected.");
            }

            // Create a buffer writer to write the contents of the message.
            var writer = new FastBufferWriter(FastBufferWriter.GetWriteSize(message), Unity.Collections.Allocator.Temp);

            using(writer)
            {
                writer.WriteValueSafe(message);
                NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(NetworkReciever.MESSAGE_NAME, 
                    NetworkManager.ServerClientId, writer, NetworkDelivery.Reliable);
            }
        }

        #region Debug
        [ContextMenu("Send Debug Message")]
        private void SendTestMessage()
        {
            int[] message = Scoreboard.GetScoreboardArray();
            message[(int)GodID.Zeus] = 500;
            message[(int)GodID.Posiedon] = 100;
            message[(int)GodID.Athena] = 300;
            message[(int)GodID.Aphrodite] = 800;

            SendNetMessage(message);
        }
        #endregion
    }
}