using System.Collections.Generic;
using System.Linq;

namespace Core.Enemy.States
{
    public class EnemyStateMachine : IStateMachine<EnemyController>
    {
        private List<IState<EnemyController>> _states;
        public EnemyController Context { get; private set; }
        public IState<EnemyController> CurrentState { get; private set; }

        public EnemyStateMachine(EnemyController controller, EnemyModel model)
        {
            Context = controller;
            _states = new List<IState<EnemyController>>()
            {
                new UpAndDownState(this, model.MovementSpeed, model.AttackCooldownInterval, model.PrimaryWeapon),
                new SwapPositionState(this),
            };
        }

        public void SwitchState<State>() where State : IState<EnemyController>
        {
            CurrentState?.Exit();
            CurrentState = _states.FirstOrDefault(state => state is State);
            CurrentState.Enter();
        }
    }
}
