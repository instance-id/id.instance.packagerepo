using System;
using System.Collections.Generic;
using System.Linq;

namespace instance.id.Extensions
{
    /// <summary>
    /// Maintains an inventory of current listener types inheriting the IEventListener interface
    /// </summary>
    internal static class EventCache
    {
        private static Dictionary<Type, List<Type>> listenerTypes = new Dictionary<Type, List<Type>>();
        public static List<Type> GetListenerTypes(IEventListener eventListener)
        {
            var listenerType = eventListener.GetType();
            if (listenerTypes.ContainsKey(listenerType)) return listenerTypes[listenerType];

            var subscriberTypes = listenerType.GetInterfaces()
                .Where(t => t.GetInterfaces().Contains(typeof(IEventListener)))
                .ToList();

            listenerTypes[listenerType] = subscriberTypes;
            return subscriberTypes;
        }
    }
}
