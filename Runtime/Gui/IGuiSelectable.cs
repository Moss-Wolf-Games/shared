using UnityEngine;

namespace MossWolfGames.Shared.Runtime.Gui
{
    public interface IGuiSelectable
    {
        bool Selected { get; set; }

        IGuiSelectable GetSelectableAtPosition(Vector2 mousePosition);
    }
}
