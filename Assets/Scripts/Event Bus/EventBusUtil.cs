using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Mert.EventBus
{
    /// <summary>
    /// This class is used to initialize all the event buses and clear them when needed.
    /// </summary>
    public class EventBusUtil
    {
        /// <summary>
        /// List of all the event types.
        /// </summary>
        public static IReadOnlyList<Type> EventTypes { get; set; }

        /// <summary>
        /// List of all the event bus types.
        /// </summary>
        public static IReadOnlyList<Type> EventBusTypes { get; set; }

        /// <summary>
        /// Initializes all the event buses.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            EventTypes = PredefinedAssemblyUtil.GetTypes(typeof(IEvent));
            EventBusTypes = InitializeAllBuses();
        }

        /// <summary>
        /// Initializes all the event buses.
        /// </summary>
        /// <returns></returns>
        static List<Type> InitializeAllBuses()
        {
            List<Type> eventBusTypes = new List<Type>();

            var typedef = typeof(EventBus<>);
            foreach (var eventType in EventTypes)
            {
                var busType = typedef.MakeGenericType(eventType);
                eventBusTypes.Add(busType);
                Debug.Log($"Initialized EventBus<{eventType.Name}>");
            }

            return eventBusTypes;
        }

        /// <summary>
        /// Clears all the event buses.
        /// </summary>
        public static void ClearAllBuses()
        {
            Debug.Log("Clearing all buses");
            for (int i = 0; i < EventBusTypes.Count; i++)
            {
                var busType = EventBusTypes[i];
                var clearMethod = busType.GetMethod("Clear", BindingFlags.Static | BindingFlags.NonPublic);
                clearMethod.Invoke(null, null);
            }
        }
    }
}