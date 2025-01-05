using MossWolfGames.Shared.Runtime.Extensions;
using UnityEngine;

namespace MossWolfGames.Shared.Runtime.Gui
{
    public static class GuiDrawViewModelExtensions
    {
        public static Rect Modify(this GuiDrawViewModel drawArea, Rect rect)
        {
            rect.x -= drawArea.PanPosition.x;
            rect.y -= drawArea.PanPosition.y;
            rect = rect.ZoomAround(drawArea.ActualZoom, drawArea.CanvasRect.center);
            return rect;
        }
    }
}