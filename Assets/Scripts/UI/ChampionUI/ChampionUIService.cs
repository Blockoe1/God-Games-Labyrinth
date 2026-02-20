/*****************************************************************************
// File Name : ChampionUIService.cs
// Author : Brandon Koederitz
// Creation Date : 2/19/2026
// Last Modified : 2/19/2026
//
// Brief Description : Base class for all scripts that display UI information about a champion.
*****************************************************************************/
using UnityEngine;

namespace GGL.UI.ChampionUI
{
    public abstract class ChampionUIService : MonoBehaviour
    {
        public abstract void Initialize(GodIdentifier champion);
    }
}
