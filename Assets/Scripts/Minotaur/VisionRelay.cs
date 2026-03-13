using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GGL.Minotaur
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class VisionRelay : MonoBehaviour
    {
        #region CONST
        private const string PLAYER_TAG = "Player";
        #endregion

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] protected PolygonCollider2D visionCollider;
        [SerializeReference, ReadOnly] private MinotaurVision vision;
        [SerializeReference, ReadOnly] private Light2D visionLight;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        protected virtual void Reset()
        {
            visionCollider = GetComponent<PolygonCollider2D>();
            vision = GetComponentInParent<MinotaurVision>();
            visionLight = GetComponentInChildren<Light2D>();
        }
        #endregion

        /// <summary>
        /// Sets the points of this trigger collider.
        /// </summary>
        /// <param name="points"></param>
        internal void UpdateVision(float visionAngle, float visionRange)
        {
            Vector2[] points = new Vector2[MinotaurVision.VISION_RESOLUTION + 1];
            points[0] = Vector2.zero;
            float subdividedAngle = visionAngle / MinotaurVision.VISION_RESOLUTION;
            for (int i = 1; i < points.Length; i++)
            {
                points[i] = MathHelpers.DegAngleToUnitVector((subdividedAngle * i) - (visionAngle / 2)) * visionRange;
            }
            
            if (visionLight != null)
            {
                visionLight.pointLightOuterRadius = visionRange;
                visionLight.pointLightInnerRadius = visionRange - 1;
                visionLight.pointLightInnerAngle = visionAngle;
                visionLight.pointLightOuterAngle = visionAngle;
            }

            visionCollider.pathCount = 1;
            visionCollider.SetPath(0, points);
        }

        /// <summary>
        /// Notifies the minotaurVision script when a champion enters the minotaur's vision collider.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(PLAYER_TAG))
            {
                vision.AddMonitoredObject(collision.gameObject);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(PLAYER_TAG))
            {
                vision.RemoveMonitoredObject(collision.gameObject);
            }
        }
    }
}
