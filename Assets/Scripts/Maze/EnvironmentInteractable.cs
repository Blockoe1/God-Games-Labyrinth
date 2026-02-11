/*****************************************************************************
// File Name : EnvironmentInteractable.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Base class for components that can be interacted with via pressure plates.
*****************************************************************************/
using UnityEngine;

namespace GGL
{
    public abstract class EnvironmentInteractable : MonoBehaviour
    {
        public abstract void OnInteract();
    }
}
