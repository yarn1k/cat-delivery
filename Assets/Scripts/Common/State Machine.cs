using UnityEngine;

namespace Core
{
    public interface IState<T>
    {
        void Enter();
        void Exit();
        void Update();
    }

    public interface IStateMachine<T>
    {
        T Context { get; }
        IState<T> CurrentState { get; }
        void SwitchState<State>() where State : IState<T>;
    }
}