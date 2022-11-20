using System;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Zenject;
using DG.Tweening;

namespace Core.Cats
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class CatView : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        [SerializeField]
        private SpriteRenderer _shield;
        [SerializeField]
        private SpriteLibrary _library;

        private SpriteRenderer _renderer;
        private IMemoryPool _pool;
        private bool _interactable;
        private Vector3 _direction = Vector3.zero;
        private float _velocity;
        private bool _kidnapped;
        public ref bool Interactable => ref _interactable;

        public event Action<CatView> Saved;
        public event Action<CatView, Vector2> Kidnapped;
        public event Action<CatView> Disposed;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }
        private void Update()
        {
            if (_direction != Vector3.zero)
            {
                transform.Translate(_direction * _velocity * Time.deltaTime, Space.World);
            }

            if (_kidnapped) transform.Rotate(Vector3.forward, 1f, Space.Self);
        }

        private void ResetState()
        {          
            _kidnapped = false;
            transform.rotation = Quaternion.identity;
        }
        private void Show()
        {
            _renderer.color = _renderer.color.WithAlpha(1f);
            _shield.color = _shield.color.WithAlpha(0.3f);
        }
        public void SetInteractable(bool isInteractable)
        {
            _interactable = isInteractable;
        }
        public void SetDirection(Vector2 direction, float velocity)
        {
            _direction = direction;
            _velocity = velocity;
        }
        public void Kidnap(Vector2 direction)
        {
            _kidnapped = true;         
            Kidnapped?.Invoke(this, direction);
        }
        public void Save()
        {
            _shield.gameObject.SetActive(true);
            Saved?.Invoke(this);
        }
        public void SetSkin(SpriteLibraryAsset asset)
        {
            _library.spriteLibraryAsset = asset;
        }
        public void Dispose()
        {        
            Disposed?.Invoke(this);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_renderer.DOColor(_renderer.color.WithAlpha(0f), 1f));
            sequence.Join(_shield.DOColor(_shield.color.WithAlpha(0f), 1f));
            sequence.SetLink(gameObject);
            sequence.SetEase(Ease.Linear);
            sequence.OnComplete(() => _pool?.Despawn(this));
            sequence.Play();
        }   

        void IPoolable<IMemoryPool>.OnDespawned()
        {
            _pool = null;
            ResetState();
        }
        void IPoolable<IMemoryPool>.OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            _shield.gameObject.SetActive(false);
            Show();           
        }

        public class Factory : PlaceholderFactory<CatView> { }
    }
}