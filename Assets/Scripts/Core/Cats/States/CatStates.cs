using UnityEngine;

namespace Core.Cats.States
{
    public abstract class BaseCatState : IState<CatView>
    {
        protected readonly IStateMachine<CatView> StateMachine;
        protected CatView Context => StateMachine.Context;

        public BaseCatState(IStateMachine<CatView> stateMachine)
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

        public FallingState(IStateMachine<CatView> stateMachine, float fallingSpeed) : base(stateMachine)
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
            Context.transform.Translate(_fallingSpeed * Vector3.down * Time.deltaTime, Space.World);
        }
    }
    public class KidnapState : BaseCatState
    {
        private readonly float _kidnapSpeed;

        public KidnapState(IStateMachine<CatView> stateMachine, float kidnapSpeed) : base(stateMachine)
        {
            _kidnapSpeed = kidnapSpeed;
        }

        public override void Enter()
        {

        }
        public override void Exit()
        {

        }
        public override void Update()
        {
            Context.transform.Translate(_kidnapSpeed * Vector3.right * Time.deltaTime, Space.World);
            Context.transform.Rotate(Vector3.forward, 1f, Space.Self);
        }
    }
    public class SaveState : BaseCatState
    {
        private readonly float _saveSpeed;

        public SaveState(IStateMachine<CatView> stateMachine, float saveSpeed) : base(stateMachine)
        {
            _saveSpeed = saveSpeed;
        }

        public override void Enter()
        {

        }
        public override void Exit()
        {
            
        }
        public override void Update()
        {
            Context.transform.Translate(_saveSpeed * Vector3.down * Time.deltaTime, Space.World);
        }
    }
}
