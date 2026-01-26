/*****************************************************************************
// File Name : ChampionMovement.cs
// Author : Brandon Koederitz
// Creation Date : 1/24/2026
// Last Modified : 1/24/2026
//
// Brief Description : Handles base champion movement.
*****************************************************************************/
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGL.Champions
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class ChampionMovement : MonoBehaviour
    {
        #region CONSTS
        private const string MOVE_ACTION_NAME = "Move";
        #endregion

        private Vector2 moveInput;

        private InputAction moveAction;
       
        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] private Rigidbody2D rb;
        [SerializeReference, ReadOnly] private PlayerInput input;



        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            rb = GetComponent<Rigidbody2D>();
            input = GetComponent<PlayerInput>();
        }
        #endregion

        /// <summary>
        /// Subscribe/Unsubscribe input.
        /// </summary>
        private void Awake()
        {
            moveAction = input.actions.FindAction(MOVE_ACTION_NAME);
            moveAction.performed += MoveAction_performed;
            moveAction.canceled += MoveAction_canceled;
        }
        private void OnDestroy()
        {
            moveAction.performed -= MoveAction_performed;
            moveAction.canceled -= MoveAction_canceled;
        }

        #region Input Functions
        private void MoveAction_performed(InputAction.CallbackContext obj)
        {
            throw new System.NotImplementedException();
        }

        private void MoveAction_canceled(InputAction.CallbackContext obj)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
