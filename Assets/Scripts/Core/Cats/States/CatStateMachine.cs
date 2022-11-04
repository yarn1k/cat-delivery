using System.Collections.Generic;
using System.Linq;
using Zenject;
using Core.Models;

namespace Core.Cats.States
{
    public class CatStateMachine : IStateMachine<CatView>, IInitializable, ITickable
    {
        private List<IState<CatView>> _states;
        private CatView _context;
        private IState<CatView> _currentState;

        public CatView Context => _context;
        public IState<CatView> CurrentState => _currentState;

        [Inject]
        private void Construct(CatView catView, CatsSettings settings)
        {
            _context = catView;
            _states = new List<IState<CatView>>()
            {
                new FallingState(this, settings.CatsFallingSpeed),
                new KidnapState(this, settings.CatsKidnapSpeed),
                new SaveState(this, settings.CatsSaveSpeed)
            };
        }

        void IInitializable.Initialize()
        {
            SwitchState<FallingState>();
        }
        void ITickable.Tick()
        {
            _currentState?.Update();
        }
        public void SwitchState<State>() where State : IState<CatView>
        {
            _currentState?.Exit();
            _currentState = _states.FirstOrDefault(state => state is State);
            _currentState.Enter();
        }
    }
}
