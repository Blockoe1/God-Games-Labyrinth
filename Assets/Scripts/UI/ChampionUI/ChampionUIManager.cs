/*****************************************************************************
// File Name : ChampionUIManager.cs
// Author : Brandon Koederitz
// Creation Date : 2/19/2026
// Last Modified : 2/19/2026
//
// Brief Description : Rerouter script to centralize references to a specific champion on the UI.
*****************************************************************************/
using System.Linq;
using UnityEngine;

namespace GGL.UI.ChampionUI
{
    public class ChampionUIManager : MonoBehaviour
    {
        [SerializeField] private GodID team;
        [SerializeField] private ChampionUIService[] services;

        /// <summary>
        /// Initialize each service on awake.
        /// </summary>
        private void Awake()
        {
            GodIdentifier champion = FindObjectsByType<GodIdentifier>(FindObjectsSortMode.InstanceID)
                .Where(item => item.Team == team).FirstOrDefault();
            // Dont initialize if a champion wasn't found.
            //Debug.Log(champion);
            ChampionUIService[] services = GetComponentsInChildren<ChampionUIService>();
            if (champion != null)
            {
                foreach(ChampionUIService service in services)
                {
                    service.Initialize(champion);
                }
            }
        }
    }
}
