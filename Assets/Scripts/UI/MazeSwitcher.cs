/*****************************************************************************
// File Name : MazeSwitcher.cs
// Author : Brandon Koederitz
// Creation Date : 3/8/2026
// Last Modified : 3/8/2026
//
// Brief Description : Swaps between maze a and b for testing.
*****************************************************************************/
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGL.UI
{
    public class MazeSwitcher : MonoBehaviour
    {
        [SerializeField] private ReadyUpHandler handler;
        [SerializeField] private TMP_Text abTest;
        [SerializeField] private InputAction swapIA;
        [SerializeField, Scene] private string mazeA;
        [SerializeField, Scene] private string mazeB;

        bool isB;

        /// <summary>
        /// Setup Input.
        /// </summary>
        private void Awake()
        {
            swapIA.Enable();
            swapIA.performed += SwapScene;
        }

        private void OnDestroy()
        {
            swapIA.performed -= SwapScene;
        }

        /// <summary>
        /// Swaps the scene loaded when all champions are ready.
        /// </summary>
        /// <param name="obj"></param>
        private void SwapScene(InputAction.CallbackContext obj)
        {
            isB = !isB;
            handler.MazeScene = isB ? mazeB : mazeA;
            abTest.text = isB ? "B" : "A";
        }
    }
}
