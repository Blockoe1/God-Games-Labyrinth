/*****************************************************************************
// File Name : CollectableSpawnArea.cs
// Author : Brandon Koederitz
// Creation Date : 2/1/2026
// Last Modified : 2/1/2026
//
// Brief Description : Continually spawns collectables for the champions to spawn.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GGL.Scoring
{
    public class CollectableSpawnArea : MonoBehaviour
    {
        [SerializeField] private Collectable collectablePrefab;
        [SerializeField, Tooltip("The min and max possible force for spawned objects to have to scatter them " +
            "throughout the room.")] 
        private Vector2 scatterForce;
        [SerializeField, Tooltip("The base amount of time between new gold being spawned when no gold has been " +
            "collected.")] 
        private float baseSpawnDelay;
        [SerializeField, Tooltip("The amount that spawn delay increases with each gold collected from this room.")] 
        private float spawnDelayFalloff;
        [SerializeField, Tooltip("The amount of gold spawned from this room when no gold has been collected.")] 
        private int baseSpawnAmount;
        [SerializeField, Tooltip("How quickly the amount of gold spawned from this room decreases based on teh amount " +
            "of gold collected.  \nThis number represents the denominator that the number of gold to spawn is " +
            "divided by.  (Ie if set to 1, then after collecting 1 gold the amount of future gold spawned will be " +
            "1/ (1 + 1) or 1/2.")] 
        private float spawnAmountFalloff;

        private int numCollectedItems;
        private bool wasCollected;
        private bool isSpawning;

        #region Nested
        private class CollectEventWrapper
        {
            internal UnityAction unsubscribeAction;
            internal readonly Collectable toCollect;

            internal CollectEventWrapper(Collectable toCollect)
            {
                this.toCollect = toCollect;
            }
        }
        #endregion

        /// <summary>
        /// Debug
        /// </summary>
        private void Start()
        {
            StartSpawning();
        }

        /// <summary>
        /// Toggles the room's gold spawning on and off.
        /// </summary>
        public void StartSpawning()
        {
            if (isSpawning) { return; } 
            isSpawning = true;
            StartCoroutine(SpawnRoutine());
        }
        public void StopSpawning()
        {
            isSpawning = false;
        }

        /// <summary>
        /// Continually spawns new collectables in this room over time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator SpawnRoutine()
        {
            while(isSpawning)
            {
                // Mark the gold as not collected.
                wasCollected = false;
                for(int i = 0; i < GetSpawnNumber(); i++)
                {
                    SpawnCollectable(collectablePrefab);
                }

                yield return new WaitUntil(() => wasCollected);

                yield return new WaitForSeconds(GetSpawnDelay());
            }
        }

        /// <summary>
        /// Calculate the number of items to spawn based on the number of items collected from this room.
        /// </summary>
        /// <returns></returns>
        private int GetSpawnNumber()
        {
            // TODO: Implement math for reducing spawn number;
            return Mathf.CeilToInt(baseSpawnAmount / (1 + spawnAmountFalloff * numCollectedItems));
        }

        /// <summary>
        /// Calculate the delay between items being spawned based on the number of items collected from this room.
        /// </summary>
        /// <returns></returns>
        private float GetSpawnDelay()
        {
            // TODO: Implement math for reducing spawn time;
            return baseSpawnDelay + (spawnDelayFalloff * numCollectedItems);
        }

        /// <summary>
        /// Spawns a collectable at a random position within this area.
        /// </summary>
        /// <param name="toSpawn">The collectable to spawn.</param>
        private void SpawnCollectable(Collectable toSpawn)
        {
            Collectable spawnedCollectable = Instantiate(toSpawn, transform.position, Quaternion.identity);
            spawnedCollectable.ApplyScatterForce(Random.Range(scatterForce.x, scatterForce.y));

            // Add a callback so that this spawner updates when the gold is collected for the first time.
            CollectEventWrapper cew = new CollectEventWrapper(spawnedCollectable);
            UnityAction unsubAction = () => { LogItemCollected(cew); };
            cew.unsubscribeAction = unsubAction;
            spawnedCollectable.SubscribeCollectEvent(unsubAction);
        }

        /// <summary>
        /// Logs an item as collected for the first time and decreases the amount of future gold spawned from the room.
        /// </summary>
        /// <param name="cew">
        /// Wrapper class containing the collectable that was collected and the action ot unsubscribe.
        /// </param>
        private void LogItemCollected(CollectEventWrapper cew)
        {
            // Flag that at least one collectable was collected from this area so that new ones can spawn.
            wasCollected = true;

            // Track the number of items collected from this room.  Used in later calcs to reduce the amount spawned.
            numCollectedItems++;

            // Removes the item collected subscription so it's only called on the first collect.
            cew.toCollect.UnsubscribeCollectEvent(cew.unsubscribeAction);
        }
    }
}
