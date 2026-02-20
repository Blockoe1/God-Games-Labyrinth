/*****************************************************************************
// File Name : PathfinderMovement.cs
// Author : Brandon Koederitz
// Creation Date : 2/17/2026
// Last Modified : 2/17/2026
//
// Brief Description : Specialized movement controller for entities that pathfind along a grid.
*****************************************************************************/
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GGL.Minotaur
{
    public class PathfinderMovement : EntityMovement
    {
        #region CONSTS
        private const float REQUIRED_PATH_DIST = 0.45f;
        #endregion

        [Header("Pathfinder Settings")]
        [SerializeField, Tooltip("The tilemap to use as wall based tiles.")] private Tilemap collisionTilemap;

        public event Action<Vector2> OnReachNode;
        public event Action OnCompletePath;

        private Vector2[] currentPath;
        private int currentPathNode;

        /// <summary>
        /// Sets a destination position to navigate this entity to.
        /// </summary>
        /// <param name="destination">The destination position.</param>
        public void SetDestination(Vector2 destination)
        {
            currentPathNode = 0;
            currentPath = Pathfinder.FindPath(collisionTilemap, rb.position, destination);
            // If not already moving, set moving and start the follow coroutine.
            if (!IsMoving)
            {
                IsMoving = true;
                StartCoroutine(FollowPathRoutine());
            }
        }

        /// <summary>
        /// Stops this entity's movement along the path.
        /// </summary>
        public void Stop()
        {
            IsMoving = false;
            currentPath = null;
            currentPathNode = 0;
        }

        /// <summary>
        /// Continually manages this entity's movement as it follows a given path.
        /// </summary>
        /// <returns></returns>
        private IEnumerator FollowPathRoutine()
        {
            while (IsMoving && currentPath != null && currentPathNode < currentPath.Length)
            {
                Pathfinder.DrawPath(currentPath);

                Debug.Log(currentPath[currentPathNode] + " " + currentPathNode);

                TargetDirection = GetDirection(currentPath[currentPathNode], rb.position);

                // If we've reached the next node in the path, update the target direction and move to the next node.
                if (Vector2.Distance(rb.position, currentPath[currentPathNode]) < REQUIRED_PATH_DIST)
                {
                    // Only call the event for nodes after the first, as the first node is the current position.
                    if (currentPathNode > 0)
                    {
                        OnReachNode?.Invoke(currentPath[currentPathNode]);
                    }
                    currentPathNode++;

                    // Get a new path if we've reached the end of this current path.
                    if (currentPathNode >= currentPath.Length)
                    {
                        // Broadcast an on complete event when the end of the path is reached.
                        OnCompletePath?.Invoke();
                        continue;
                    }

                    TargetDirection = GetDirection(currentPath[currentPathNode],
                        currentPath[currentPathNode - 1]);
                }

                // Move along the path based on physics movement.
                yield return new WaitForFixedUpdate();
            }

            Stop();
        }

        /// <summary>
        /// Gets the direction that the minotaur should move in based on two path points.
        /// </summary>
        /// <param name="targetPathPoint">The target path point to move to.</param>
        /// <param name="currentPathPoint">The current path point.</param>
        /// <returns>The orthogonal direction the minotaur should move in.</returns>
        public static Vector2 GetDirection(Vector2 targetPathPoint, Vector2 currentPathPoint)
        {
            Vector2 toVector = targetPathPoint - currentPathPoint;
            return Mathf.Abs(toVector.x) > Mathf.Abs(toVector.y) ?
                new Vector2(System.MathF.Sign(toVector.x), 0) : new Vector2(0, System.MathF.Sign(toVector.y));
        }
    }
}
