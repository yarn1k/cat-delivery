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
        private LayerMask _layerMask;
        [SerializeField]
        private BoxCollider2D _boxCollider;
        [SerializeField]
        private Rigidbody2D _rigidbody;
        [SerializeField]
        private Transform _firePoint;

        private PlayerSettings _settings;
        private IInputSystem _inputSystem;
        private Bullet.Factory _bulletFactory;
        private Camera _cachedCamera;

        private bool IsMoving => _inputSystem.HorizontalAxis != 0f;
        private int Sign => _gun.flipX ? -1 : 1;
        private float _oldAxis;
        private float _firePointStartX;
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
            _inputSystem.Jump += Jump;
            _rigidbody.gravityScale = _settings.NormalGravityScale;
            _firePointStartX = _firePoint.localPosition.x;
        }
        private void OnDisable()
        {
            _inputSystem.Fire -= Fire;
            _inputSystem.Jump -= Jump;
        }
        private void Update()
        {
            if (IsMoving) _oldAxis = _inputSystem.HorizontalAxis;
            FlipSprite(_oldAxis > 0f);

            _animator.SetBool("IsMoving", IsMoving);

            _mouseWorldPosition = _cachedCamera.ScreenToWorldPoint(_inputSystem.MousePosition);

            _gun.transform.rotation = TrackCursor(_mouseWorldPosition);

            Vector2 direction = Vector2.right * _inputSystem.HorizontalAxis * _settings.MovementSpeed * Time.deltaTime;
            transform.Translate(direction, Space.World);

            CheckGravity();
        }
        private Quaternion TrackCursor(Vector3 worldPosition)
        {
            Vector3 lookDirection = worldPosition - transform.position;
            float degrees = _gun.flipX ? 0f : -180f;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + degrees;
            float clampedAngle = _gun.flipX ? Mathf.Clamp(angle, 0f, ANGLE) : Mathf.Clamp(angle, -ANGLE, 0f);
            return Quaternion.Euler(0f, 0f, clampedAngle);
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

            Vector3 localPos = _firePoint.localPosition;
            localPos.x = _firePointStartX * Sign;
            _firePoint.localPosition = localPos;
        }
        private void Fire()
        {
            if (!CheckFireCooldown()) return;

            _reloadTimer = Time.realtimeSinceStartup;

            Bullet bullet = _bulletFactory.Create();
            bullet.transform.position = _firePoint.position;
            bullet.transform.rotation = _firePoint.rotation;

            Vector3 direction = Sign * -_firePoint.right;
            bullet.Blast(direction, _settings.BulletForce);
        }
        private void Jump()
        {
            if (!IsGrounded()) return;

            _rigidbody.velocity = Vector2.up * _settings.JumpForce;
        }
        private void CheckGravity()
        {
            if (_rigidbody.velocity.y >= 0)
            {
                _rigidbody.gravityScale = _settings.NormalGravityScale;
            }
            else if (_rigidbody.velocity.y < 0)
            {
                _rigidbody.gravityScale = _settings.FallingGravityScale;
            }
        }
        private bool IsGrounded()
        {
            RaycastHit2D raycastHit2d = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f, Vector2.down, .1f, _layerMask);
            return raycastHit2d.collider != null;
        }
    }
}
