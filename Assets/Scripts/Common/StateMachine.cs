namespace Core
{
    public interface IState<StateMachineType>
    {
        StateMachineType Target { get; }
        void Enter();
        void Exit();
    }

    public interface IStateMachine<StateMachineType>
    {
        IState<StateMachineType> CurrentState { get; }
        void SwitchState<State>(State state) where State : IState<StateMachineType>;
    }
}