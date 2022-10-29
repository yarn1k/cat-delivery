using UnityEngine;
using Zenject;
using Core.Models;

namespace Core.Player
{
    public class Player : MonoBehaviour
    {
        private PlayerSettings _settings;
        private Bullet.Factory _bulletFactory;
        private Camera _cachedCamera;

        private Vector3 _mousePosition;
        private bool _isCanFire = true;
        private float _reloadTimer;

        [SerializeField]
        private Transform _firePoint;

        [Inject]
        public void Construct(PlayerSettings settings, Bullet.Factory bulletFactory)
        {
            _settings = settings;
            _bulletFactory = bulletFactory;
        }

        private void Start()
        {
            _cachedCamera = Camera.main;
        }
        private void Update()
        {
            _mousePosition = _cachedCamera.ScreenToWorldPoint(Input.mousePosition);

            Vector2 lookDirection = _mousePosition - transform.position;
            float rotationZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            float angle = Mathf.Clamp(rotationZ, -55, 55);
            transform.rotation = Quaternion.Euler(0, 0, angle);

            if (!_isCanFire)
            {
                _reloadTimer += Time.deltaTime;
                if (_reloadTimer > _settings.ReloadTime)
                {
                    _isCanFire = true;
                    _reloadTimer = 0;
                }
            }

            if (_isCanFire && Input.GetMouseButton(0))
            {
                _isCanFire = false;
                Fire();
            }

            float moveHorizontal = Input.GetAxis("Horizontal");

            transform.Translate(moveHorizontal * _settings.MovementSpeed * Time.deltaTime, 0, 0, Space.World);
        }
        private void Fire()
        {
            Bullet bullet = _bulletFactory.Create();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
        }
    }
}
