using UnityEngine;

namespace Core.Cats.States
{
    public abstract class BaseCatState : IState<Cat>
    {
        protected readonly IStateMachine<Cat> StateMachine;
        protected Cat Context => StateMachine.Context;

        public BaseCatState(IStateMachine<Cat> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
    }

    public class FallingState : BaseCatState
    {
        private readonly float _fallingSpeed;

        public FallingState(IStateMachine<Cat> stateMachine, float fallingSpeed) : base(stateMachine)
        {
            _fallingSpeed = fallingSpeed;
        }

        public override void Enter()
        {
 
        }
        public override void Exit()
        {

        }
        public override void Update()
        {
            Context.transform.Translate(_fallingSpeed * Vector3.down * Time.fixedDeltaTime, Space.World);
        }
    }
    public class KidnapState : BaseCatState
    {
        private readonly float _fallingSpeed;

        public KidnapState(IStateMachine<Cat> stateMachine, float fallingSpeed) : base(stateMachine)
        {
            _fallingSpeed = fallingSpeed;
        }

        public override void Enter()
        {

        }
        public override void Exit()
        {

        }
        public override void Update()
        {
            Context.transform.Translate(_fallingSpeed * 3f * Vector3.right * Time.fixedDeltaTime, Space.World);
        }
    }
    public class SaveState : BaseCatState
    {
        private readonly float _fallingSpeed;

        public SaveState(IStateMachine<Cat> stateMachine, float fallingSpeed) : base(stateMachine)
        {
            _fallingSpeed = fallingSpeed;
        }

        public override void Enter()
        {

        }
        public override void Exit()
        {

        }
        public override void Update()
        {
            Context.transform.Translate(_fallingSpeed * 2f * Vector3.down * Time.fixedDeltaTime, Space.World);
        }
    }
    public class NeutralState : BaseCatState
    {
        private readonly float _fallingSpeed;

        public NeutralState(IStateMachine<Cat> stateMachine, float fallingSpeed) : base(stateMachine)
        {
            _fallingSpeed = fallingSpeed;
        }

        public override void Enter()
        {

        }
        public override void Exit()
        {

        }
        public override void Update()
        {
            Context.transform.Translate(_fallingSpeed * 2f * Vector3.down * Time.fixedDeltaTime, Space.World);
        }
    }
}
