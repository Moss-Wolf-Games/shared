using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace MossWolfGames.Shared.Editor.Extensions
{
    public static class SerializedPropertyExtensions
    {
        public static IEnumerable<SerializedProperty> GetChildren(this SerializedProperty property)
        {
            property = property.Copy();
            var nextElement = property.Copy();
            bool hasNextElement = nextElement.NextVisible(false);
            if (!hasNextElement)
            {
                nextElement = null;
            }

            property.NextVisible(true);
            while (true)
            {
                if ((SerializedProperty.EqualContents(property, nextElement)))
                {
                    yield break;
                }

                yield return property;

                bool hasNext = property.NextVisible(false);
                if (!hasNext)
                {
                    break;
                }
            }
        }

        public static SerializedProperty FindPropertyByManagedReferenceValueRecursive(this SerializedProperty property, object propertyValue)
        {
            switch(property.propertyType)
            {
                case SerializedPropertyType.ManagedReference:
                    if(property.managedReferenceValue == propertyValue)
                    {
                        return property;
                    }
                    break;
            }

            bool hasNextElement = property.Next(true);
            if (hasNextElement)
            {
                return property.FindPropertyByManagedReferenceValueRecursive(propertyValue);
            }

            return null;
        }

        public static SerializedProperty FindParentProperty(this SerializedProperty serializedProperty)
        {
            string[] propertyPaths = serializedProperty.propertyPath.Split('.');
            if (propertyPaths.Length <= 1)
            {
                return default;
            }

            SerializedProperty parentSerializedProperty = serializedProperty.serializedObject.FindProperty(propertyPaths.First());
            for (int index = 1; index < propertyPaths.Length - 1; index++)
            {
                if (propertyPaths[index] == "Array")
                {
                    if (index + 1 == propertyPaths.Length - 1)
                    {
                        break;
                    }
                    if (propertyPaths.Length > index + 1 && Regex.IsMatch(propertyPaths[index + 1], "^data\\[\\d+\\]$"))
                    {
                        Match match = Regex.Match(propertyPaths[index + 1], "^data\\[(\\d+)\\]$");
                        int arrayIndex = int.Parse(match.Groups[1].Value);
                        parentSerializedProperty = parentSerializedProperty.GetArrayElementAtIndex(arrayIndex);
                        index++;
                    }
                }
                else
                {
                    parentSerializedProperty = parentSerializedProperty.FindPropertyRelative(propertyPaths[index]);
                }
            }

            return parentSerializedProperty;
        }

        public static int GetArrayIndexOfManagedReference(this SerializedProperty serializedProperty, object managedReference)
        {
            for (int i = 0; i < serializedProperty.arraySize; i++)
            {
                SerializedProperty prop = serializedProperty.GetArrayElementAtIndex(i);
                if (prop.managedReferenceValue == managedReference)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
