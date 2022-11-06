using System.Collections.Generic;
using System.Linq;
using Zenject;
using Core.Models;

namespace Core.Cats.States
{
    public class CatStateMachine : IStateMachine<CatView>, IInitializable, ITickable
    {
        private List<IState<CatView>> _states;
        public CatView Context { get; private set; }
        public IState<CatView> CurrentState { get; private set; }

        [Inject]
        private void Construct(CatView catView, CatsSettings settings)
        {
            Context = catView;
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
            CurrentState.Update();
        }
        public void SwitchState<State>() where State : IState<CatView>
        {
            CurrentState?.Exit();
            CurrentState = _states.FirstOrDefault(state => state is State);
            CurrentState.Enter();
        }
    }
}
