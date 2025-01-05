using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal.Profiling.Memory.Experimental;

namespace MossWolfGames.Shared.Editor.Gui
{
    public class StringListSearchProvider : AdvancedDropdown
    {
        private class ItemReference
        {
            public string ItemName;
            public Action<object> Action;
            public object Object;
        }

        private readonly Dictionary<string, ItemReference> itemReferenceTable = new Dictionary<string, ItemReference>();

        private AdvancedDropdownItem root;

        public StringListSearchProvider(string listName, AdvancedDropdownState state) : base(state)
        {
            root = new AdvancedDropdownItem(listName);
        }

        /// <summary>
        /// Adds an option while ignoring sub menus. Use this if there are a lot of options and none are in sub menus
        /// </summary>
        public void AddSimple(string itemName, Action<object> onSelected, object selectedObject)
        {
            if(itemReferenceTable.ContainsKey(itemName))
            {
                // TODO Add duplicates
                return;
            }

            AdvancedDropdownItem newItem = new AdvancedDropdownItem(itemName);
            root.AddChild(newItem);

            itemReferenceTable.Add(itemName, new ItemReference
            {
                ItemName = itemName,
                Action = onSelected,
                Object = selectedObject,
            });
        }

        public void Add(string itemName, Action<object> onSelected, object selectedObject)
        {
            string[] pathSplit = itemName.Split(new char[] { '/','\\'}, StringSplitOptions.RemoveEmptyEntries);

            AdvancedDropdownItem currentItem = root;

            for(int i = 0; i < pathSplit.Length; i++)
            {
                string newName = pathSplit[i];
                AdvancedDropdownItem newItem = currentItem.children.FirstOrDefault(x => x != null && x.name.Equals(newName));
                if(newItem == null)
                {
                    newItem = new AdvancedDropdownItem(newName);
                    currentItem.AddChild(newItem);
                }
                currentItem = newItem;

                if(i == pathSplit.Length-1)
                {
                    if(itemReferenceTable.ContainsKey(newItem.name))
                    {
                        UnityEngine.Debug.LogError($"A node with the name {newItem.name} already exists");
                        continue;
                    }

                    itemReferenceTable.Add(newItem.name, new ItemReference
                    {
                        ItemName = newItem.name,
                        Action = onSelected,
                        Object = selectedObject,
                    });
                }
            }
        }

        public void Clear()
        {
            // TODO Figure out a less wasteful way to do this.
            root = new AdvancedDropdownItem(root.name);
            itemReferenceTable.Clear();
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            base.ItemSelected(item);

            if(itemReferenceTable.ContainsKey(item.name))
            {
                ItemReference reference = itemReferenceTable[item.name];
                reference.Action?.Invoke(reference.Object);
            }
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            return root;
        }
    }
}