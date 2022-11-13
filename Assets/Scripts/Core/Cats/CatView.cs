using System;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Zenject;
using Core.Models;
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
        private CatsSettings _settings;
        private bool _interactable;
        private Vector3 _direction = Vector3.zero;
        private float _velocity;
        private bool _kidnapped;
        public bool Interactable => _interactable;

        public event Action<CatView> Disposed;

        [Inject]
        private void Construct(CatsSettings settings)
        {
            _settings = settings;
        }

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

        private void InitState()
        {
            _interactable = true;
            _direction = Vector2.down;
            _velocity = _settings.CatsFallingSpeed;
            _shield.gameObject.SetActive(false);
        }
        private void ResetState()
        {
            _interactable = false;
            _kidnapped = false;
            _direction = Vector2.zero;
            _velocity = 0f;
            transform.rotation = Quaternion.identity;
        }
        private void SetInteractable(bool isInteractable)
        {
            _interactable = isInteractable;
        }
        private void SetDirection(Vector2 direction, float velocity)
        {
            _direction = direction;
            _velocity = velocity;
        }
        public void Kidnap(Vector2 direction)
        {
            _kidnapped = true;
            SetInteractable(false);
            SetDirection(direction, _settings.CatsKidnapSpeed);
            Hide();
        }
        public void Save()
        {
            _shield.gameObject.SetActive(true);
            SetInteractable(false);
            SetDirection(Vector2.down, _settings.CatsSaveSpeed);
        }
        public void Show()
        {
            _renderer.color = _renderer.color.WithAlpha(1f);
            _shield.color = _shield.color.WithAlpha(0.3f);
        }
        public void Hide(float time = 2f)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_renderer.DOColor(_renderer.color.WithAlpha(0f), time));
            sequence.Join(_shield.DOColor(_shield.color.WithAlpha(0f), time));
            sequence.SetLink(gameObject);
            sequence.SetEase(Ease.Linear);
            sequence.OnComplete(Dispose);
            sequence.Play();
        }
        public void Dispose()
        {
            _pool?.Despawn(this);
            Disposed?.Invoke(this);
        }   

        void IPoolable<IMemoryPool>.OnDespawned()
        {
            _pool = null;
            ResetState();
        }
        void IPoolable<IMemoryPool>.OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            Show();
            InitState();

            int rand = UnityEngine.Random.Range(0, _settings.Skins.Length);
            _library.spriteLibraryAsset = _settings.Skins[rand];
        }

        public class Factory : PlaceholderFactory<CatView> { }
    }
}