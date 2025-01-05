using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MossWolfGames.Shared.Runtime.Extensions
{
    public static class RectExtensions
    {
        public static Rect Expand(this Rect rect, float amount)
        {
            rect.x -= amount / 2;
            rect.y -= amount / 2;
            rect.width += amount;
            rect.height += amount;
            return rect;
        }

        public static Rect Zoom(this Rect rect, float zoom, Space space)
        {
            float zoomWidth = rect.width * (zoom - 1);
            float zoomHeight = rect.height * (zoom - 1);
            rect.x -= zoomWidth / 2;
            rect.y -= zoomHeight / 2;
            rect.width += zoomWidth;
            rect.height += zoomHeight;
            return rect;
        }

        public static Rect ZoomAround(this Rect rect, float zoom, Vector2 axis)
        {
            rect.x -= axis.x;
            rect.y -= axis.y;

            rect.xMin *= zoom;
            rect.xMax *= zoom;
            rect.yMin *= zoom;
            rect.yMax *= zoom;

            rect.x += axis.x;
            rect.y += axis.y;

            return rect;
        }
    }
}