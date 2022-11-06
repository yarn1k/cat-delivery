using System;
using System.Collections;
using UnityEngine;
using Zenject;
using VolumetricLines;
using Core.Cats;

namespace Core.Weapons
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(VolumetricLineBehavior))]
    public class Laser : MonoBehaviour, IPoolable<Vector2, Quaternion, float, float, IMemoryPool>, IDisposable
    {
        private BoxCollider2D _collider;
        private VolumetricLineBehavior _lineRenderer;
        private LevelBounds _levelBounds;
        private IMemoryPool _pool;
        private bool _prepared;
        private float _width;
        private Color _startColor;

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
            _width = _levelBounds.Size.x;
            _startColor = _lineRenderer.LineColor;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_prepared && collision.TryGetComponent(out CatView target) && target.Interactable)
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

        private IEnumerator PreparationCoroutine(float width, float preparationTime)
        {
            float factor = 0f;
            float startWidth = 0f;
            while (factor < 1f)
            {
                _lineRenderer.m_templateMaterial.color = Color.Lerp(Color.red, _startColor, Time.deltaTime / preparationTime);
                float lerp = Mathf.Lerp(startWidth, width, factor += Time.deltaTime / preparationTime);
                SetWidth(lerp);
                yield return null;
            }
            _prepared = true;
        }

        void IPoolable<Vector2, Quaternion, float, float, IMemoryPool>.OnDespawned()
        {
            _pool = null;
            _collider.enabled = false;
            _prepared = false;
        }
        void IPoolable<Vector2, Quaternion, float, float, IMemoryPool>.OnSpawned(Vector2 position, Quaternion rotation, float preparationTime, float lifetime, IMemoryPool pool)
        {
            _pool = pool;
            transform.position = position;
            transform.rotation = rotation;
            _collider.enabled = true;
            if (preparationTime > 0f) StartCoroutine(PreparationCoroutine(_width, preparationTime));
            else _prepared = true;
            Invoke(nameof(OnLifetimeElapsed), preparationTime + lifetime);
        }

        public class Factory : PlaceholderFactory<Vector2, Quaternion, float, float, Laser> { }
    }
}
