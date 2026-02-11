/*****************************************************************************
// File Name : GoldStealer.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Fires a projectile that steals gold on contact with another player.
*****************************************************************************/
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGL.Champions
{
    public class GoldStealer : MonoBehaviour
    {
        #region CONSTS
        private const string STEAL_ACTION_NAME = "Steal";
        #endregion

        [SerializeField] private StealProjectile projectile;
        [SerializeField] private float stealCooldown;

        private InputAction stealAction;

        public Vector2 StealDirection { get; set; }

        /// <summary>
        /// Setup Input.
        /// </summary>
        private void Awake()
        {
            if (TryGetComponent(out PlayerInput input))
            {
                stealAction = input.currentActionMap.FindAction(STEAL_ACTION_NAME);

                stealAction.performed += StealAction_performed;
            }
        }
        private void OnDestroy()
        {
            if (stealAction != null)
            {
                stealAction.performed -= StealAction_performed;
            }
        }

        /// <summary>
        /// Launches the steal projectile when the player presses the steal key.
        /// </summary>
        /// <param name="obj"></param>
        private void StealAction_performed(InputAction.CallbackContext obj)
        {

        }
    }
}
