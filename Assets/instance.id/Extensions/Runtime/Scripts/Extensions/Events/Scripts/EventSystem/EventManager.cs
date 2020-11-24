using System;
using System.Collections.Generic;
using instance.id.Extensions;
using UnityEngine;

namespace instance.id.Extensions
{
    public static class EventManager
    {
        private static Dictionary<Type, ListenerList<IEventListener>> listeners = new Dictionary<Type, ListenerList<IEventListener>>();

        public static void Subscribe(IEventListener subscriber)
        {
            var subscriberTypes = EventCache.GetListenerTypes(subscriber);
            foreach (var t in subscriberTypes)
            {
                if (!listeners.ContainsKey(t)) listeners[t] = new ListenerList<IEventListener>();
                listeners[t].Add(subscriber);
            }
        }

        public static void Unsubscribe(IEventListener subscriber)
        {
            var listenerTypes = EventCache.GetListenerTypes(subscriber);
            for (var i = 0; i < listenerTypes.Count; i++)
            {
                var t = listenerTypes[i];
                if (listeners.ContainsKey(t))
                    listeners[t].Remove(subscriber);
            }
        }

        public static void RaiseEvent<TListener>(Action<TListener> action) where TListener : class, IEventListener
        {
            if (!listeners.TryGetValue(typeof(TListener), out var listener))
            {
                Debug.LogWarning($"Unable to locate the listener of type: {typeof(TListener).Name}");
                // return;
            }

            listener.isInvoked = true;
            foreach (var l in listener.listenerList) // @formatter:off
            {
                try { action.Invoke(l as TListener); }
                catch (Exception e) { Debug.LogError(e); }
            } // @formatter:on

            listener.isInvoked = false;
            listener.Complete();
        }
    }
}
