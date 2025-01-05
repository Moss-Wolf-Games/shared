using MossWolfGames.Shared.Runtime.Utilities;
using System.Collections.Generic;

namespace MossWolfGames.Shared.Runtime.Gui
{
    public class GuiDrawer : IObjectHolder, IGuiDrawer
    {
        private readonly List<IGuiDrawable> drawList = new List<IGuiDrawable>();

        private readonly Dictionary<IGuiDrawable, List<IGuiDrawable>> parentedTable = new Dictionary<IGuiDrawable, List<IGuiDrawable>>();

        public IReadOnlyList<IGuiDrawable> DrawList => drawList;

        public void RegisterDrawable(IGuiDrawable newDrawable, IGuiDrawable parent = null)
        {
            if (drawList.Contains(newDrawable))
            {
                return;
            }
            newDrawable.Registered = true;
            newDrawable.OnRegistered(this);
            drawList.Add(newDrawable);

            if(parent != null)
            {
                if(!parentedTable.ContainsKey(parent))
                {
                    parentedTable.Add(parent, new List<IGuiDrawable>());
                }
                parentedTable[parent].Add(newDrawable);
            }
        }

        public void RegisterDrawableBackground(IGuiDrawable newDrawable, IGuiDrawable parent = null)
        {
            if (drawList.Contains(newDrawable))
            {
                return;
            }
            if (drawList.Count == 0)
            {
                drawList.Add(newDrawable);
            }
            else
            {
                drawList.Insert(0, newDrawable);
            }
            newDrawable.Registered = true;
            newDrawable.OnRegistered(this);

            if (parent != null)
            {
                if (!parentedTable.ContainsKey(parent))
                {
                    parentedTable.Add(parent, new List<IGuiDrawable>());
                }
                parentedTable[parent].Add(newDrawable);
            }
        }

        public bool UnRegisterDrawable(IGuiDrawable drawable) 
        {
            if (drawList.Remove(drawable))
            {
                drawable.Registered = false;
                drawable.OnUnRegistered(this);

                if(parentedTable.ContainsKey(drawable))
                {
                    List<IGuiDrawable> childList = parentedTable[drawable];
                    parentedTable.Remove(drawable);
                    for(int i = 0; i < childList.Count; i++)
                    {
                        UnRegisterDrawable(childList[i]);
                    }
                }
                return true;
            }
            return false;
        }

        public void ClearAll()
        {
            parentedTable.Clear();
            for(int i = drawList.Count - 1; i >= 0; i--)
            {
                UnRegisterDrawable(drawList[i]);
            }
        }

        public void Draw(GuiDrawViewModel drawArea)
        {
            foreach(IGuiDrawable draw in drawList) 
            { 
                draw.Draw(drawArea);
            }
        }

        void IObjectHolder.GetObjects(List<object> objects)
        {
            objects.AddRange(drawList);
        }
    }
}
