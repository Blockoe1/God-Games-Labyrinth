/*****************************************************************************
// File Name : SnapTargetFinder.cs
// Author : Brandon Koederitz
// Creation Date : 2/6/2026
// Last Modified : 2/6/2026
//
// Brief Description : Finds a snap target for this champion's movement.
*****************************************************************************/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGL.Champions
{
    public class SnapTargetFinder : MonoBehaviour
    {
        #region CONSTS
        private const string SNAP_TAG = "SnapTarget";
        #endregion

        [SerializeField] private ChampionMovement movement;

        private readonly List<Transform> snapTargets = new List<Transform>();

        /// <summary>
        /// Detect any snap targets and keep track of them.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(SNAP_TAG))
            {
                snapTargets.Add(collision.transform);
                SetSnapTarget();
            }
        }

        /// <summary>
        /// Remove snap targets fromt eh tracked list.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(SNAP_TAG) && snapTargets.Contains(collision.transform))
            {
                snapTargets.Remove(collision.transform);
                SetSnapTarget();
            }
        }

        /// <summary>
        /// Set the snap target used by the movement script.
        /// </summary>
        private void SetSnapTarget()
        {
            snapTargets.OrderBy((item) => Vector2.Distance(transform.position, item.position));
            movement.SnapTarget = snapTargets.FirstOrDefault();
        }
    }
}
