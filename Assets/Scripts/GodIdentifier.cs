/*****************************************************************************
// File Name : GodMask.cs
// Author : Brandon Koederitz
// Creation Date : 2/11/2026
// Last Modified : 2/11/2026
//
// Brief Description : Determines which god a champion represents.
*****************************************************************************/
using UnityEngine;

namespace GGL
{
    public class GodIdentifier : MonoBehaviour
    {
        [field: SerializeField] public GodID Team { get; private set; }
    }
}
