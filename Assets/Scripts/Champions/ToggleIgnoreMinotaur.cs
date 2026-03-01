/*****************************************************************************
// File Name : CollectableSpawner.cs
// Author : Brandon Koederitz
// Creation Date : 3/1/2026
// Last Modified : 3/1/2026
//
// Brief Description : Toggles the player's hitbox colliding with the minotaur.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGL.Champions
{
    public class ToggleIgnoreMinotaur : MonoBehaviour
    {
        #region Component References
        [SerializeReference, ReadOnly] private Collider2D coll;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References (Child)")]
        private void Reset()
        {
            coll = GetComponent<Collider2D>();
        }
        #endregion
        public void Toggle(bool canHit)
        {
            if (canHit)
            {
                coll.excludeLayers = coll.excludeLayers | LayerMask.GetMask("Minotaur");
            }
            else
            {
                coll.excludeLayers = coll.excludeLayers & ~LayerMask.GetMask("Minotaur");
            }
        }
    }
}
