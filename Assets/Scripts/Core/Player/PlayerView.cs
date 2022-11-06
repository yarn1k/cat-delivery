using System.Linq;
using UnityEngine;

namespace Core.Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] 
        private Rigidbody2D _rigidbody;
        [SerializeField] 
        private SpriteRenderer _gun;
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public Transform FirePoint { get; private set; }

        private float _startLocalScaleX;
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
        public void RotateGun(Quaternion rotation)
        {
            _gun.transform.rotation = rotation;
        }
        public void Jump(float force)
        {
            IsGrounded = false;
            _rigidbody.velocity = Vector2.up * force;
        }
    }
}
