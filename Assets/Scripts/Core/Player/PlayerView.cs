using UnityEngine;
using Zenject;
using Core.Models;
using Core.Input;

namespace Core.Player
{
    public class PlayerView : MonoBehaviour
    {
        private PlayerSettings _settings;
        private IInputSystem _inputSystem;
        private Bullet.Factory _bulletFactory;
        private Camera _cachedCamera;

        private Vector3 _mouseWorldPosition;
        private float _reloadTimer;
        private float _horizontalAxis;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private Transform _firePoint;

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
            _horizontalAxis = _inputSystem.HorizontalAxis;

            SetFlipX();
            CheckRunningAnimation();

            _mouseWorldPosition = _cachedCamera.ScreenToWorldPoint(_inputSystem.MousePosition);

            transform.rotation = LookAtRotation(_mouseWorldPosition);

            transform.Translate(Vector2.right * _horizontalAxis * _settings.MovementSpeed * Time.deltaTime, Space.World);
        }
        private void SetFlipX()
        {
            if (_horizontalAxis > 0)
                _spriteRenderer.flipX = true;
            else if (_horizontalAxis < 0)
                _spriteRenderer.flipX = false;
        }
        private void CheckRunningAnimation()
        {
            var movement = _horizontalAxis != 0;
            _animator.SetBool("IsMoving", movement);
        }
        private Quaternion LookAtRotation(Vector3 worldPosition)
        {
            Vector3 lookDirection = worldPosition - transform.position;
            float rotationZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            float angle = Mathf.Clamp(rotationZ, -55f, 55f);
            return Quaternion.Euler(0f, 0f, angle);
        }
        private bool CheckFireCooldown()
        {
            return Time.realtimeSinceStartup - _reloadTimer >= _settings.ReloadTime;
        }
        private void Fire()
        {
            if (!CheckFireCooldown()) return;

            _reloadTimer = Time.realtimeSinceStartup;

            Bullet bullet = _bulletFactory.Create();
            bullet.transform.position = _firePoint.position;
            bullet.transform.rotation = transform.rotation;

            Vector2 direction = (_mouseWorldPosition - transform.position).normalized;
            bullet.Blast(direction, _settings.BulletForce);
        }
    }
}
