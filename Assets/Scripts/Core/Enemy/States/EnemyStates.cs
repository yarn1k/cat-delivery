using UnityEngine;

namespace Core.Enemy.States
{
    public abstract class EnemyBaseState : IState<EnemyView>
    {
        protected readonly IStateMachine<EnemyView> StateMachine;
        protected EnemyView Context => StateMachine.Context;

        public EnemyBaseState(IStateMachine<EnemyView> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
    }

    public class UpAndDownState : EnemyBaseState
    {
        private readonly float _speed = 3f;
        private float _startTime;

        public UpAndDownState(IStateMachine<EnemyView> stateMachine) : base(stateMachine)
        {

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
            float runningTime = Time.realtimeSinceStartup - _startTime;
            float deltaHeight = Mathf.Sin(runningTime + Time.deltaTime) - Mathf.Sin(runningTime);
            Context.transform.position += Vector3.up * deltaHeight * _speed;
        }
    }
    public class SwapPositionState : EnemyBaseState
    {
        public SwapPositionState(IStateMachine<EnemyView> stateMachine) : base(stateMachine)
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