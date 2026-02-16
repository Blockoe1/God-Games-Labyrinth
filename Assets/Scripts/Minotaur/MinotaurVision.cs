/*****************************************************************************
// File Name : MinotaurVision.cs
// Author : Brandon Koederitz
// Creation Date : 2/16/2026
// Last Modified : 2/16/2026
//
// Brief Description : Component that handles detecting champions.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;

namespace GGL.Minotaur
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class MinotaurVision : MonoBehaviour
    {
        [SerializeField] private float visionAngle;
        [SerializeField] private float visionRange;

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] protected PolygonCollider2D visionCollider;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        protected virtual void Reset()
        {
            visionCollider = GetComponent<PolygonCollider2D>();
        }
        #endregion

        /// <summary>
        /// Update the polygon collider's shape based on the specified vision andle and range.
        /// </summary>
        private void OnValidate()
        {
            visionCollider.pathCount = 3;
            Vector2 origin = Vector2.zero;
            Vector2 posPoint = MathHelpers.DegAngleToUnitVector(visionAngle/2) * visionRange;
            Vector2 negPoint = MathHelpers.DegAngleToUnitVector(-visionAngle / 2) * visionRange;

            visionCollider.SetPath(0, new Vector2[] { origin, posPoint });
            visionCollider.SetPath(1, new Vector2[] { posPoint, negPoint });
            visionCollider.SetPath(2, new Vector2[] { negPoint, origin });
        }
    }
}
