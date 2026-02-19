/*****************************************************************************
// File Name : ClassDropdownAttributeDrawer.cs
// Author : Eli Koederitz
// Creation Date : 12/30/2025
// Last Modified : 12/30/2025
//
// Brief Description : Draws a custom dropdown for an abstract base class and any relevant serialized properties.
// Based on an article at: https://medium.com/@modyari/unity-workflow-customization-robust-custom-dropdowns-to-unlock-manageable-architecture-f16c183b993a 
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GGL.Minotaur.Editor
{
    [CustomPropertyDrawer(typeof(ClassDropdownAttribute))]
    public class ClassDropdownAttributeDrawer : PropertyDrawer
    {
        private bool isInitialized;
        private int selectionIndex;
        private string[] selectionOptions;
        private List<Type> selectionTypes;

        /// <summary>
        /// Draws the class dropdown and relevant properties, if any.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Perform initialization if we haven't already.
            if (!isInitialized)
            {
                Initialize(property);
            }

            UpdateSelectionIndex(property);

            // Draw the dropdown
            Rect dropdownRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.BeginChangeCheck();
            selectionIndex = EditorGUI.Popup(dropdownRect, property.displayName, selectionIndex, selectionOptions);
            // If the user made a change to the popup selection:
            if (EditorGUI.EndChangeCheck())
            {
                // Creates an instance of the type the user selected and assign it to the property value.
                property.managedReferenceValue = Activator.CreateInstance(selectionTypes[selectionIndex]);
                property.serializedObject.ApplyModifiedProperties();
            }

            // Draw the subclass serialized fields.
            if (property.managedReferenceValue != null)
            {
                EditorGUI.indentLevel++;

                // Calculate the starting y position for the child fields.
                float yOffset = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                
                void DrawProperty(SerializedProperty property)
                {
                    float childHeight = EditorGUI.GetPropertyHeight(property, true);
                    Rect childRect = new Rect(position.x, yOffset, position.width, childHeight);

                    // Draw the child property field.
                    EditorGUI.PropertyField(childRect,property, true);

                    // Move to the next property.
                    yOffset += childHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                LoopForProperties(DrawProperty, property);

                EditorGUI.indentLevel--;
            }
        }

        /// <summary>
        /// Specifies the total height taken up by this property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            // If we have a class selected, then loop through all of it's properties and add their heights to the total.
            if (property.managedReferenceValue != null)
            {
                void IncreaseHeight(SerializedProperty ppt)
                {
                    height += EditorGUI.GetPropertyHeight(ppt, true) + EditorGUIUtility.standardVerticalSpacing;
                }

                LoopForProperties(IncreaseHeight, property);
            }
            return height;
        }

        /// <summary>
        /// Loops through the child properties in a given SerializedProperty and calls a propertyFunc on 
        /// copies of the property.
        /// </summary>
        /// <param name="propertyFunc">The function to call on copies of the base property's children.</param>
        /// <param name="baseProperty">The base property to run the function on the children of.</param>
        private void LoopForProperties(Action<SerializedProperty> propertyFunc, SerializedProperty baseProperty)
        {
            // Not sure why we need to copy the property.
            SerializedProperty copy = baseProperty.Copy();
            SerializedProperty end = copy.GetEndProperty();

            // Tracks if the loop should enter child properties. Only need to enter children the first loop, as
            // further children will be drawn automatically.
            bool enterChildren = true;
            // Loop over all the properties in the copy until we hit the end property.
            while (copy.NextVisible(enterChildren) && !SerializedProperty.EqualContents(copy, end))
            {
                //Skip the default m_Script property
                if (copy.name.Equals("m_Script", StringComparison.Ordinal))
                {
                    enterChildren = false;
                    continue;
                }

                propertyFunc(copy);

                enterChildren = false;
            }
        }

        /// <summary>
        /// Initializes the list of potential classes that this dropdodn can hold.
        /// </summary>
        /// <remarks>
        /// Done with a separate function so it only happens once and Assemblies aren;t being queried every update.
        /// </remarks>
        /// <param name="property">The SerializedProperty the attribute is on.</param>
        private void Initialize(SerializedProperty property)
        {
            ClassDropdownAttribute dda = (attribute as ClassDropdownAttribute);
            Type baseType = dda?.BaseType;

            // Create a temp list to add selection options to.
            List<string> tempSelections = new List<string>();

            selectionTypes = new List<Type>();

            if (dda.RestrictAssemblies)
            {
                // If RestrictAssemblies is true, only search the assembly that the base
                // type is in.
                foreach (Type type in Assembly.GetAssembly(baseType).GetTypes())
                {
                    if (!type.IsSubclassOf(baseType) || type.IsAbstract)
                    {
                        continue;
                    }

                    // Store the type and the type's name for use in the dropdown.
                    selectionTypes.Add(type);
                    string displayName = type.Name;

                    // Attempt to put the type into a given group in the dropdown.
                    if (Attribute.GetCustomAttribute(type, typeof(DropdownGroupAttribute)) is DropdownGroupAttribute dga)
                    {
                        displayName = dga.GroupName + "/" + displayName;
                    }
                    tempSelections.Add(displayName);
                }
            }
            else
            {
                // Loop through all types in all assemblies (More generic impmenentation).
                foreach (Assembly asmb in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (Type type in asmb.GetTypes())
                    {
                        // Skip classes that aren't subclasses of the base class and abstract classes.
                        if (!type.IsSubclassOf(baseType) || type.IsAbstract)
                        {
                            continue;
                        }

                        // Store the type and the type's name for use in the dropdown.
                        selectionTypes.Add(type);
                        string displayName = type.Name;
                        tempSelections.Add(displayName);
                    }
                }
            }

            // Convert the temp list into a permanent array.
            selectionOptions = tempSelections.ToArray();

            UpdateSelectionIndex(property);

            isInitialized = true;
        }

        /// <summary>
        /// Updates the index of the selected class to match the one stored in the property.
        /// </summary>
        private void UpdateSelectionIndex(SerializedProperty property)
        {
            object currentSubclass = property.managedReferenceValue;

            // If no subclass is selected, then pick the first type by default.
            if (currentSubclass == null)
            {
                selectionIndex = 0;
                SetPropertyValue(property, selectionIndex);
                return;
            }

            // If a subclass is selected, loop through our valid types array to find the correct index.
            selectionIndex = selectionTypes.IndexOf(currentSubclass.GetType());
            // If we fail to find the correct index, then throw an error that current subclass is invalid.
            if (selectionIndex == -1)
            {
                throw new ArrayTypeMismatchException("Could not find type " + currentSubclass + " in the type list.");
            }
        }

        /// <summary>
        /// Sets the value of the property to a given subclass instance specified by the type index.
        /// </summary>
        /// <param name="property">The property to set the value of.</param>
        /// <param name="typeIndex">The index of the subclass type to set to the property.</param>
        private void SetPropertyValue(SerializedProperty property, int typeIndex)
        {
            property.managedReferenceValue = Activator.CreateInstance(selectionTypes[typeIndex]);
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
