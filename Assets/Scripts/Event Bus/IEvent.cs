using System;

namespace Mert.EventBus
{
    /// <summary>
    /// Interface for all the events.
    /// </summary>
    public interface IEvent { }

    /// <summary>
    /// Interface for all the event bindings.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IEventBinding<T>
    {
        public Action<T> OnEvent { get; set; }
        public Action OnEventNoArgs { get; set; }
    }

    /// <summary>
    /// Class to bind an event to a method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventBinding<T> : IEventBinding<T> where T : IEvent
    {
        Action<T> onEvent = _ => { }; // Default action with args
        Action onEventNoArgs = () => { }; // Default empty action

        /// <summary>
        /// Action to be called when the event is raised.
        /// </summary>
        Action<T> IEventBinding<T>.OnEvent
        {
            get => onEvent;
            set => onEvent = value;
        }

        /// <summary>
        /// Action to be called when the event is raised with no arguments.
        /// </summary>
        Action IEventBinding<T>.OnEventNoArgs
        {
            get => onEventNoArgs;
            set => onEventNoArgs = value;
        }

        /// <summary>
        /// Event binding constructor with an action that takes an argument.
        /// </summary>
        /// <param name="onEvent"></param>
        public EventBinding(Action<T> onEvent) => this.onEvent = onEvent;

        /// <summary>
        /// Event binding constructor with an action that takes no arguments.
        /// </summary>
        /// <param name="onEventNoArgs"></param>
        public EventBinding(Action onEventNoArgs) => this.onEventNoArgs = onEventNoArgs;

        public void Add(Action onEvent) => onEventNoArgs += onEvent; // Add an action with no arguments
        public void Remove(Action onEvent) => onEventNoArgs -= onEvent; // Remove an action with no arguments
        public void Add(Action<T> onEvent) => this.onEvent += onEvent; // Add an action with arguments
        public void Remove(Action<T> onEvent) => this.onEvent -= onEvent; // Remove an action with arguments
    }
}
