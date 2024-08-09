namespace Mert.FSM
{
    /// <summary>
    /// This class is used to store the transition data.
    /// </summary>
    public class Transition : ITransition
    {
        public IState To { get; }

        public IPredicate Condition { get; }

        public Transition(IState to, IPredicate condition)
        {
            To = to;
            Condition = condition;
        }
    }
}