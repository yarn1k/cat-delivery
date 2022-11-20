using System;
using System.Collections;
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
        private Coroutine _lifetimeCoroutine;

        public event Action LifetimeElapsed;
        public event Action<Bullet, CatView> Hit;
        public event Action<Bullet> Disposed;

        public Vector2 ContactPoint { get; private set; }
        public bool HitAnyTarget { get; private set; }

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
            transform.position += transform.right * _bulletForce * Time.deltaTime;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            ContactPoint = collision.ClosestPoint(transform.position);
            
            if (collision.TryGetComponent(out CatView target) && target.Interactable)
            {
                HitAnyTarget = true;
                Hit?.Invoke(this, target);
            }
        }
        private void Enable(bool isEnabled)
        {
            _collider.enabled = isEnabled;
            _collider.attachedRigidbody.simulated = isEnabled;
            _collider.attachedRigidbody.constraints = isEnabled ? RigidbodyConstraints2D.FreezeRotation : RigidbodyConstraints2D.FreezeAll;
        }
        public void Dispose()
        {
            ContactPoint = _collider.bounds.center;
            if (_lifetimeCoroutine != null)
            {
                StopCoroutine(_lifetimeCoroutine);
                _lifetimeCoroutine = null;
            }
            Disposed?.Invoke(this);
            _pool?.Despawn(this);
        }

        void IPoolable<Vector2, Quaternion, float, float, IMemoryPool>.OnDespawned()
        {
            _pool = null;
            HitAnyTarget = false;
            Enable(false);
        }
        void IPoolable<Vector2, Quaternion, float, float, IMemoryPool>.OnSpawned(Vector2 position, Quaternion rotation, float force, float lifetime, IMemoryPool pool)
        {
            _pool = pool;
            transform.position = position;
            transform.rotation = rotation;
            _bulletForce = force;
            Enable(true);
            _lifetimeCoroutine = StartCoroutine(LifetimeCoroutine(lifetime));
        }
        private IEnumerator LifetimeCoroutine(float lifetime)
        {
            yield return new WaitForSeconds(lifetime);
            LifetimeElapsed?.Invoke();
        }

        public class Factory : PlaceholderFactory<Vector2, Quaternion, float, float, Bullet> { }
        public class ExplosionFactory : PlaceholderFactory<GameObject, Vector2, GameObject> 
        {
            private readonly DiContainer _container;
            public ExplosionFactory(DiContainer container)
            {
                _container = container;
            }

            public override GameObject Create(GameObject prefab, Vector2 position)
            {
                return _container.InstantiatePrefab(prefab, position, Quaternion.identity, null);
            }
        }
    }
}