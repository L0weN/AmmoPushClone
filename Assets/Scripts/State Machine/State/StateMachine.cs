using System;
using System.Collections.Generic;

namespace Mert.FSM
{
    /// <summary>
    /// This class is responsible for managing the states and transitions between them.
    /// </summary>
    public class StateMachine
    {
        StateNode current; // Current state
        Dictionary<Type, StateNode> nodes = new(); // All states
        HashSet<ITransition> anyTransitions = new(); // Transitions that can be made from any state

        /// <summary>
        /// Checks the transitions and updates the current state.
        /// </summary>
        public void Update()
        {
            var transition = GetTransition();
            if (transition != null)
            {
                ChangeState(transition.To);
            }
            current.State?.Update();
        }

        public void FixedUpdate()
        {
            current.State?.FixedUpdate();
        }

        public void LateUpdate()
        {
            current.State?.LateUpdate();
        }
        
        /// <summary>
        /// This method sets the initial state of the state machine.
        /// </summary>
        /// <param name="state"></param>
        public void SetState(IState state)
        {
            current = nodes[state.GetType()];
            current.State?.OnEnter();
        }

        /// <summary>
        /// This method changes the current state of the state machine.
        /// </summary>
        /// <param name="state"></param>
        void ChangeState(IState state)
        {
            if (state == current.State) return;

            var previousState = current.State;
            var nextState = nodes[state.GetType()].State;

            previousState?.OnExit();
            nextState?.OnEnter();

            current = nodes[state.GetType()];
        }

        /// <summary>
        /// This method checks the transitions and returns the transition that can be made.
        /// </summary>
        /// <returns></returns>
        ITransition GetTransition()
        {
            foreach (var transition in anyTransitions)
                if (transition.Condition.Evaluate())
                    return transition;

            foreach (var transition in current.Transitions)
                if (transition.Condition.Evaluate())
                    return transition;
            return null;
        }

        /// <summary>
        /// This method adds a transition between two states.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="condition"></param>
        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        /// <summary>
        /// This method adds a transition that can be made from any state.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="condition"></param>
        public void AddAnyTransition(IState to, IPredicate condition)
        {
            anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }

        /// <summary>
        /// This method returns the state node of the given state. If the state node does not exist, it creates a new one.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        StateNode GetOrAddNode(IState state)
        {
            var node = nodes.GetValueOrDefault(state.GetType());

            if (node == null)
            {
                node = new StateNode(state);
                nodes.Add(state.GetType(), node);
            }

            return node;
        }

        /// <summary>
        /// This class represents a transition between two states.
        /// </summary>
        class StateNode
        {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState to, IPredicate condition)
            {
                Transitions.Add(new Transition(to, condition));
            }
        }
    }
}