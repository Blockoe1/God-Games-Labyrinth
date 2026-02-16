/*****************************************************************************
// File Name : MinotaurVision.cs
// Author : Brandon Koederitz
// Creation Date : 2/16/2026
// Last Modified : 2/16/2026
//
// Brief Description : Component that handles detecting champions.
*****************************************************************************/
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace GGL.Minotaur
{
    public class MinotaurVision : MonoBehaviour
    {
        #region CONSTS
        private const int VISION_RESOLUTION = 10;
        #endregion

        [SerializeField] private float visionAngle;
        [SerializeField] private float visionRange;

        private readonly List<GameObject> monitoredObjects = new List<GameObject>();

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] protected VisionRelay relay;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        protected virtual void Reset()
        {
            relay = GetComponentInChildren<VisionRelay>();
        }
        #endregion

        /// <summary>
        /// Update the polygon collider's shape based on the specified vision andle and range.
        /// </summary>
        private void OnValidate()
        {
            Vector2[] points = new Vector2[VISION_RESOLUTION + 1];
            points[0] = Vector2.zero;
            float subdividedAngle = visionAngle / VISION_RESOLUTION;
            for (int i = 1; i < points.Length; i++)
            {
                points[i] = MathHelpers.DegAngleToUnitVector((subdividedAngle * i) - (visionAngle / 2)) * visionRange;
            }

            relay.SetPath(points);
        }

        /// <summary>
        /// Adds/Removes game objects from the monitoredObjects list.  Tracks what champions are in the line of sight
        /// collider.
        /// </summary>
        /// <param name="monitoredObject"></param>
        internal void AddMonitoredObject(GameObject monitoredObject)
        {
            monitoredObjects.Add(monitoredObject);
        }
        internal void RemoveMonitoredObject(GameObject monitoredObject)
        {
            monitoredObjects.Remove(monitoredObject);
        }
    }
}
