using UnityEditor;
using MossWolfGames.Shared.Runtime.Attributes;
using UnityEngine;

namespace MossWolfGames.Shared.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(DisabledAttribute), useForChildren: true)]
    public class DisabledAttributePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool wasEnabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label: new GUIContent(property.displayName), includeChildren: true);
            GUI.enabled = wasEnabled;
        }
    }
}