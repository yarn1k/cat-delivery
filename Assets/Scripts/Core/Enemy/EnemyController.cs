using UnityEngine;
using Zenject;
using Core.Enemy.States;

namespace Core.Enemy
{
    public class EnemyController : IInitializable, ITickable
    {
        private readonly EnemyModel _model;
        private readonly EnemyView _view;
        private readonly SignalBus _signalBus;
        private IStateMachine<EnemyController> _stateMachine;

        public EnemyController(SignalBus signalBus, EnemyModel enemyModel, EnemyView enemyView)
        {
            _signalBus = signalBus;
            _model = enemyModel;
            _view = enemyView;
        }

        void IInitializable.Initialize()
        {
            _stateMachine = new EnemyStateMachine(this, _model);
            _stateMachine.SwitchState<UpAndDownState>();
        }
        void ITickable.Tick()
        {
            _stateMachine.CurrentState.Update();
        }

        public void Enable()
        {
            _view.gameObject.SetActive(true);
        }
        public void Disable()
        {
            _view.gameObject.SetActive(false);
        }
        public void Translate(Vector2 direction)
        {
            _view.transform.Translate(direction, Space.World);
        }
        public void SetPosition(Vector2 position)
        {
            _view.transform.position = position;
        }
    }
}
