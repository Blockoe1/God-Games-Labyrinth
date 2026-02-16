using NaughtyAttributes;
using UnityEngine;

namespace GGL.Minotaur
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class VisionRelay : MonoBehaviour
    {
        #region CONST
        private string PLAYER_TAG = "Player";
        #endregion

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] protected PolygonCollider2D visionCollider;
        [SerializeReference, ReadOnly] private MinotaurVision vision;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        protected virtual void Reset()
        {
            visionCollider = GetComponent<PolygonCollider2D>();
            vision = GetComponentInParent<MinotaurVision>();
        }
        #endregion

        /// <summary>
        /// Sets the points of this trigger collider.
        /// </summary>
        /// <param name="points"></param>
        internal void SetPath(Vector2[] points)
        {
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
