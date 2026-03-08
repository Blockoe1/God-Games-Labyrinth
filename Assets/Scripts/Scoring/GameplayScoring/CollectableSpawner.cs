/*****************************************************************************
// File Name : CollectableSpawner.cs
// Author : Brandon Koederitz
// Creation Date : 2/8/2026
// Last Modified : 2/8/2026
//
// Brief Description : Spawns new collectables throughout the maze during the game.
*****************************************************************************/
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace GGL.Scoring
{
    public class CollectableSpawner : MonoBehaviour
    {
        [SerializeField] private Collectable[] collectablePrefabs;
        [Header("Spawning Settings")]
        [SerializeField] private int spawnAmount;
        [SerializeField] private float spawnDelay;
        [SerializeField] private int startingGoldAmount;
        [SerializeField, Tooltip("Controls how heavily gold skews towards spawning in the center.")] 
        private float centerSkew;

        [Header("Spawn Map Settings")]
        [SerializeField] private Tilemap mazeCollisionTilemap;
        [SerializeField, Tooltip("The coordinates of the bottom-left cell that the gold can spawn at.")] 
        private Vector2Int minBounds;
        [SerializeField, Tooltip("The coordiantes of the top-right cell that the gold can spawn at.")] 
        private Vector2Int maxBounds;
        [SerializeField, Tooltip("All BoxCollider2Ds within game objects in this array will be considered " +
            "invalid spaces for gold to spawn.")] 
        private GameObject[] excludedAreas;

        [SerializeField, ReadOnly] private Vector2Int[] spawnMap;
        [SerializeField, ReadOnly] private int totalMapWeight;

        private List<Vector2Int> validPositions;
        private int totalValidWeight;

        public static List<Collectable> Collectables { get; private set; } = new List<Collectable>();

        private readonly Queue<Collectable> collectablePool = new Queue<Collectable>();

        private bool isSpawning;

        #region Properties
        public bool IsSpawning
        {
            get { return isSpawning; }
            set { isSpawning = value; }
        }
        #endregion

        /// <summary>
        /// Bakes the array of spawn position data for this object.
        /// </summary>
        [Button]
        private void BakeSpawnPositions()
        {
            totalMapWeight = 0;
            List<Vector2Int> spawnPositions = new List<Vector2Int>();
            for (int i = minBounds.y; i <= maxBounds.y; i++)
            {
                for (int j = minBounds.x; j <= maxBounds.x; j++)
                {
                    Vector2 position = new Vector3(j, i);

                    bool inValidArea = true;
                    // Check if the position is within an excluded area.
                    foreach (GameObject excludedArea in excludedAreas)
                    {
                        // Get all box colliders within the excluded area.
                        BoxCollider2D[] colliders = excludedArea.GetComponentsInChildren<BoxCollider2D>();
                        foreach(BoxCollider2D collider in colliders)
                        {
                            if (collider.bounds.Contains(position))
                            {
                                inValidArea = false;
                                break;
                            }
                        }
                    }

                    // Check if this cell is unoccupied in the tilemap.
                    TileBase tile = mazeCollisionTilemap.GetTile(mazeCollisionTilemap.WorldToCell(position));
                    if (inValidArea && tile == null)
                    {
                        Debug.DrawLine(position, position + Vector2.up, Color.green, 5f);
                        Vector2Int intPos = new Vector2Int(j, i);
                        spawnPositions.Add(intPos);
                        totalMapWeight += intPos.x + intPos.y;
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
#if UNITY_EDITOR
            // Dynamically bake on awake so it's easier on the designers.
            BakeSpawnPositions();
#endif

            validPositions = spawnMap.ToList();
            totalValidWeight = totalMapWeight;

            // Log any gold placed in the scene already by designers.
            Collectable[] inSceneCollectables = GetComponentsInChildren<Collectable>();
            foreach(Collectable sceneCollectable in inSceneCollectables)
            {
                Vector2Int position = MathHelpers.RoundVectorToInt(sceneCollectable.transform.localPosition);
                RegisterCollectable(sceneCollectable, position);
            }

            // Spawn the initial random gold.
            for (int i = 0; i < startingGoldAmount; i++)
            {
                SpawnAtRandomPosition();
            }
        }

        /// <summary>
        /// Fill the maze with collectables if the setting is set.
        /// </summary>
        private void Start()
        {
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
        /// Spawns a collectable at evenly spaced valid spaces.
        /// </summary>
        private void FillSpaces(int numToSpawn)
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
            if (validPositions.Count > 0)
            {
                Vector2Int position = GetWeightedRandomPosition();
                SpawnAtPosition(position);
            }
        }

        /// <summary>
        /// Gets a position to spawn gold at using weighted randomness, with gold skewing towards the middle.
        /// </summary>
        /// <returns></returns>
        private Vector2Int GetWeightedRandomPosition()
        {
            InverseNormalDist(,0, centerSkew);
            return validPositions[Random.Range(0, validPositions.Count)];
        }

        /// <summary>
        /// Calculates the probability density of a value based on the mean and standard deviation of a normal distribution.
        /// </summary>
        /// <param name="value">The value to find the probability density of.</param>
        /// <param name="mean">The mean of the normal distribution.</param>
        /// <param name="centerSkew">The standard deviation of the normal distribution.</param>
        public static float InverseNormalDist(float value, float mean, float centerSkew)
        {
            return (centerSkew * Mathf.Pow(System.MathF.E, (Mathf.Pow((value - mean) * centerSkew, 2) / -2))) /
                Mathf.Sqrt(2 * Mathf.PI);
        }

        /// <summary>
        /// Spawns a collectable at a given position.
        /// </summary>
        /// <param name="position">The valid position to spawn the collectable at.</param>
        private void SpawnAtPosition(Vector2Int position)
        {
            Collectable toSpawn = GetCollectable();
            toSpawn.transform.localPosition = (Vector2)position;
            toSpawn.OnCashedCallback = ReturnCollectable;

            // Gives the collectable a random rotation.
            float randomAngle = 90 * Random.Range(0, 4);
            toSpawn.transform.eulerAngles = new Vector3(toSpawn.transform.eulerAngles.x,
                toSpawn.transform.eulerAngles.y, randomAngle);

            RegisterCollectable(toSpawn, position);

            toSpawn.gameObject.SetActive(true);

            //Debug.Log($"Spawned a collectable at position {position}");
        }

        /// <summary>
        /// Registers this collectable with the collectable systems.
        /// </summary>
        /// <param name="collectable"></param>
        private void RegisterCollectable(Collectable collectable, Vector2Int position)
        {
            // Setup so that when the collectable is collected for the first time, it makes it's spawn
            // position valid again.
            //CollectEventWrapper cew = new CollectEventWrapper(collectable, position);
            //void unsubAction() { LogCollected(cew); }
            //cew.unsubscribeAction = unsubAction;
            //collectable.SubscribeCollectEvent(unsubAction);
            collectable.SubscribeCollectOneShot(() => { LogCollected(position, collectable); });

            // Remove the position this object was spawned at from our valid positions. (cant have double coins)
            validPositions.Remove(position);
            totalValidWeight -= position.x + position.y;

            Collectables.Add(collectable);
        }

        /// <summary>
        /// Logs a certain collectable as collected and makes it's position valid again.
        /// </summary>
        /// <param name="position">The spawn position of the collected item.</param>
        private void LogCollected(Vector2Int position, Collectable collectable)
        {
            //Debug.Log("Logged " + position + " as collected");
            validPositions.Add(position);
            totalValidWeight += position.x + position.y;
            Collectables.Remove(collectable);
        }

        #region Object Pooling
        /// <summary>
        /// Gets a collectable GameObject from the object pool.
        /// </summary>
        /// <returns>The collectable from the pool.</returns>
        private Collectable GetCollectable()
        {
            Collectable toGet = collectablePool.Count > 0 ? collectablePool.Dequeue() : 
                Instantiate(GetRandomPrefab(), transform);
            return toGet;
        }

        /// <summary>
        /// Gets a random collectable prefab to use to spawn a new collectable.
        /// </summary>
        /// <returns></returns>
        private Collectable GetRandomPrefab()
        {
            return collectablePrefabs[Random.Range(0, collectablePrefabs.Length)];
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
