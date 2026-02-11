/*****************************************************************************
// File Name : InteractableTrigger.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Abstract base class for any script that interacts with an environment object.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace GGL.Maze
{
    public abstract class InteractableTrigger : MonoBehaviour
    {
        #region CONSTS
        private static readonly Color GIZMO_COLOR = Color.green;
        #endregion

        [SerializeField, Tooltip("The main object that this pressure pad activates.")]
        protected EnvironmentInteractable interactTarget;
        [SerializeField] protected bool requireSpecificGod;
        [SerializeField, ShowIf("requireSpecificGod")] protected GodID targetGod;
        [SerializeField] protected UnityEvent OnInteractEvent;

        [SerializeField, HideInInspector] private EnvironmentInteractable oldTarget;

        /// <summary>
        /// Checks if a certain game object is valid to interact with this trigger.
        /// </summary>
        /// <returns></returns>
        protected bool CheckValid(GameObject obj)
        {
            return obj.TryGetComponent(out GodIdentifier god) && (!requireSpecificGod || god.Team == targetGod);
        }

        /// <summary>
        /// Adds this object to the gizmoTargets list of our interact target, so that when it is selected it shows
        /// what triggers it.
        /// </summary>
        private void OnValidate()
        {
            if (oldTarget != interactTarget)
            {
                if (oldTarget != null)
                {
                    oldTarget.RemoveGizmoTarget(this);
                }
                if (interactTarget != null)
                {
                    interactTarget.AddGizmoTarget(this);
                }
                oldTarget = interactTarget;
            }
        }

        /// <summary>
        /// Draws a gizmo to show which door this pressure pad is connected to.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (interactTarget != null)
            {
                Gizmos.color = GIZMO_COLOR;
                Gizmos.DrawLine(transform.position, interactTarget.transform.position);
            }
        }
    }
}
