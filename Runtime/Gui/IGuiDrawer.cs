using System.Collections.Generic;

namespace MossWolfGames.Shared.Runtime.Gui
{
    public interface IGuiDrawer
    {
        IReadOnlyList<IGuiDrawable> DrawList { get; }

        void RegisterDrawable(IGuiDrawable drawable, IGuiDrawable parent = null);

        void RegisterDrawableBackground(IGuiDrawable drawable, IGuiDrawable parent = null);

        bool UnRegisterDrawable(IGuiDrawable drawable);

        void ClearAll();

        void Draw(GuiDrawViewModel drawArea);
    }
}