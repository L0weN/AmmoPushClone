namespace Mert.FSM
{
    /// <summary>
    /// This interface is used to define the transition.
    /// </summary>
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
    }
}