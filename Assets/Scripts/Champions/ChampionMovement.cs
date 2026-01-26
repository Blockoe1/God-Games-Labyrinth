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

        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private bool positionSnap;

        private InputAction moveAction;

        private float speed;
        private Vector2 moveInput;
        private bool markForSnap;
       
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
        /// <summary>
        /// Read player movement input.
        /// </summary>
        /// <param name="obj"></param>
        private void MoveAction_performed(InputAction.CallbackContext obj)
        {
            // Only take the X or Y Component for locked movement.
            Vector2 rawInput = obj.ReadValue<Vector2>().normalized;
            Vector2 oldMoveInput = moveInput;
            moveInput = Mathf.Abs(rawInput.y) > Mathf.Abs(rawInput.x) ? Vector2.up * System.MathF.Sign(rawInput.y) :
                Vector2.right * System.MathF.Sign(rawInput.x);

            //Snap the player's position to the grid when they change direction.
            if (positionSnap && moveInput != oldMoveInput)
            {
                markForSnap = true;
            }

        }
        private void MoveAction_canceled(InputAction.CallbackContext obj)
        {
            moveInput = Vector2.zero;
        }
        #endregion

        /// <summary>
        /// Control movement in FixedUpdate
        /// </summary>
        /// <remarks>
        /// Using FixedUpdate instead of a corutine because FixedUpdate happens before internal physics updates, while
        /// WaitForFixedUpdate happens after.
        /// </remarks>
        private void FixedUpdate()
        {
            speed = Mathf.MoveTowards(speed, maxSpeed, acceleration * Time.fixedDeltaTime);
            rb.linearVelocity = speed * moveInput;

            // Snap the player's position tot he grid when they change direction.
            if (markForSnap)
            {
                rb.MovePosition(MathHelpers.RoundVectorToInt(rb.position));
                markForSnap = false;
            }
        }
    }
}
