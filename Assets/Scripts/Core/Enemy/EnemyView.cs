using System;
using System.Collections;
using UnityEngine;
using Zenject;
using Core.Infrastructure.Signals.Game;

namespace Core.Enemy
{
    public class EnemyView : MonoBehaviour, IDisposable
    {
        private SignalBus _signalBus;

        private bool _isMoving = true;
        [SerializeField]
        private Transform _firePoint;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            _signalBus.Subscribe<EnemyWantsAttackSignal>(OnEnemyWantsAttackSignal);
        }

        public void Dispose()
        {
            _signalBus.Subscribe<EnemyWantsAttackSignal>(OnEnemyWantsAttackSignal);
        }

        private void OnEnemyWantsAttackSignal(EnemyWantsAttackSignal signal)
        {
            StartCoroutine(StopMoving());
        }

        private IEnumerator StopMoving()
        {
            _isMoving = false;
            yield return new WaitForSeconds(1.5f);
            _signalBus.Fire(new GameSpawnedLaserSignal { });
            yield return new WaitForSeconds(2);
            _isMoving = true;
        }
    }
}
