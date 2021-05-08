using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace instance.id.Extensions
{
    public static class CreateListView
    {
        public static ListView ToObjectListView<T>(this List<T> listItems,
            Action<IEnumerable<object>> itemChosen = null,
            Action<IEnumerable<object>> layerChanged = null,
            SelectionType selectionType = SelectionType.Single,
            int itemHeight = 16) where T : UnityEngine.Object
        {
            Action<VisualElement, int> bindItem;
            Func<VisualElement> makeItem = () => new Label();

            bindItem = (e, i) => (e as Label).text = listItems[i].name.ToString();

            var listView = new ListView(listItems, itemHeight, makeItem, bindItem);
            listView.selectionType = selectionType;
            listView.onItemsChosen += itemChosen ?? DefaultItemChosen;
            listView.onSelectionChange += layerChanged ?? DefaultSelectionChanged;
            listView.style.flexGrow = 1.0f;
            return listView;
        }

        public static ListView ToDataListView<T>(this List<T> listItems,
            Action<IEnumerable<object>> itemChosen = null,
            Action<IEnumerable<object>> layerChanged = null,
            SelectionType selectionType = SelectionType.Single,
            int itemHeight = 16)
        {
            Action<VisualElement, int> bindItem;
            Func<VisualElement> makeItem = () => new Label();

            bindItem = (e, i) => (e as Label).text = listItems[i].ToString();

            var listView = new ListView(listItems, itemHeight, makeItem, bindItem);
            listView.selectionType = selectionType;
            listView.onItemsChosen += itemChosen ?? DefaultItemChosen;
            listView.onSelectionChange += layerChanged ?? DefaultSelectionChanged;
            listView.style.flexGrow = 1.0f;
            return listView;
        }


        static void DefaultItemChosen(IEnumerable<object> obj)
        {
            Debug.Log(obj);
        }

        static void DefaultSelectionChanged(IEnumerable<object> obj)
        {
            Debug.Log(obj);
        }
    }
}
