using UnityEngine;
using Zenject;
using VolumetricLines;
using Core.Cats;
using Core.Infrastructure.Signals.Cats;

namespace Core.Enemy
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(VolumetricLineBehavior))]
    public class Laser : MonoBehaviour
    {
        private SignalBus _signalBus;
        private LevelBounds _levelBounds;
        private BoxCollider2D _collider;
        private VolumetricLineBehavior _lineRenderer;

        [Inject]
        private void Construct(SignalBus signalBus, LevelBounds levelBounds)
        {
            _signalBus = signalBus;
            _levelBounds = levelBounds;
        }

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _lineRenderer = GetComponent<VolumetricLineBehavior>();
        }
        private void Start()
        {
            gameObject.SetActive(false);
            SetWidth(_levelBounds.Size.x * 2f);
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out CatView cat) && !cat.IsInvisible)
            {
                _signalBus.Fire(new CatKidnappedSignal { KidnappedCat = cat });
            }
        }
#if UNITY_EDITOR
        private void Update()
        {
            SetWidth(_levelBounds.Size.x * 2f);
        }
#endif

        private void SetWidth(float width)
        {
            _lineRenderer.EndPos = Vector3.up * width;
            _collider.offset = new Vector2(_collider.offset.x, width / 2f);
            _collider.size = new Vector2(_collider.size.x, width);
        }

        public class Factory : PlaceholderFactory<Laser> { }
    }
}
