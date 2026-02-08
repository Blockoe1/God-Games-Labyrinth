/*****************************************************************************
// File Name : CollectableSpawner.cs
// Author : Brandon Koederitz
// Creation Date : 2/8/2026
// Last Modified : 2/8/2026
//
// Brief Description : Spawns new collectables throughout the maze during the game.
*****************************************************************************/
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Linq;
using UnityEngine.Events;
using Unity.VisualScripting;

namespace GGL.Scoring
{
    public class CollectableSpawner : MonoBehaviour
    {
        [SerializeField] private Collectable collectablePrefab;
        [Header("Spawning Settings")]
        [SerializeField] private int spawnAmount;
        [SerializeField] private float spawnDelay;
        [SerializeField] private bool startFilled;

        [Header("Spawn Map Settings")]
        [SerializeField] private Tilemap mazeCollisionTilemap;
        [SerializeField, Tooltip("The coordinates of the bottom-left cell that the gold can spawn at.")] 
        private Vector2Int minBounds;
        [SerializeField, Tooltip("The coordiantes of the top-right cell that the gold can spawn at.")] 
        private Vector2Int maxBounds;
        [SerializeField] private BoxCollider2D[] excludedAreas;

        [SerializeField, ReadOnly] private Vector2Int[] spawnMap;

        private List<Vector2Int> validPositions;

        private readonly Queue<Collectable> collectablePool = new Queue<Collectable>();

        private bool isSpawning;

        #region Properties
        public bool IsSpawning
        {
            get { return isSpawning; }
            set { isSpawning = value; }
        }
        #endregion

        #region Nested
        private class CollectEventWrapper
        {
            internal UnityAction unsubscribeAction;
            internal readonly Collectable toCollect;
            internal readonly Vector2Int position;

            internal CollectEventWrapper(Collectable toCollect, Vector2Int position)
            {
                this.toCollect = toCollect;
                this.position = position;
            }
        }
        #endregion

        /// <summary>
        /// Bakes the array of spawn position data for this object.
        /// </summary>
        [Button]
        private void BakeSpawnPositions()
        {
            List<Vector2Int> spawnPositions = new List<Vector2Int>();
            for (int i = minBounds.y; i <= maxBounds.y; i++)
            {
                for (int j = minBounds.x; j <= maxBounds.x; j++)
                {
                    Vector2 position = transform.position + new Vector3(j, i);

                    bool inValidArea = true;
                    // Check if the position is within an excluded area.
                    foreach (BoxCollider2D excludedArea in excludedAreas)
                    {
                        if (excludedArea.bounds.Contains(position))
                        {
                            inValidArea = false;
                            break;
                        }
                    }

                    // Check if this cell is unoccupied in the tilemap.
                    TileBase tile = mazeCollisionTilemap.GetTile(mazeCollisionTilemap.WorldToCell(position));
                    if (inValidArea && tile == null)
                    {
                        Debug.DrawLine(position, position + Vector2.up, Color.green, 5f);
                        spawnPositions.Add(new Vector2Int(j, i));
                    }
                }
            }
            spawnMap = spawnPositions.ToArray();
        }

        /// <summary>
        /// Initialize our valid positions.
        /// </summary>
        private void Awake()
        {
            validPositions = spawnMap.ToList();
        }

        /// <summary>
        /// Fill the maze with collectables if the setting is set.
        /// </summary>
        private void Start()
        {
            if (startFilled)
            {
                FillAllSpaces();
            }
            isSpawning = true;
            StartCoroutine(SpawnRoutine());
        }

        /// <summary>
        /// Continually spawn collectables
        /// </summary>
        /// <returns></returns>
        private IEnumerator SpawnRoutine()
        {
            while (isSpawning)
            {
                for (int i = 0; i < spawnAmount; i++)
                {
                    SpawnAtRandomPosition();
                }

                yield return new WaitForSeconds(spawnDelay);
            }
        }

        /// <summary>
        /// Spawns a collectable at all valid spaces.
        /// </summary>
        private void FillAllSpaces()
        {
            Vector2Int[] validPositionsCopy = validPositions.ToArray();
            foreach (var item in validPositionsCopy)
            {
                SpawnAtPosition(item);
            }
        }

        /// <summary>
        /// Spawns a collectable at a random valid position.
        /// </summary>
        private void SpawnAtRandomPosition()
        {
            Vector2Int position = validPositions[Random.Range(0, validPositions.Count)];
            SpawnAtPosition(position);
        }

        /// <summary>
        /// Spawns a collectable at a given position.
        /// </summary>
        /// <param name="position">The valid position to spawn the collectable at.</param>
        private void SpawnAtPosition(Vector2Int position)
        {
            Collectable toSpawn = GetCollectable();
            toSpawn.transform.position = (Vector2)position + (Vector2)transform.position;
            toSpawn.OnCashedCallback = ReturnCollectable;

            // Setup so that when the collectable is collected for the first time, it makes it's spawn
            // position valid again.
            CollectEventWrapper cew = new CollectEventWrapper(toSpawn, position);
            UnityAction unsubAction = () => { LogCollected(cew); };
            cew.unsubscribeAction = unsubAction;
            toSpawn.SubscribeCollectEvent(unsubAction);

            // Remove the position this object was spawned at from our valid positions. (cant have double coins)
            validPositions.Remove(position);

            toSpawn.gameObject.SetActive(true);
        }

        /// <summary>
        /// Logs a certain collectable as collected and makes it's position valid again.
        /// </summary>
        /// <param name="cew"></param>
        private void LogCollected(CollectEventWrapper cew)
        {
            AddValidPosition(cew.position);
            cew.toCollect.UnsubscribeCollectEvent(cew.unsubscribeAction);
        }

        /// <summary>
        /// Adds a valid position for collectables to spawn at.
        /// </summary>
        /// <param name="position">The position that collectables are now allowed to spawn at.</param>
        private void AddValidPosition(Vector2Int position)
        {
            validPositions.Add(position);
        }

        #region Object Pooling
        /// <summary>
        /// Gets a collectable GameObject from the object pool.
        /// </summary>
        /// <returns>The collectable from the pool.</returns>
        private Collectable GetCollectable()
        {
            Collectable toGet = collectablePool.Count > 0 ? collectablePool.Dequeue() : 
                Instantiate(collectablePrefab, transform);
            return toGet;
        }

        /// <summary>
        /// Returns a cashes collectable to the object pool.
        /// </summary>
        /// <param name="collectable">The collectable tor eturn to the pool.</param>
        private void ReturnCollectable(Collectable collectable)
        {
            collectable.gameObject.SetActive(false);
            collectablePool.Enqueue(collectable);
        }
        #endregion
    }
}
