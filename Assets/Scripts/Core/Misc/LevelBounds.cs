using System;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelBounds : MonoBehaviour
    {
        private BoxCollider2D _collider;
        private Camera _camera;
        public Vector2 Size
        {
            get => new Vector2(_camera.orthographicSize * _camera.aspect * 2f, _camera.orthographicSize * 2f);
        }

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _collider.isTrigger = true;
            _camera = Camera.main;
        }
        private void Start()
        {
            UpdateBounds();
        }

#if UNITY_EDITOR
        private void Update()
        {
            UpdateBounds();
        }
#endif
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDisposable disposableObj))
            {
                disposableObj.Dispose();
            }
        }

        private void UpdateBounds()
        {
            _collider.size = Size;
        }
    }
}
