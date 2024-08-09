using System.Collections.Generic;
using UnityEngine;

namespace Mert.EventBus
{
    /// <summary>
    /// This class is used to create an event bus for a specific type of event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class EventBus<T> where T : IEvent
    {
        /// <summary>
        /// HashSet to store all the bindings for the event bus.
        /// </summary>
        static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();

        /// <summary>
        /// Registers a new binding to the event bus.
        /// </summary>
        /// <param name="binding"></param>
        public static void Register(EventBinding<T> binding) => bindings.Add(binding);

        /// <summary>
        /// Unregisters a binding from the event bus.
        /// </summary>
        /// <param name="binding"></param>
        public static void Unregister(EventBinding<T> binding) => bindings.Remove(binding);

        /// <summary>
        /// Raises the event and calls all the bindings.
        /// </summary>
        /// <param name="event"></param>
        public static void Raise(T @event)
        {
            foreach (var binding in bindings)
            {
                binding.OnEvent?.Invoke(@event);
                binding.OnEventNoArgs?.Invoke();
            }
        }

        /// <summary>
        /// Clears all the bindings from the event bus.
        /// </summary>
        static void Clear()
        {
            Debug.Log($"Clearing EventBus<{typeof(T).Name}>");
            bindings.Clear();
        }
    }
}