using System;

namespace Mert.FSM
{
    /// <summary>
    /// This class is used to store the predicate data.
    /// </summary>
    public class FuncPredicate : IPredicate
    {
        readonly Func<bool> func;

        public FuncPredicate(Func<bool> func)
        {
            this.func = func;
        }

        public bool Evaluate() => func.Invoke();
    }
}