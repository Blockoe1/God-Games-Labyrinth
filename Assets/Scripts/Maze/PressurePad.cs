/*****************************************************************************
// File Name : PressurePad.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Broadcasts an event when a champion of a specific god type walks over this.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace GGL.Maze
{
    public class PressurePad : MonoBehaviour
    {
        [SerializeField, Tooltip("The main object that this pressure pad activates.")] 
        private EnvironmentInteractable interactTarget;
        [SerializeField] private bool requireSpecificGod;
        [SerializeField, ShowIf("requireSpecificGod")] private GodID targetGod;
        [SerializeField] private UnityEvent OnInteractEvent;
    }
}
