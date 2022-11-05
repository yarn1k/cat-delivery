using UnityEngine;
using Core.Weapons;

namespace Core.Enemy.States
{
    public abstract class EnemyBaseState : IState<EnemyController>
    {
        protected readonly IStateMachine<EnemyController> StateMachine;
        protected EnemyController Context => StateMachine.Context;

        public EnemyBaseState(IStateMachine<EnemyController> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
    }

    public class UpAndDownState : EnemyBaseState
    {
        private readonly float _speed;
        private readonly Vector2 _attackCooldownInterval;
        private readonly IWeapon _weapon;
        private float _startTime;
        private float _reloadTimer;
        private float _reloadTime;

        public UpAndDownState(IStateMachine<EnemyController> stateMachine, float speed, Vector2 attackCooldownInterval, IWeapon weapon) : base(stateMachine)
        {
            _speed = speed;
            _attackCooldownInterval = attackCooldownInterval;
            _weapon = weapon;
        }

        private bool CheckFireCooldown()
        {
            return Time.realtimeSinceStartup - _reloadTimer >= _reloadTime;
        }

        public override void Enter()
        {
            _startTime = Time.realtimeSinceStartup;
        }
        public override void Exit()
        {
  
        }
        public override void Update()
        {
            if (CheckFireCooldown())
            {
                _weapon.Shoot();
                _reloadTimer = Time.realtimeSinceStartup;
                _reloadTime = Random.Range(_attackCooldownInterval.x, _attackCooldownInterval.y);
            }

            float runningTime = Time.realtimeSinceStartup - _startTime;
            float deltaHeight = Mathf.Sin(runningTime + Time.deltaTime) - Mathf.Sin(runningTime);
            Context.Translate(Vector3.up * deltaHeight * _speed);
        }
    }
    public class SwapPositionState : EnemyBaseState
    {
        public SwapPositionState(IStateMachine<EnemyController> stateMachine) : base(stateMachine)
        {

        }

        public override void Enter()
        {
    
        }
        public override void Exit()
        {

        }
        public override void Update()
        {
         
        }
    }
}