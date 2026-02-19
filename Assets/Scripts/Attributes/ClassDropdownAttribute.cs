/*****************************************************************************
// File Name : ClassDropdownAttribute.cs
// Author : Eli Koederitz
// Creation Date : 12/30/2025
// Last Modified : 12/30/2025
//
// Brief Description : Custom attribute that allows for selecting a modifiable child class from a dropdown menu.
*****************************************************************************/
using System;
using UnityEngine;

namespace GGL.Minotaur
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ClassDropdownAttribute : PropertyAttribute
    {
        public Type BaseType { get; }
        public bool RestrictAssemblies { get; }

        public ClassDropdownAttribute(Type baseType) : this(baseType, true)
        {
        }

        public ClassDropdownAttribute(Type baseType, bool restrictAssemblies)
        {
            BaseType = baseType;
            RestrictAssemblies = restrictAssemblies;
        }
    }
}
