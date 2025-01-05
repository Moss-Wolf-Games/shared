using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MossWolfGames.Shared.Editor.Extensions
{
    public static class SerializedObjectExtensions
    {
        public static SerializedProperty FindPropertyByManagedReferenceValueRecursive(this SerializedObject serializedObject, object propertyValue)
        {
            SerializedProperty property = serializedObject.GetIterator();
            return property.FindPropertyByManagedReferenceValueRecursive(propertyValue);
        }
    }
}