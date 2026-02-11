/*****************************************************************************
// File Name : EnvironmentInteractable.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Base class for components that can be interacted with via pressure plates.
*****************************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace GGL.Maze
{
    public abstract class EnvironmentInteractable : MonoBehaviour
    {
        #region CONSTS
        private static readonly Color GIZMO_COLOR = Color.green;
        #endregion

        [SerializeField, HideInInspector] private List<InteractableTrigger> gizmoTargets = new List<InteractableTrigger>();

        public abstract void OnInteract();

        #region Gizmo Drawing

        /// <summary>
        /// Adds/removes a target for drawing gizmos when this object is selected.
        /// </summary>
        /// <param name="trigger"></param>
        internal void AddGizmoTarget(InteractableTrigger trigger)
        {
            gizmoTargets.Add(trigger);
        }
        internal void RemoveGizmoTarget(InteractableTrigger trigger)
        {
            gizmoTargets.Remove(trigger);
        }

        /// <summary>
        /// Draws a gizmo showing which triggers can trigger this object.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = GIZMO_COLOR;
            foreach(var trigger in gizmoTargets)
            {
                if (trigger != null)
                {
                    Gizmos.DrawLine(transform.position, trigger.transform.position);
                }
            }
        }
        #endregion
    }
}
