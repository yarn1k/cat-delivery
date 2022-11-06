using System;
using UnityEngine;
using Zenject;
using Core.Cats;
using DG.Tweening;

namespace Core.Weapons
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Laser : MonoBehaviour, IPoolable<Vector2, Quaternion, float, float, IMemoryPool>, IDisposable
    {
        private BoxCollider2D _collider;
        private SpriteRenderer _renderer;
        private LevelBounds _levelBounds;
        private IMemoryPool _pool;
        private bool _prepared;
        private Vector3 _startPosition;

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
            _renderer = GetComponent<SpriteRenderer>();
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
            ShowLaser(false, 0.5f)
                .SetLink(gameObject)
                .SetEase(Ease.Linear)
                .OnComplete(() => LifetimeElapsed?.Invoke(this));
        }

        private void SetWidth(float width)
        {
            _renderer.size = new Vector2(width, _renderer.size.y);
            _collider.size = new Vector2(width, _collider.size.y);
            transform.position = _startPosition + transform.right * width / 2f;
        }

        public void Dispose()
        {
            _pool?.Despawn(this);
        }

        private Tweener ShowLaser(bool isShow, float time)
        {
            float alpha = isShow ? 1f : 0f;
            _renderer.color = _renderer.color.WithAlpha(1f - alpha);
            return _renderer.DOColor(_renderer.color.WithAlpha(alpha), time);
        }
        private void PrepareLaser(float preparationTime)
        {
            float width = _levelBounds.Size.x;
            _startPosition = transform.position;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(ShowLaser(true, preparationTime));
            sequence.Join(DOTween.To(() => 0f, x => SetWidth(x), width, preparationTime));
            sequence.SetLink(gameObject);
            sequence.SetEase(Ease.Linear);
            sequence.OnComplete(() => _prepared = true);
            sequence.Play();
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

            if (preparationTime > 0f) PrepareLaser(preparationTime);
            else _prepared = true;

            Invoke(nameof(OnLifetimeElapsed), preparationTime + lifetime);
        }

        public class Factory : PlaceholderFactory<Vector2, Quaternion, float, float, Laser> { }
    }
}
