using UnityEngine;

namespace MossWolfGames.Shared.Runtime.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 SnapToGrid(this Vector2 originalVector2, float gridSize)
        {
            originalVector2.x = originalVector2.x.SnapToGrid(gridSize);
            originalVector2.y = originalVector2.y.SnapToGrid(gridSize);
            return originalVector2;
        }
    }
}