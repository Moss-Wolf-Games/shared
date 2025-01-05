using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MossWolfGames.Shared.Editor.CustomEditors
{
    // TODO Potentially shift into editor hacks. Need to see how this interacts with the validation system
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class MonobehaviourEditorOverride : UnityEditor.Editor
    {
        private const string ScriptPropertyName = "PropertyField:m_Script";
        private const float ScriptOpacity = 0.5f;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement container = new VisualElement();
            InspectorElement.FillDefaultInspector(container, serializedObject, this);

            List<VisualElement> children = container.Query().ToList();
            foreach (VisualElement child in children)
            {
                if (child.name.Equals(ScriptPropertyName))
                {
                    child.SetEnabled(true);
                    child.style.opacity = ScriptOpacity;
                }
            }

            return container;
        }
    }
}