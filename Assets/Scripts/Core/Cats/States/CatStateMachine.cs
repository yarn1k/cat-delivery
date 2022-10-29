using System.Collections.Generic;
using System.Linq;
using Zenject;
using Core.Models;

namespace Core.Cats.States
{
    public class CatStateMachine : IStateMachine<Cat>, IInitializable, ITickable
    {
        private List<IState<Cat>> _states;
        private Cat _context;
        private CatsSettings _settings;
        private IState<Cat> _currentState;

        public Cat Context => _context;
        public IState<Cat> CurrentState => _currentState;

        [Inject]
        private void Construct(Cat catView, CatsSettings settings)
        {
            _context = catView;
            _settings = settings;
        }
        void IInitializable.Initialize()
        {
            _states = new List<IState<Cat>>()
            {
                new FallingState(this, _settings.CatsFallingSpeed),
                new KidnapState(this, _settings.CatsFallingSpeed),
                new SaveState(this, _settings.CatsFallingSpeed),
                new NeutralState(this, _settings.CatsFallingSpeed)
            };
            SwitchState<FallingState>();
        }
        void ITickable.Tick()
        {
            _currentState?.Update();
        }
        public void SwitchState<State>() where State : IState<Cat>
        {
            _currentState?.Exit();
            _currentState = _states.FirstOrDefault(state => state is State);
            _currentState.Enter();
        }
    }
}
