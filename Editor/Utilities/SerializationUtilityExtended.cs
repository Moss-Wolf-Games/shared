using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace MossWolfGames.Shared.Editor.Utilities
{
    public static class SerializationUtilityExtended
    {
        public static string GetPropertyName(string fieldName)
        {
            return $"<{fieldName}>k__BackingField";
        }
    }
}