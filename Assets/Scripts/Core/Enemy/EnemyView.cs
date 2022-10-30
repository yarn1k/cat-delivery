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

        [SerializeField]
        private Laser _laser;

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
        private IEnumerator ShootLaser(float duration)
        {
            _laser.gameObject.SetActive(true);
            yield return new WaitForSeconds(duration);
            _laser.gameObject.SetActive(false);
        }

        private void OnEnemyWantsAttackSignal(EnemyWantsAttackSignal signal)
        {
            StartCoroutine(ShootLaser(1.5f));
        }
    }
}
