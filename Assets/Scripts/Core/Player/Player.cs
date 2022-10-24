using Core.Infrastructure.Signals.Player;
using Core.Models;
using UnityEngine;
using Zenject;

namespace Core.Player
{
    public class Player : MonoBehaviour
    {
        private SignalBus _signalBus;
        private GameSettings _settings;

        [SerializeField]
        private Camera _mainCamera;
        private Vector3 _mousePosition;
        private bool _isCanFire;
        private float _reloadTimer;
        [SerializeField]
        private GameObject _firePoint;

        [Inject]
        public void Construct(SignalBus signalBus, GameSettings settings)
        {
            _signalBus = signalBus;
            _settings = settings;
        }

        private void FixedUpdate()
        {
            _mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

            Vector2 lookDirection = _mousePosition - transform.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            if (!_isCanFire)
            {
                _reloadTimer += Time.fixedDeltaTime;
                if (_reloadTimer > _settings.ReloadTime)
                {
                    _isCanFire = true;
                    _reloadTimer = 0;
                }
            }

            if (Input.GetMouseButton(0) && _isCanFire)
            {
                _isCanFire = false;
                _signalBus.Fire(new PlayerFiredSignal { FirePoint = _firePoint.transform });
            }
        }
    }
}
