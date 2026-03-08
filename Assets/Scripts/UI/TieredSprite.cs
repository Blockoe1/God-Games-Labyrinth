/*****************************************************************************
// File Name : TieredSprite.cs
// Author : Brandon Koederitz
// Creation Date : 1/28/2026
// Last Modified : 1/28/2026
//
// Brief Description : Interpolates between different sprites for an image based on a normalized value.
*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;

namespace GGL.UI
{
    public class TieredSprite : MonoBehaviour
    {
        [SerializeField] private Image targetImage;
        [SerializeField] private Sprite[] sprites;

        private float spriteAmount;

        #region Properties
        public float SpriteAmount
        {
            get { return spriteAmount; }
            set 
            { 
                spriteAmount = Mathf.Clamp01(value);
                SetSprite(spriteAmount);
            }
        }
        #endregion

        /// <summary>
        /// Sets the sprite of the image to a given sprite based on a normalizedd valie.
        /// </summary>
        /// <param name="value"></param>
        private void SetSprite(float value)
        {
            int spriteIndex = (int)(value * (sprites.Length - 1));
            //Debug.Log(spriteIndex);
            targetImage.sprite = sprites[spriteIndex];
        }
    }
}
