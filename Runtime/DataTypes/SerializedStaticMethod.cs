using System;
using UnityEngine;
#if UNITY_EDITOR
using System.Reflection;
#endif

namespace MossWolfGames.Shared.Runtime.DataTypes
{
    [Serializable]
    public class SerializedStaticMethod
    {
        [field: SerializeField]
        public string TypeName { get; private set; }

        [field: SerializeField]
        public string MethodName { get; private set; }

#if UNITY_EDITOR
        /// <summary>
        /// Calls the static method. Editor only.
        /// </summary>
        public void Invoke_Editor()
        {
            System.Type windowType = System.Type.GetType(TypeName);
            if (windowType == null)
            {
                Debug.LogError($"Could not find class type {TypeName}. Try adding the assembly name if it isn't there");
                return;
            }

            MethodInfo method = windowType.GetMethod(MethodName);
            if (method == null)
            {
                Debug.LogError($"Could not find method {MethodName} in class {TypeName}");
                return;
            }

            method.Invoke(null, null);
        }
#endif
    }
}