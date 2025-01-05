using UnityEngine;

namespace MossWolfGames.Shared.Runtime.Extensions
{
    public static class FloatExtensions
    {
        public static float SnapToGrid(this float originalFloat, float gridSize)
        {
            return Mathf.Round(originalFloat / gridSize) * gridSize;
        }
    }
}