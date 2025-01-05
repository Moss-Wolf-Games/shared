using MossWolfGames.Shared.Runtime.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MossWolfGames.Shared.Runtime.Gui
{
    //TODO handle multi select
    public class GuiSelectHandler
    {
        private readonly IObjectHolder selectableObjectHolder;
        private readonly List<IGuiSelectable> selectableList = new List<IGuiSelectable>();

        private Predicate<IGuiSelectable> delayedSelectPredicate;

        public IGuiSelectable ActiveSelectable { get; private set; }

        public GuiSelectHandler(IObjectHolder selectableObjectHolder)
        {
            this.selectableObjectHolder = selectableObjectHolder;
        }

        public bool Update() 
        {
            // TODO put into control settings object
            if (Event.current.button != 0)
            {
                return false;
            }

            selectableList.Clear();
            selectableObjectHolder.GetObjectsWithType(selectableList);

            if (delayedSelectPredicate != null)
            {
                IGuiSelectable selectableFound = selectableObjectHolder.FindObjectMatchingReverseOrder<IGuiSelectable>(delayedSelectPredicate);
                if(selectableFound != null)
                {
                    Select(selectableFound);
                    delayedSelectPredicate = null;
                }
            }

            switch (Event.current.type) 
            {
                case EventType.MouseUp:
                    SelectAtMouse(Event.current.mousePosition);
                    return true;
                default:
                    return false;
            }
        }

        public void Select(IGuiSelectable newSelectable)
        {
            if (newSelectable == ActiveSelectable)
            {
                return;
            }

            if (ActiveSelectable != null)
            {
                ActiveSelectable.Selected = false;
            }

            ActiveSelectable = newSelectable;

            if (ActiveSelectable != null)
            {
                ActiveSelectable.Selected = true;
            }
        }

        public void DelayedSelect(Predicate<IGuiSelectable> delayedSelectPredicate)
        {
            this.delayedSelectPredicate = delayedSelectPredicate;
        }

        private void SelectAtMouse(Vector2 mousePosition)
        {
            IGuiSelectable newSelectable = GetSelectableAtMousePosition(mousePosition);
            Select(newSelectable);
        }

        private IGuiSelectable GetSelectableAtMousePosition(Vector2 mousePosition) 
        {
            for (int i = selectableList.Count - 1; i >= 0; i--)
            {
                IGuiSelectable selectable = selectableList[i];
                IGuiSelectable selectableAtPosition = selectable.GetSelectableAtPosition(mousePosition);
                if (selectableAtPosition != null)
                {
                    return selectableAtPosition;
                }
            }

            return null;
        }
    }
}