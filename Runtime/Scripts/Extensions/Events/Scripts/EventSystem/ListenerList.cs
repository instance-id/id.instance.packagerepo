using System.Collections.Generic;
using instance.id.Extensions;
using UnityEngine;

namespace instance.id.Extensions
{
    // ------------------------------------------------------- ListenerList

    internal class ListenerList<TListener> where TListener : class
    {
        private bool isComplete = false;
        public bool isInvoked;
        public readonly List<TListener> listenerList = new List<TListener>();

        public void Add(TListener listener)
        {
            if(!listenerList.TryAddValue(listener))
                Debug.LogWarning($"Unable to add {listener} to event listener list");
        }

        public void Remove(TListener listener)
        {
            if (isInvoked)
            {
                var i = listenerList.IndexOf(listener);
                if (i < 0) return;
                isComplete = true;
                listenerList[i] = null;
            }
            else listenerList.Remove(listener);
        }

        public void Complete()
        {
            if (!isComplete) return;

            listenerList.RemoveAll(s => s == null);
            isComplete = false;
        }
    }
}
