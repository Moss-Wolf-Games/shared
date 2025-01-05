using MossWolfGames.Shared.Runtime.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MossWolfGames.Shared.Runtime.Gui
{
    //TODO handle multi select
    public class GuiDragHandler
    {
        private readonly IObjectHolder draggableObjectHolder;
        private readonly List<IGuiDraggable> draggableList = new List<IGuiDraggable>();

        private IGuiDraggable activeDraggable;

        public GuiDragHandler(IObjectHolder draggableObjectHolder)
        {
            this.draggableObjectHolder = draggableObjectHolder;
        }

        public bool Update()
        {
            // TODO put into control settings object
            if (Event.current.button != 0)
            {
                return false;
            }

            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    activeDraggable = FindDraggableAt(Event.current.mousePosition);
                    if (activeDraggable != null)
                    {
                        activeDraggable.BeginDrag(Event.current.mousePosition);
                    }
                    return true;
                case EventType.MouseDrag:
                    if(activeDraggable != null)
                    {
                        activeDraggable.Drag(Event.current.mousePosition);
                    }
                    return true;
                case EventType.MouseUp:
                    if (activeDraggable != null)
                    {
                        activeDraggable.EndDrag(Event.current.mousePosition);
                    }
                    return true;
                default:
                    return false;
            }
        }

        private IGuiDraggable FindDraggableAt(Vector2 mousePosition)
        {
            draggableList.Clear();
            draggableObjectHolder.GetObjectsWithType(draggableList);

            for(int i = draggableList.Count -1; i >= 0; i--)
            {
                IGuiDraggable draggable = draggableList[i];
                IGuiDraggable draggableAtPosition = draggable.GetDraggableAtPosition(mousePosition);
                if (draggableAtPosition != null)
                {
                    return draggableAtPosition;
                }
            }
            return null;
        }
    }
}