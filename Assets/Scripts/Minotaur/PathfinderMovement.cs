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
        private const float REQUIRED_PATH_DIST = 0.5f;
        #endregion

        [SerializeField, Tooltip("The tilemap to use as wall based tiles.")] private Tilemap collisionTilemap;

        public event Action<Vector2> OnReachNode;
        public event Action OnCompletePath;

        /// <summary>
        /// Sets a destination position to navigate this entity to.
        /// </summary>
        /// <param name="destination">The destination position.</param>
        public void SetDestination(Vector2 destination)
        {
            // If we weren't already following a path, start the follow routine.
            if (currentPath == null)
            {
                StartCoroutine(FollowPathRoutine());
            }
            currentPath = Pathfinder.FindPath(collisionTilemap, rb.position, destination);
        }

        /// <summary>
        /// Stops this entity's movement along the path.
        /// </summary>
        public void Stop()
        {

        }

        /// <summary>
        /// Continually manages this entity's movement as it follows a given path.
        /// </summary>
        /// <returns></returns>
        private IEnumerator FollowPathRoutine(Vector2[] path)
        {
            int currentPathNode = 0;
            // Delay a frame before beginning the follow routine.
            yield return new WaitForFixedUpdate();

            while (path != null && currentPathNode < currentPath.Length)
            {

            }
        }
    }
}
