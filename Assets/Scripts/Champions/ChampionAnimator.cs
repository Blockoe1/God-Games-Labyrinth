/*****************************************************************************
// File Name : ChampionAnimator.cs
// Author : Brandon Koederitz
// Creation Date : 2/10/2026
// Last Modified : 2/10/2026
//
// Brief Description : Controls champion animations and allows external scripts to interface with the animator.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGL.Champions
{
    [RequireComponent(typeof(Animator))]
    public class ChampionAnimator : MonoBehaviour
    {
        #region CONSTS
        private const string MOVE_BOOL_NAME = "IsMoving";
        private const string DASH_BOOL_NAME = "IsDashing";
        #endregion

        #region Component References
        [SerializeReference, ReadOnly] private ChampionMovement movement;
        [SerializeReference, ReadOnly] private Animator anim;


        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            anim = GetComponent<Animator>();
            movement = GetComponent<ChampionMovement>();
        }
        #endregion

        /// <summary>
        /// Setup so that animation updates when the champion is moving.
        /// </summary>
        private void Awake()
        {
            movement.OnMove += SetMoving;
        }
        private void OnDestroy()
        {
            movement.OnMove -= SetMoving;
        }

        /// <summary>
        /// Sets the IsMoving parameter of the champion's animator.
        /// </summary>
        /// <param name="isMoving">The value to set the IsMoving parameter to.</param>
        public void SetMoving(bool isMoving)
        {
            anim.SetBool(MOVE_BOOL_NAME, isMoving);
        }

        /// <summary>
        /// Sets the IsDashing parameter of the champion's animator.
        /// </summary>
        /// <param name="isDashing">The value to set the IsDashing parameter to.</param>
        public void SetDashing(bool isDashing)
        {
            anim.SetBool(DASH_BOOL_NAME , isDashing);
        }
    }
}
