/*****************************************************************************
// File Name : VectorLabelsAttributeDrawer.cs
// Author : Brandon Koederitz
// Creation Date : 1/28/2026
// Last Modified : 1/28/2026
//
// Brief Description : Drawer that replaces a Vector's default x, y, z, w labels with custom ones defined by an attrubute.
// Based on this unity forum post: https://discussions.unity.com/t/how-to-change-the-names-of-a-vector-3-that-is-set-in-the-inspector/216935/3
*****************************************************************************/
using UnityEngine;
using UnityEditor;

namespace CustomAttributes
{
    [CustomPropertyDrawer(typeof(VectorLabelsAttribute))]
    public class VectorLabelsAttributeEditor : PropertyDrawer
    {
        #region CONSTS
        private const int TWO_LINES_THRESHOLD = 375;
        #endregion

        private static GUIContent[] defaultLabels = new GUIContent[] {
            new GUIContent("X"),
            new GUIContent("Y"),
            new GUIContent("Z"),
            new GUIContent("W")
        };

        /// <summary>
        /// Gets the height of this property, takinginto account if the labels wrap to 2 lines.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Caluclate if this property's height should be doubled due to taking up to lines.
            float multiplier = Screen.width < TWO_LINES_THRESHOLD ? 2 : 1;
            return multiplier * base.GetPropertyHeight(property, label);
        }

        /// <summary>
        /// Draws the vector property on the Editor.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
        }

        private void DrawFields()
        {

        }
    }
}
