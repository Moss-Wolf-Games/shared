using UnityEngine;

namespace MossWolfGames.Shared.Runtime.Attributes
{
    /// <summary>
    /// Marks a field as not editable in the inspector but still visible
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class DisabledAttribute : PropertyAttribute
    {

    }
}