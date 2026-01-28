/*****************************************************************************
// File Name : VectorLabelsAttribute.cs
// Author : Brandon Koederitz
// Creation Date : 1/28/2026
// Last Modified : 1/28/2026
//
// Brief Description : Custom Attribute to override the default x, y, z, w labels of a serialized vector.
*****************************************************************************/
using UnityEngine;

namespace CustomAttributes
{
    public class VectorLabelsAttribute : PropertyAttribute
    {
        public string[] Lables {  get; private set; }

        public VectorLabelsAttribute(params string[] labels)
        {
            this.Lables = labels;
        }
    }
}
