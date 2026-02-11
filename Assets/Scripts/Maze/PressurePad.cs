/*****************************************************************************
// File Name : PressurePad.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Broadcasts an event when a champion of a specific god type walks over this.
*****************************************************************************/
using UnityEngine;

namespace GGL.Maze
{
    
    public class PressurePad : InteractableTrigger
    {
        /// <summary>
        /// Handles interacting with the pressure pad via collision.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Onlt trigger if this pressure pad doesn't require a specific god, or the champion's god matches the
            // target god.
            if (CheckValid(collision.gameObject))
            {
                if (interactTarget != null)
                {
                    interactTarget.OnInteract();
                }
                OnInteractEvent?.Invoke();
            }
        }
    }
}
