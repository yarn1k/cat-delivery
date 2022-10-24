using Core.Infrastructure.Signals.Game;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Core.Enemy
{
    public class Enemy : MonoBehaviour, IDisposable
    {
        private SignalBus _signalBus;

        private float _speed = 3f;
        //private float _waitTime = 0.5f;
        [SerializeField]
        private Transform[] _moveSpots;
        private Vector3 _toPosition;
        private bool _isMoving = true;
        private bool _isMoveUp;
        [SerializeField]
        private Transform _firePoint;

        [Inject]
        private void Construct(SignalBus signalBus) => _signalBus = signalBus;

        void Start()
        {
            _signalBus.Subscribe<EnemyWantsAttackSignal>(OnEnemyWantsAttackSignal);
            StartCoroutine(Move());
        }

        IEnumerator Move()
        {
            if (_moveSpots.Length != 2) yield break;

            while (_isMoving)
            {
                if (_isMoveUp)
                {
                    _isMoveUp = false;
                    _toPosition = _moveSpots[0].position;
                }
                else
                {
                    _isMoveUp = true;
                    _toPosition = _moveSpots[1].position;
                }

                while (Vector3.Distance(transform.position, _toPosition) > 0.2f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _toPosition, _speed * Time.deltaTime);
                    yield return null;
                }
                //yield return new WaitForSeconds(_waitTime);
            }
        }

        public void Dispose()
        {
            _signalBus.Subscribe<EnemyWantsAttackSignal>(OnEnemyWantsAttackSignal);
        }

        private void OnEnemyWantsAttackSignal(EnemyWantsAttackSignal signal)
        {
            StartCoroutine(StopMoving());
        }

        IEnumerator StopMoving()
        {
            _isMoving = false;
            yield return new WaitForSeconds(2);
            _signalBus.Fire(new GameSpawnedLaserSignal { });
            yield return new WaitForSeconds(2);
            _isMoving = true;
            StartCoroutine(Move());
        }
    }
}
