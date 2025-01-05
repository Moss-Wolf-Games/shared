using UnityEditor;
using UnityEngine;

namespace MossWolfGames.Shared.Editor.Extensions
{
    public static class ScriptableObjectExtensions
    {
        public static U CreateManagedReferenceDeepCopy<T, U>(this T parentScriptableObject, U managedReference) where T : ScriptableObject where U : class
        {
            SerializedObject serializedObject = new SerializedObject(parentScriptableObject);
            SerializedProperty referenceProperty = serializedObject.FindPropertyByManagedReferenceValueRecursive(managedReference);
            SerializedProperty referenceListProperty = referenceProperty.FindParentProperty();
            int indexFound = referenceListProperty.GetArrayIndexOfManagedReference(managedReference);

            string listPropertyName = referenceListProperty.propertyPath;
            T duplicateScriptableObject = ScriptableObject.Instantiate(parentScriptableObject);
            serializedObject = new SerializedObject(duplicateScriptableObject);
            referenceListProperty = serializedObject.FindProperty(listPropertyName);
            referenceProperty = referenceListProperty.GetArrayElementAtIndex(indexFound);
            U copiedObject = referenceProperty.managedReferenceValue as U;

            ScriptableObject.DestroyImmediate(duplicateScriptableObject);

            return copiedObject;
        }
    }
}