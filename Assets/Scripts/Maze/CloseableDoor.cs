/*****************************************************************************
// File Name : CloseableDoor.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : A door that can be remotely closed by a pressure plate.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GGL.Maze
{
    public class CloseableDoor : EnvironmentInteractable
    {
        [SerializeField] private float closeTime;
        [SerializeField] private UnityEvent OnDoorClosed;
        [SerializeField] private UnityEvent OnDoorOpen;

        private bool isClosed;

        /// <summary>
        /// Closes the door on interact.
        /// </summary>
        public override void OnInteract()
        {
            CloseForSeconds(closeTime);
        }

        /// <summary>
        /// Controls closing the door.
        /// </summary>
        public void CloseDoor()
        {
            isClosed = true;
            OnDoorClosed?.Invoke();
        }
        /// <summary>
        /// Controls opening the door.
        /// </summary>
        public void OpenDoor()
        {
            isClosed = false;
            OnDoorOpen?.Invoke();
        }

        /// <summary>
        /// Closes the door for a certain number of seconds before automatically reopening it.
        /// </summary>
        /// <param name="seconds">The amount of time to keep the door closed before it reopens.</param>
        public void CloseForSeconds(float seconds)
        {
            // Prevent double closures.
            if (isClosed) { return; }

            CloseDoor();
            StartCoroutine(ReopenDoorRoutine(seconds));
        }
        private IEnumerator ReopenDoorRoutine(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            if (isClosed)
            {
                OpenDoor();
            }
        }
    }
}
