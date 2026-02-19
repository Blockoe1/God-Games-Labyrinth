/*****************************************************************************
// File Name : MinotaurVision.cs
// Author : Brandon Koederitz
// Creation Date : 2/16/2026
// Last Modified : 2/16/2026
//
// Brief Description : Component that handles detecting champions.
*****************************************************************************/
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGL.Minotaur
{
    public class MinotaurVision : MonoBehaviour
    {
        #region CONSTS
        private const int VISION_RESOLUTION = 10;
        private const string PLAYER_TAG = "Player";
        #endregion

        [SerializeField] private float visionAngle;
        [SerializeField] private float visionRange;

        private readonly List<GameObject> monitoredObjects = new List<GameObject>();
        private readonly List<GameObject> seenObjects = new List<GameObject>();

        public event Action<GameObject> OnChampionFound;
        public event Action<GameObject> OnChampionLost;

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
            if (seenObjects.Contains(monitoredObject))
            {
                seenObjects.Remove(monitoredObject);
                OnChampionLost?.Invoke(monitoredObject);
                //Debug.Log("Lost champion " + monitoredObject);
            }
        }

        /// <summary>
        /// Check for if monitored objects are within the minotaur's Line of Sight.
        /// </summary>
        private void FixedUpdate()
        {
            // Check for if monitored objects are in the minotaur's line of sight.
            for(int i = 0; i < monitoredObjects.Count; i++)
            {
                // Raycast to the monitored object.
                Vector2 toObj = monitoredObjects[i].transform.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toObj.normalized, visionRange, 
                    GGLHelpers.MazeMask | GGLHelpers.ChampionMask);
                // If the player was detected, mark it as a found champion.
                if (hit.collider != null && hit.collider.gameObject.CompareTag(PLAYER_TAG))
                {
                    OnChampionFound?.Invoke(hit.collider.gameObject);
                    monitoredObjects.RemoveAt(i);
                    seenObjects.Add(hit.collider.gameObject);
                    i--;
                    //Debug.DrawRay(transform.position, toObj.normalized * visionRange, Color.red);
                    //Debug.Log("Found champion " + hit.collider.gameObject);
                }
            }

            // Check for if seen objects have left the minotaur's line of sight.
            for (int i = 0; i < seenObjects.Count; i++)
            {
                // Raycast to the monitored object.
                Vector2 toObj = seenObjects[i].transform.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toObj.normalized, visionRange,
                    GGLHelpers.MazeMask | GGLHelpers.ChampionMask);
                // If the player was not detected, move it to monitored.
                if (hit.collider == null || !hit.collider.gameObject != seenObjects[i])
                {
                    OnChampionLost?.Invoke(seenObjects[i]);
                    monitoredObjects.Add(seenObjects[i]);
                    //Debug.Log("Lost champion " + seenObjects[i]);
                    seenObjects.RemoveAt(i);
                    i--;
                    //Debug.DrawRay(transform.position, toObj.normalized * visionRange, Color.green);
                }
            }
        }
    }
}
