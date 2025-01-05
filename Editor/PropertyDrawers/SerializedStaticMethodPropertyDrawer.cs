using MossWolfGames.Shared.Editor.Gui;
using MossWolfGames.Shared.Editor.Utilities;
using MossWolfGames.Shared.Runtime.DataTypes;
using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace MossWolfGames.Shared.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(SerializedStaticMethod))]
    public class SerializedStaticMethodPropertyDrawer : PropertyDrawer
    {
        private const int Padding = 2;
        private const int IndentSize = 10;
        private const float HeaderHeight = 10;

        private readonly StringListSearchProvider typeSearchProvider = new StringListSearchProvider("Types", new AdvancedDropdownState());
        private readonly StringListSearchProvider methodSearchProvider = new StringListSearchProvider("Methods", new AdvancedDropdownState());

        private System.Type[] allTypes;

        private Type typeToSet;
        private MethodInfo methodToSet;
        private Type currentMethodType;

        private float Height => EditorGUIUtility.singleLineHeight * 2 + Padding * 4 + HeaderHeight;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return Height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty typeNameProperty = property.FindPropertyRelative(SerializationUtilityExtended.GetPropertyName(nameof(SerializedStaticMethod.TypeName)));
            SerializedProperty methodNameProperty = property.FindPropertyRelative(SerializationUtilityExtended.GetPropertyName(nameof(SerializedStaticMethod.MethodName)));

            System.Type targetType = System.Type.GetType(typeNameProperty.stringValue);

            

            if (allTypes == null)
            {
                InitializeTypes();
            }

            if(typeToSet != null)
            {
                typeNameProperty.stringValue = typeToSet.AssemblyQualifiedName;
                targetType = typeToSet;
                typeToSet = null;
            }
            if(methodToSet != null)
            {
                MethodInfo foundMethod = targetType.GetMethod(methodToSet.Name, new Type[0]);
                if(foundMethod != null)
                {
                    methodNameProperty.stringValue = foundMethod.Name;
                }
                methodToSet = null;
            }

            Rect boxRect = position;
            boxRect.x += IndentSize * EditorGUI.indentLevel;
            boxRect.width -= IndentSize * EditorGUI.indentLevel;
            GUI.Box(boxRect, string.Empty, EditorStyles.helpBox);

            // Header
            Rect headerRect = new Rect(position.x, position.y + Padding, position.width, HeaderHeight);
            EditorGUI.LabelField(headerRect, property.displayName, EditorStyles.centeredGreyMiniLabel);

            // Class Type
            Rect classTypeRect = new Rect(position.x, position.y + Padding * 2 + HeaderHeight, position.width, EditorGUIUtility.singleLineHeight);
            if(DropdownWithLabel(classTypeRect, "Class", targetType != null ? targetType.Name : "None"))
            {
                typeSearchProvider.Show(new Rect(position.x, position.y, 300, Height));
            }

            if(currentMethodType != targetType)
            {
                currentMethodType = targetType;
                InitializeMethods(currentMethodType);
            }

            MethodInfo method = targetType != null && !string.IsNullOrEmpty(methodNameProperty.stringValue) ? targetType.GetMethod(methodNameProperty.stringValue) : null;

            // Method Type
            Rect methodRect = new Rect(position.x, position.y + Padding * 3 + HeaderHeight + EditorGUIUtility.singleLineHeight * 1, position.width, EditorGUIUtility.singleLineHeight);
            if (DropdownWithLabel(methodRect, "Method", method != null ? method.Name : "None"))
            {
                methodSearchProvider.Show(new Rect(position.x, position.y, 300, Height));
            }
        }

        private bool DropdownWithLabel(Rect rect, string label, string content)
        {
            float currentLabelWidth = EditorGUIUtility.labelWidth;
            Rect labelRect = new Rect(rect.x, rect.y, currentLabelWidth, rect.height);
            Rect dropdownRect = new Rect(rect.x + currentLabelWidth, rect.y, rect.width - currentLabelWidth, rect.height);


            EditorGUI.LabelField(labelRect, label);
            return EditorGUI.DropdownButton(dropdownRect, new GUIContent(content), FocusType.Passive);
        }

        private void InitializeTypes()
        {
            typeSearchProvider.Clear();

            allTypes = EditorClassUtility.FindAlphabeticalUnityCompiledClassTypes(onlyTypesWithStaticMethods:true).ToArray();
            for (int i = 0; i < allTypes.Length; i++)
            {
                // TODO Assembly name for duplicates
                typeSearchProvider.AddSimple(allTypes[i].Name, OnTypeSelected, allTypes[i]);
            }
        }

        private void InitializeMethods(Type currentType)
        {
            methodSearchProvider.Clear();

            MethodInfo[] methods = currentType.GetMethods();
            for(int i = 0; i < methods.Length; i++)
            {
                MethodInfo method = methods[i];
                if(method.GetParameters().Length != 0)
                {
                    continue;
                }
                if(method.IsStatic)
                {
                    methodSearchProvider.AddSimple(method.Name, OnMethodSelected, method);
                }
            }
        }

        private void OnTypeSelected(object typeObject)
        {
            typeToSet = (Type)typeObject;
        }

        private void OnMethodSelected(object methodObject)
        {
            methodToSet = (MethodInfo)methodObject;
        }
    }
}