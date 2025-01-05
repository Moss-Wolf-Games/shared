using UnityEngine;

namespace MossWolfGames.Shared.Runtime.Gui
{
    public interface IGuiDraggable
    {
        IGuiDraggable GetDraggableAtPosition(Vector2 mousePosition);

        void BeginDrag(Vector2 mousePosition);

        void Drag(Vector2 mousePosition);

        void EndDrag(Vector2 mousePosition);
    }
}