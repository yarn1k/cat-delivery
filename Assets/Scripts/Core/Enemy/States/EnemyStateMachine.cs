using System.Collections.Generic;
using System.Linq;
using Zenject;
using Core.Models;

namespace Core.Enemy.States
{
    public class EnemyStateMachine : IStateMachine<EnemyView>, IInitializable, ITickable
    {
        private List<IState<EnemyView>> _states;
        private EnemyView _context;
        private IState<EnemyView> _currentState;

        public EnemyView Context => _context;
        public IState<EnemyView> CurrentState => _currentState;

        [Inject]
        private void Construct(EnemyView enemyView, EnemySettings settings)
        {
            _context = enemyView;
            _states = new List<IState<EnemyView>>()
            {
                new UpAndDownState(this, settings),
                new SwapPositionState(this),
            };
        }
        void IInitializable.Initialize()
        {         
            SwitchState<UpAndDownState>();
        }
        void ITickable.Tick()
        {
            _currentState?.Update();
        }
        public void SwitchState<State>() where State : IState<EnemyView>
        {
            _currentState?.Exit();
            _currentState = _states.FirstOrDefault(state => state is State);
            _currentState.Enter();
        }
    }
}
