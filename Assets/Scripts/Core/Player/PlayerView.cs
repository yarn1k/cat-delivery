using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace Core.Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] 
        private Rigidbody2D _rigidbody;
        [SerializeField] 
        private SpriteRenderer _gun;
        [SerializeField]
        private Animator _animator;
        [field: SerializeField] public Transform FirePoint { get; private set; }

        private float _startLocalScaleX;
        private const string MOVING_KEY = "IsMoving";
        public bool IsGrounded { get; private set; }
        public bool FlipX => transform.localScale.x < 0f;


        private void Awake()
        {
            _startLocalScaleX = transform.localScale.x;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.contacts.Any(x => x.normal == Vector2.up))
            {
                IsGrounded = true;
            }
        }

        public void FlipSprite(bool flipX)
        {
            int sign = flipX ? -1 : 1;
            float firePointRotZ = flipX ? 0f : 180f;
            transform.localScale = new Vector3(_startLocalScaleX * sign, transform.localScale.y, transform.localScale.z);
            FirePoint.localRotation = Quaternion.Euler(0f, 0f, firePointRotZ);
        } 
        public void SetDirection(float horizontalAxis)
        {
            bool isMoving = horizontalAxis != 0f;

            _animator.SetBool(MOVING_KEY, isMoving);

            if (isMoving)
            {
                FlipSprite(horizontalAxis > 0f);
            }
        }
        public void RotateGun(Quaternion rotation)
        {
            _gun.transform.rotation = rotation;
        }
        public void ReloadGun(float cooldownTime)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_gun.DOColor(Color.red, cooldownTime / 2f));
            sequence.Append(_gun.DOColor(Color.white, cooldownTime / 2f));
            sequence.SetEase(Ease.Linear);
            sequence.SetLink(gameObject);
            sequence.Play();
        }
        public void Jump(float force)
        {
            IsGrounded = false;
            _rigidbody.velocity = Vector2.up * force;
        }
    }
}
