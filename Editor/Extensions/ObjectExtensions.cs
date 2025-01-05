using MossWolfGames.Shared.Editor.Utilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MossWolfGames.Shared.Editor.Extensions
{
    public static class ObjectExtensions
    {
        public static void AddManagedReferenceToList<T, U>(this T parentObject, string listPropertyName, U newManagedReference) where T : Object where U : class
        {
            SerializedObject serializedObject = new SerializedObject(parentObject);
            SerializedProperty listProperty = serializedObject.FindProperty(listPropertyName);

            listProperty.arraySize++;
            SerializedProperty nodeProperty = listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
            nodeProperty.managedReferenceValue = newManagedReference;

            serializedObject.ApplyModifiedProperties();
        }

        public static bool DeleteManagedReference<T, U>(this T parentObject, U managedReference) where T : Object where U : class
        {
            SerializedObject serializedObject = new SerializedObject(parentObject);
            SerializedProperty referenceProperty = serializedObject.FindPropertyByManagedReferenceValueRecursive(managedReference);
            SerializedProperty referenceListProperty = referenceProperty.FindParentProperty();
            int indexFound = referenceListProperty.GetArrayIndexOfManagedReference(managedReference);

            if (indexFound != -1)
            {
                referenceListProperty.DeleteArrayElementAtIndex(indexFound);
                serializedObject.ApplyModifiedProperties();
                return true;
            }

            return false;
        }

        public static void DeleteManagedReferencesWithIntId<T>(this T parentObject, string listPropertyName, int id, params string[] idPropertyNames) where T : Object
        {
            SerializedObject serializedObject = new SerializedObject(parentObject);
            SerializedProperty listProperty = serializedObject.FindProperty(listPropertyName);
            for (int i = listProperty.arraySize - 1; i >= 0; i--)
            {
                SerializedProperty connectionProperty = listProperty.GetArrayElementAtIndex(i);

                bool idFound = false;
                for (int idIndex = 0; idIndex < idPropertyNames.Length; idIndex++)
                {
                    string idPropertyName = idPropertyNames[idIndex];
                    SerializedProperty idProperty = connectionProperty.FindPropertyRelative(SerializationUtilityExtended.GetPropertyName(idPropertyName));
                    if (idProperty.intValue == id)
                    {
                        idFound = true;
                        break;
                    }
                }

                if (idFound)
                {
                    listProperty.DeleteArrayElementAtIndex(i);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }

        public static void DrawManagedReferenceWithId<T>(this T parentObject, string listPropertyName, string idPropertyName, int id) where T : Object
        {
            SerializedObject serializedObject = new SerializedObject(parentObject);
            SerializedProperty listProperty = serializedObject.FindProperty(listPropertyName);

            for (int i = 0; i < listProperty.arraySize; i++)
            {
                SerializedProperty subProperty = listProperty.GetArrayElementAtIndex(i);
                SerializedProperty idProperty = subProperty.FindPropertyRelative(SerializationUtilityExtended.GetPropertyName(idPropertyName));

                if (idProperty.intValue == id)
                {
                    DrawProperty(serializedObject, subProperty);
                    return;
                }
            }
        }

        private static void DrawProperty(SerializedObject serializedObject, SerializedProperty parentProperty)
        {
            IEnumerable<SerializedProperty> childPropertyList = parentProperty.GetChildren();
            foreach (SerializedProperty childProperty in childPropertyList)
            {
                EditorGUILayout.PropertyField(childProperty);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
