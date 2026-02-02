/*****************************************************************************
// File Name : CollectableSpawnArea.cs
// Author : Brandon Koederitz
// Creation Date : 2/1/2026
// Last Modified : 2/1/2026
//
// Brief Description : Continually spawns collectables for the champions to spawn.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;

namespace GGL.Scoring
{
    public class CollectableSpawnArea : MonoBehaviour
    {
        [SerializeField] private Vector2 goldScatterForce;

        /// <summary>
        /// Spawns a collectable at a random position within this area.
        /// </summary>
        /// <param name="toSpawn">The collectable to spawn.</param>
        private void SpawnCollectable(Collectable toSpawn)
        {
            Collectable spawnedCollectable = Instantiate(toSpawn, transform.position, Quaternion.identity);
            spawnedCollectable.ApplyScatterForce(Random.Range(goldScatterForce.x, goldScatterForce.y));

            // Add a callback so that this spawner updates when the gold is collected for the first time.
            spawnedCollectable.SubscribeCollectEvent(() => { });
        }

        private void 
    }
}
