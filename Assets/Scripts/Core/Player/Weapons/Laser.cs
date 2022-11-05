using System;
using UnityEngine;
using Zenject;
using VolumetricLines;
using Core.Cats;

namespace Core.Weapons
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(VolumetricLineBehavior))]
    public class Laser : MonoBehaviour, IPoolable<Vector2, Quaternion, float, IMemoryPool>, IDisposable
    {
        private LevelBounds _levelBounds;
        private BoxCollider2D _collider;
        private VolumetricLineBehavior _lineRenderer;
        private IMemoryPool _pool;

        public event Action<Laser> LifetimeElapsed;
        public event Action<CatView> Hit;

        [Inject]
        private void Construct(LevelBounds levelBounds)
        {
            _levelBounds = levelBounds;
        }

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _lineRenderer = GetComponent<VolumetricLineBehavior>();
        }
        private void Start()
        {
            SetWidth(_levelBounds.Size.x * 2f);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out CatView target) && target.Interactable)
            {
                Hit?.Invoke(target);
            }
        }
        private void OnLifetimeElapsed()
        {
            LifetimeElapsed?.Invoke(this);
        }

        private void SetWidth(float width)
        {
            _lineRenderer.EndPos = Vector3.up * width;
            _collider.offset = new Vector2(_collider.offset.x, width / 2f);
            _collider.size = new Vector2(_collider.size.x, width);
        }
        public void Dispose()
        {
            _pool?.Despawn(this);
        }

        void IPoolable<Vector2, Quaternion, float, IMemoryPool>.OnDespawned()
        {
            _pool = null;
            _collider.enabled = false;
        }
        void IPoolable<Vector2, Quaternion, float, IMemoryPool>.OnSpawned(Vector2 position, Quaternion rotation, float lifetime, IMemoryPool pool)
        {
            _pool = pool;
            transform.position = position;
            transform.rotation = rotation;
            _collider.enabled = true;
            Invoke(nameof(OnLifetimeElapsed), lifetime);
        }

        public class Factory : PlaceholderFactory<Vector2, Quaternion, float, Laser> { }
    }
}
