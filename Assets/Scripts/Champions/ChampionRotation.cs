/*****************************************************************************
// File Name : Champion Rotation.cs
// Author : Brandon Koederitz
// Creation Date : 1/31/2026
// Last Modified : 1/31/2026
//
// Brief Description : Rotates the champion's visuals so that players know which way they are facing.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace GGL.Champions
{
    public class ChampionRotation : MonoBehaviour
    {
        [SerializeField] private GameObject rotationGameObject;
        [SerializeField] private float rotationSmoothTime = 0.25f;

        private Coroutine rotateRoutine;

        private float angleSpeed;  

        /// <summary>
        /// Sets the direction that this champion's visuals face.
        /// </summary>
        /// <remarks>
        /// Actual rotation is controlled by the ChampionMover.
        /// </remarks>
        /// <param name="direction">The direction to set this champion's rotation to.</param>
        public void SetRotation(Vector2 direction)
        {
            if (rotateRoutine != null)
            {
                StopCoroutine(rotateRoutine);
                rotateRoutine = null;
            }
            Debug.Log(MathHelpers.VectorToDegAngleUnity(direction));
            rotateRoutine = StartCoroutine(RotateRoutine(MathHelpers.VectorToDegAngle(direction)));
        }

        /// <summary>
        /// Continually rotates the player's graphics towards the target angle.
        /// </summary>
        /// <param name="targetAngle">The target angle to rotate towards.</param>
        /// <returns>Coroutine.</returns>
        public IEnumerator RotateRoutine(float targetAngle)
        {
            Vector3 eulers = rotationGameObject.transform.eulerAngles;
            while (!MathHelpers.ApproximatelyWithin(eulers.z, targetAngle, 0.1f))
            {
                eulers = rotationGameObject.transform.eulerAngles;
                eulers.z = Mathf.SmoothDampAngle(eulers.z, targetAngle, ref angleSpeed, rotationSmoothTime);
                rotationGameObject.transform.eulerAngles = eulers;
                yield return null;
            }

            // Snap to the target rotation.
            eulers.z = targetAngle;
            rotationGameObject.transform.eulerAngles = eulers;
            rotateRoutine = null;
        }
    }
}
