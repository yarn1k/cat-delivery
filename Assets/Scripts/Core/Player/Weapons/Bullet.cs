using System;
using UnityEngine;
using Zenject;
using Core.Cats;
using Core.Models;

namespace Core.Weapons
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Bullet : MonoBehaviour, IPoolable<Vector2, Quaternion, float, float, IMemoryPool>, IDisposable
    {
        private Collider2D _collider;
        private IMemoryPool _pool;
        private float _bulletForce;

        public event Action<Bullet> LifetimeElapsed;
        public event Action<Bullet, CatView> Hit;

        private AudioSource _audioSource;
        private AudioPlayerSettings _audioPlayerSettings;

        [Inject]
        public void Construct(AudioSource audioSource, AudioPlayerSettings audioPlayerSettings)
        {
            _audioSource = audioSource;
            _audioPlayerSettings = audioPlayerSettings;
        }

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }
        private void Update()
        {
            transform.position += transform.up * _bulletForce * Time.deltaTime;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out CatView target) && target.Interactable)
            {
                Audio playerOnHit = _audioPlayerSettings.PlayerOnHit;
                _audioSource.PlayOneShot(playerOnHit.Clip, playerOnHit.Volume);
                Hit?.Invoke(this, target);
            }
        }
        private void OnLifetimeElapsed()
        {
            LifetimeElapsed?.Invoke(this);
        }
        public void Dispose()
        {
            _pool?.Despawn(this);
        }

        void IPoolable<Vector2, Quaternion, float, float, IMemoryPool>.OnDespawned()
        {
            _pool = null;
            _collider.enabled = false;
            _collider.attachedRigidbody.simulated = false;
            _collider.attachedRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        void IPoolable<Vector2, Quaternion, float, float, IMemoryPool>.OnSpawned(Vector2 position, Quaternion rotation, float force, float lifetime, IMemoryPool pool)
        {
            _pool = pool;
            transform.position = position;
            transform.rotation = rotation;
            _bulletForce = force;
            _collider.enabled = true;
            _collider.attachedRigidbody.simulated = true;
            _collider.attachedRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            Invoke(nameof(OnLifetimeElapsed), lifetime);
        }

        public class Factory : PlaceholderFactory<Vector2, Quaternion, float, float, Bullet> { }
    }
}