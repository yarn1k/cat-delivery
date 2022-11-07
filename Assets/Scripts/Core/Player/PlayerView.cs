using System.Collections;
using System.Linq;
using UnityEngine;
using Cooldown = Core.Weapons.Cooldown;

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
            transform.localScale = new Vector3(_startLocalScaleX * sign, transform.localScale.y, transform.localScale.z);
        }
        public void RotateGun(Quaternion rotation)
        {
            _gun.transform.rotation = rotation;
        }
        public void ReloadGun(Cooldown cooldown)
        {
            if (cooldown.IsOver) return;

            _gun.color = Color.red;
            StartCoroutine(ReloadEffect(cooldown.Duration, Color.red, Color.white));
        }
        IEnumerator ReloadEffect(float cooldown, Color startColor, Color endColor)
        {
            float timeElapsed = 0;
            while (timeElapsed < cooldown)
            {
                _gun.color = Color.Lerp(startColor, endColor, timeElapsed / cooldown);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        public void Jump(float force)
        {
            IsGrounded = false;
            _rigidbody.velocity = Vector2.up * force;
        }
    }
}
