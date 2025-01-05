using UnityEngine;

namespace MossWolfGames.Shared.Runtime.Gui
{
    public interface IGuiDrawable
    {
        bool Registered { get; set; }

        Rect Rect { get; }

        void OnRegistered(IGuiDrawer drawer);

        void OnUnRegistered(IGuiDrawer drawer);

        void Draw(GuiDrawViewModel drawArea);
    }
}