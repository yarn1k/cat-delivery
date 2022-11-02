using UnityEngine;
using Zenject;
using Core.Models;
using Core.Input;

namespace Core.Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _gun;
        [SerializeField]
        private SpriteRenderer _head;
        [SerializeField]
        private SpriteRenderer _body;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private Transform _firePoint;

        private PlayerSettings _settings;
        private IInputSystem _inputSystem;
        private Bullet.Factory _bulletFactory;
        private Camera _cachedCamera;

        private Vector3 _mouseWorldPosition;
        private float _reloadTimer;

        private const float ANGLE = 55f;

        [Inject]
        public void Construct(PlayerSettings settings, IInputSystem inputSystem, Bullet.Factory bulletFactory)
        {
            _settings = settings;
            _inputSystem = inputSystem;
            _bulletFactory = bulletFactory;
        }

        private void Start()
        {
            _cachedCamera = Camera.main;
            _inputSystem.Fire += Fire;
        }
        private void OnDisable()
        {
            _inputSystem.Fire -= Fire;
        }
        private void Update()
        {
            FlipSprite(_inputSystem.HorizontalAxis > 0);
            _animator.SetBool("IsMoving", _inputSystem.HorizontalAxis != 0);

            _mouseWorldPosition = _cachedCamera.ScreenToWorldPoint(_inputSystem.MousePosition);

            _gun.transform.rotation = TrackCursor(_mouseWorldPosition);

            Vector2 direction = Vector2.right * _inputSystem.HorizontalAxis * _settings.MovementSpeed * Time.deltaTime;
            transform.Translate(direction, Space.World);
        }
        private Quaternion TrackCursor(Vector3 worldPosition)
        {
            Vector3 lookDirection = worldPosition - transform.position;
            float radians = _gun.flipX ? 0f : 180f;
            float rotationZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - radians;
            float angle = Mathf.Clamp(rotationZ, -ANGLE, ANGLE);
            return Quaternion.Euler(0f, 0f, angle);
        }
        private bool CheckFireCooldown()
        {
            return Time.realtimeSinceStartup - _reloadTimer >= _settings.ReloadTime;
        }
        private void FlipSprite(bool flipX)
        {
            _gun.flipX = flipX;
            _head.flipX = flipX;
            _body.flipX = flipX;
        }
        private void Fire()
        {
            if (!CheckFireCooldown()) return;

            _reloadTimer = Time.realtimeSinceStartup;
            Vector2 direction = _mouseWorldPosition - transform.position;

            Bullet bullet = _bulletFactory.Create();
            bullet.transform.position = _firePoint.position;
            bullet.transform.rotation = _gun.transform.rotation;

            bullet.Blast(direction.normalized, _settings.BulletForce);
        }
    }
}
