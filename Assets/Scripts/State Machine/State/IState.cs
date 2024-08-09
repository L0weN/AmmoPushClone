namespace Mert.FSM
{
    /// <summary>
    /// Interface for states
    /// </summary>
    public interface IState
    {
        void OnEnter();
        void Update();
        void FixedUpdate();
        void LateUpdate();
        void OnExit();
    }
}