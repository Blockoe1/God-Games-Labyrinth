/*****************************************************************************
// File Name : Collectable.cs
// Author : Brandon Koederitz
// Creation Date : 1/28/2026
// Last Modified : 1/28/2026
//
// Brief Description : Allows an object to be collected by the players.
*****************************************************************************/
using UnityEngine;
using UnityEngine.Events;

namespace GGL.Scoring
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private int pointValue;
        [SerializeField] private UnityEvent OnCollect;

        #region Properties
        public int PointValue => pointValue;
        #endregion

        /// <summary>
        /// Called when this object is collected.
        /// </summary>
        public void OnCollected()
        {
            OnCollect?.Invoke();
        }

        /// <summary>
        /// Called when the gold is cashed as score by the champion returning to their base.
        /// </summary>
        public void OnCashed()
        {

        }
    }
}
